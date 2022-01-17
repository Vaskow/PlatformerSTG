using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContainer : MonoBehaviour {


    public Animator anim;
    private bool closed; //закрыт ли ящик
    private float distance;
    public float range; //на каком расстоянии ящик открывается и закрывается


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("closed", closed);
        distance = transform.position.x - FindObjectOfType<Character>().transform.position.x;
        closed = distance > range || distance < -range;
        GetComponent<EnemyManager>().invisible = closed; //неуязвимость зависит от закрытия 
    }
}
