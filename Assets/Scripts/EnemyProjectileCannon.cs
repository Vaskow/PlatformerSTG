using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyProjectileCannon : MonoBehaviour {

    private GameObject parent; //для проверки пули и игрока (родителя)
    public GameObject Parent { set { parent = value; } get { return parent; } }

    Rigidbody2D myRigidbody;
    public float movespeed;
    private int scale;

    private void Start()
    {
        scale = Mathf.Clamp((int)(FindObjectOfType<Character>().transform.position.x * 10000 - transform.position.x * 10000), -1, 1);
        myRigidbody = GetComponent<Rigidbody2D>(); //скорость пули строчкой ниже ускоряется от взятия powerUp типа R
        myRigidbody.AddRelativeForce(Vector2.right * movespeed, ForceMode2D.Impulse);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        if (transform.parent != null) Destroy(transform.parent.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collider) //сработал триггер, когда пуля касается какого-то триггера
    { //попала пуля в unit -> должна уничтожится 



    }
}
