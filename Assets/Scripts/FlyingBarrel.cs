using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBarrel : MonoBehaviour {


    //Бочка с оружием будет лететь по графику sin(x)
    public float movespeed;
    public float y;

    public float amplitude = 1; //амплитуда
    public float frequency = 1; //частота 

    // Use this for initialization
    void Start () {

        y = transform.position.y;
        
    }

    public void Activate() //придает ускорение бочке
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * movespeed, ForceMode2D.Impulse);
    }


    // Update is called once per frame
    void Update () {

        //движение объекта будет по sin(x)  Амплитуда * sin(Частота* x) 
        transform.position = new Vector3(transform.position.x, y + amplitude * Mathf.Sin(transform.position.x * frequency));


	}
}
