using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clas_RunnerSpawnPoint : MonoBehaviour {

    //public Animator anim;
    //private bool active;
    private bool done;
    public GameObject runner;

    // Use this for initialization
    void Start()
    {
        //active = false;
        done = false;
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpawnBunker();
        }
    }
    // Update is called once per frame
    //void Update()
    //{
    //    anim.SetBool("Active", active);
    //}

    //private void OnBecameVisible()
    //{
    //    active = true;
    //}

    public void SpawnBunker()
    {
        if (!done) //спавн противников только 1 раз
        {
            Vector3 pos = transform.position;
            pos.x += 1f;
            Instantiate(runner, pos, transform.rotation);
            pos.x -= 2f;
            Instantiate(runner, pos, transform.rotation);
        }
        Destroy(gameObject); //удаляем оболочку-анимацию
        done = true; //защита от повторного запуска функции
    }
}
