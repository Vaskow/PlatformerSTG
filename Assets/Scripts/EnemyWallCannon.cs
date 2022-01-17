using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCannon : MonoBehaviour {

    private AngleChecked angler;

    public float shoodDelay = 1.5f;
    private float shootDelayCounter;

    public float delay = 0.3f; //задержка поворота пушки
    private float delayCounter;

    private float currentAngle; //угол поворота пушки
    private float anglToPlayer; //угол до игрока

    public GameObject projectile;
    public GameObject Dulo;

    private bool isActive = false; //активность пушки

    private void Start()
    {
        angler = GetComponent<AngleChecked>();
        delayCounter = delay;
    }
    // Update is called once per frame
    void Update () {

        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angler.checkAngle());
        ////даем нужный поворот пушке по рассчитанному углу на игрока (поворот сразу)

        if (!isActive) return;

        if (delayCounter <= 0)
        {
            delayCounter = delay;
            anglToPlayer = angler.checkAngle();
            if (anglToPlayer < 0) anglToPlayer += 360; //исключаем получение отрицательных углов
            currentAngle = Mathf.Round(transform.rotation.eulerAngles.z); //округляем угол поворота, чтобы не было погрешностей

            if (currentAngle - anglToPlayer > 0)
            {
                if (currentAngle - anglToPlayer > 180)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentAngle + 30);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentAngle - 30);
                }
            }
            else if (currentAngle - anglToPlayer < 0)
            {
                if (currentAngle - anglToPlayer < -180)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentAngle - 30);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentAngle + 30);
                }
            }
            else if (currentAngle == anglToPlayer) //пушка навелась на игрока
            {
                Shoot(); 
            }

        }
        else
        {
            delayCounter -= Time.deltaTime;
        }

        shootDelayCounter -= Time.deltaTime;

    }

    void Shoot()
    {
        if (shootDelayCounter <= 0)
        {
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
