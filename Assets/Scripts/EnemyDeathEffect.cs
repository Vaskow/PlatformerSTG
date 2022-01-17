using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//подбрасывание юнита и разворот к игроку при уничтожении
public class EnemyDeathEffect : MonoBehaviour {

    //private GameObject target;
    public float height;
    private int scale;

	// Use this for initialization
	void Start () {
        //target = GameObject.Find("Character");
        //target.transform.position.x
        scale = Mathf.Clamp((int)(FindObjectOfType<Character>().transform.position.x*10000 - transform.position.x*10000), -1, 1);
        //смотрит разницу между игроком и эффектом, приравнивает их -1 (правее игрока) или 1 (левее) в зависимости от ситуации
        transform.localScale = new Vector3(scale, 1, 1);

        if (GetComponent<Rigidbody2D>().velocity != null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-scale * 2, height); //для взлета объекта
        }



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
