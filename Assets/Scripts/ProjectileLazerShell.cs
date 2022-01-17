using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLazerShell : MonoBehaviour {

    public GameObject projectile;
    public float delay; //временной промежуток между 4-мя выстрелами
    private float delayCounter;//счетчик
    public int projectilesCount;//количество пуль


	// Use this for initialization
	void Start () {
        Instantiate(projectile, transform.position, transform.rotation); //первый выстрел после нажатия на кнопку
        projectilesCount--; //уменьшили на 1
        delayCounter = delay; //запуск счетчика
	}
	
	// Update is called once per frame
	void Update () {
        if (projectilesCount > 0 && delayCounter <= 0)
        {
            Instantiate(projectile, transform.position, transform.rotation); //выстрел после нажатия на кнопку
            projectilesCount--; //уменьшили
            delayCounter = delay; 
        }
        else
        {
            delayCounter -= Time.deltaTime;
        }

        if (projectilesCount <= 0) Destroy(gameObject);

	}
}
