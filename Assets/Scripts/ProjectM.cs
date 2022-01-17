using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectM : MonoBehaviour {

    private GameObject parent; //для проверки пули и игрока (родителя)
    public GameObject Parent { set { parent = value; } get { return parent; } }

    Rigidbody2D myRigidbody;
    public float movespeed;
    public float spinningSpeed; //скорость вращения пули
    private int scale;

    private void Start()
    {
        scale = Mathf.Clamp((int)(FindObjectOfType<Character>().transform.position.x * 10000 - transform.position.x * 10000), -1, 1);
        myRigidbody = GetComponent<Rigidbody2D>(); //скорость пули строчкой ниже ускоряется от взятия powerUp типа R
        myRigidbody.AddRelativeForce(Vector2.right * (movespeed + Character.rapidsPicked*Character.projectileSpeedKoef), ForceMode2D.Impulse);
        if (scale == -1) { myRigidbody.angularVelocity = spinningSpeed; } //чтобы импульс был симметричным для разных направлений
        else myRigidbody.angularVelocity = -spinningSpeed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        if (transform.parent != null) Destroy(transform.parent.gameObject);
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F); //проверяет траекторию пули на препятствия (блоки)
                                                                                       // transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        if (colliders.Length > 1 && colliders.All(x => !x.GetComponent<Unit>()) && colliders.All(x => !x.GetComponent<Heart>()) && colliders.All(x => !x.GetComponent<Character>()))
        {
            //Destroy(gameObject);
        }
        //откуда, куда, скорость на время м/у кадрами
    }

    private void OnTriggerEnter2D(Collider2D collider) //сработал триггер, когда пуля касается какого-то триггера
    { //попала пуля в unit -> должна уничтожится 

        Unit unit = collider.GetComponent<Unit>();
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * direction.x * 0.3F, 0.5F);

        if (unit && unit.gameObject != parent && !(unit is Character))
        {
            if (!(unit is MoveRobot) /*&& !(unit is Character)*/ ) unit.ReceiveDamage(); //наносит дамаг всем кроме двигающегося монстра
           // Destroy(gameObject); // уничтожаем пулю
        }

        //блок добавлен для врагов Enemy
        if (collider.tag == "Enemy") //Проверка на тэг Enemy
        {
            if (collider.GetComponent<EnemyManager>() != null) //у объекта есть компонент EnemyManager
            {
                collider.GetComponent<EnemyManager>().TakeDamage(); //вызываем функцию получения урона противником
                Destroy(gameObject);
                if (transform.parent != null) Destroy(transform.parent.gameObject);
            }
        }

        //if (colliders.Length > 0)
        //{
        //    Destroy(gameObject);
        //}

    }
}
