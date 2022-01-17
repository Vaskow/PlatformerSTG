using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletContr : MonoBehaviour {

    Rigidbody2D myRigidbody;
    public float movespeed;
    public float speed = 10.0f;
    private GameObject parent; //для проверки пули и игрока (родителя)
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } } //чтобы задать направление пуле

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.AddRelativeForce(Vector2.right * movespeed, ForceMode2D.Impulse);
        Destroy(gameObject, 1.4F); //уничтожаем нашу пулю с задержкой 1.4 сек
    }
	
	// Update is called once per frame
	void Update () {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F); //проверяет траекторию пули на препятствия (блоки)
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        if (colliders.Length > 1 && colliders.All(x => !x.GetComponent<Unit>()) && colliders.All(x => !x.GetComponent<Heart>()))
        {
            Destroy(gameObject);
        }
        //откуда, куда, скорость на время м/у кадрами
    }
    private void OnTriggerEnter2D(Collider2D collider) //сработал триггер, когда пуля касается какого-то триггера
    { //попала пуля в unit -> должна уничтожится 

        Unit unit = collider.GetComponent<Unit>();
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * direction.x * 0.3F, 0.5F);

        if (unit && unit.gameObject != parent)
        {
            if (!(unit is MoveRobot)) unit.ReceiveDamage(); //наносит дамаг всем кроме двигающегося монстра
            Destroy(gameObject); // уничтожаем пулю
        }
        //if (colliders.Length > 0)
        //{
        //    Destroy(gameObject);
        //}

    }
}
