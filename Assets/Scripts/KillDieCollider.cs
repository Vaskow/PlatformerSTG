using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDieCollider : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "DieCollider")
        {
            Destroy(gameObject); //уничтожаем объект при касании с коллайдером
        }

    }
}
