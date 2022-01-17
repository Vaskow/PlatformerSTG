using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyInWater : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy") //Проверка на тэг Enemy
        {
            if (collider.GetComponent<EnemyManager>() != null) //у объекта есть компонент EnemyManager
            {
                collider.GetComponent<EnemyManager>().TakeDamage(); //вызываем функцию получения урона противником
            }
        }
    }
}
