using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour {

    private GameObject parent; //для проверки пули и игрока (родителя)
    public GameObject Parent { set { parent = value; } get { return parent; } }
    public GameObject ImpactEffect;

    Rigidbody2D myRigidbody;
    public float speed = 10.0f;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } } //чтобы задать направление пуле

    public Color Color //чтобы можно было задать цвет пули для игровых объектов
    {
        set { sprite.color = value; }
    }

    public SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
       myRigidbody = GetComponent<Rigidbody2D>();
       myRigidbody.AddRelativeForce(Vector2.right * speed, ForceMode2D.Impulse);
       Destroy(gameObject, 1.4F); //уничтожаем нашу пулю с задержкой 1.4 сек
    }

    private void Update()
    {
        //Пока убрал пролет через препятствия 
       // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F); //проверяет траекторию пули на препятствия (блоки)
       //// transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

       // if (colliders.Length > 1 && colliders.All(x => !x.GetComponent<Unit>()) && colliders.All(x => !x.GetComponent<Heart>()))
       // {
       //     Destroy(gameObject);
       // }
        //откуда, куда, скорость на время м/у кадрами
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) //сработал триггер, когда пуля касается какого-то триггера
    { //попала пуля в unit -> должна уничтожится 

        Unit unit = collider.GetComponent<Unit>();
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * direction.x * 0.3F, 0.5F);

        if (unit && unit.gameObject != parent)
        {
            if(!(unit is MoveRobot)) unit.ReceiveDamage(); //наносит дамаг всем кроме двигающегося монстра
            Destroy(gameObject); // уничтожаем пулю
        }

        //добавил с блоком врагов
        if (collider.tag == "Enemy") //Проверка на тэг Enemy
        {
            if (collider.GetComponent<EnemyManager>() != null) //у объекта есть компонент EnemyManager
            {
                collider.GetComponent<EnemyManager>().TakeDamage();
                Instantiate(ImpactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        //if (colliders.Length > 0)
        //{
        //    Destroy(gameObject);
        //}

    }
}
