using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootLieDown : EnemySniperRegular {

    private Animator _anim;
       
    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (FindObjectOfType<Character>().transform.position.x < transform.position.x) //разворот на игрока
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
}
