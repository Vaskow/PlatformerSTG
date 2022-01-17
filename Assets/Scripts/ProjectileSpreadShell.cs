using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpreadShell : MonoBehaviour {


    public GameObject projectile;
    public float range; //расстояние между пулями
    private Vector3 rot; //отвечает за поворот, зависит от количества пуль

	// Use this for initialization
	void Start () {
        rot = transform.localEulerAngles;//задаем поворот при создании

        //Будет создано 5 пуль с разными углами
        if (FindObjectsOfType<ProjectM>().Length < 10) //центральная пуля
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(rot.x, rot.y, rot.z));
            //создаем пулю в позиции расположения нашей "обертки" с углами
        }
        if (FindObjectsOfType<ProjectM>().Length < 10)
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(rot.x, rot.y, rot.z - range));
            //создаем пулю в позиции расположения нашей "обертки" с углами
        }
        if (FindObjectsOfType<ProjectM>().Length < 10)
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(rot.x, rot.y, rot.z + range));
            //создаем пулю в позиции расположения нашей "обертки" с углами
        }
        if (FindObjectsOfType<ProjectM>().Length < 10)
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(rot.x, rot.y, rot.z - range * 2));
            //создаем пулю в позиции расположения нашей "обертки" с углами
        }
        if (FindObjectsOfType<ProjectM>().Length < 10)
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(rot.x, rot.y, rot.z + range * 2));
            //создаем пулю в позиции расположения нашей "обертки" с углами
        }

        //Далее нужно избавиться от "обертки"
        Destroy(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
