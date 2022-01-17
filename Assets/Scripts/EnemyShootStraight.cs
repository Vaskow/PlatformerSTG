using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootStraight : MonoBehaviour {

    public float shoodDelay = 1.5f;
    private float shootDelayCounter;

    public GameObject projectile;
    public GameObject Dulo;

    private bool isActive = false; //активность пушки

    private void Start()
    {
    
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        Shoot();
        
        shootDelayCounter -= Time.deltaTime;

    }

    void Shoot()
    {
        if (shootDelayCounter <= 0)
        {
            if (FindObjectOfType<Character>().transform.position.x < transform.position.x) //разворот на игрока
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            }
            Instantiate(projectile, Dulo.transform.position, transform.rotation);
            shootDelayCounter = shoodDelay;
        }
    }

    private void OnBecameVisible()
    {
        isActive = true;
    }

    private void OnBecameInvisible()
    {
        isActive = false;
    }
}
