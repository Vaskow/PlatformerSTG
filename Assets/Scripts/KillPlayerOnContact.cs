using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnContact : MonoBehaviour {

    public bool killSelf; //если нужно себя уничтожить (в случае пули)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<Character>().Death();
            if (killSelf) Destroy(gameObject);
        }

    }
}
