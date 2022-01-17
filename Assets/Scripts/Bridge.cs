using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    public EdgeCollider2D[] blocks; //взрывающиеся блоки моста
    public float Delay; //частота взрыва блоков
    float counter;
    public GameObject DeathEffect;
    bool blowing; //мост взрывается
    int i;

	// Use this for initialization
	void Start () {
        counter = 0;
        blocks = GetComponentsInChildren<EdgeCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!blowing) return;
        if (counter <= 0)
        {
            Destroy(blocks[i].gameObject);
            Instantiate(DeathEffect, blocks[i].gameObject.transform.position, blocks[i].gameObject.transform.rotation);
            i++;
            counter = Delay;
        }
        else
        {
            counter -= Time.deltaTime;
        }
        if (i == blocks.Length) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")  blowing = true;
    }

}
