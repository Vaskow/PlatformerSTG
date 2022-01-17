using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classic_EnemyRunner_Spawn : MonoBehaviour {

    public Animator anim;
    private bool active;
    private bool done;
    public GameObject Runner;

    // Use this for initialization
    void Start()
    {
        active = false;
        done = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Active", active);
    }

    private void OnBecameVisible()
    {
        active = true;
    }

    public void SpawnBunker()
    {
        if (!done) Instantiate(Runner, transform.position, transform.rotation); //спавним бункер
        Destroy(gameObject); //удаляем оболочку-анимацию
        done = true; //защита от повторного запуска функции
    }
}
