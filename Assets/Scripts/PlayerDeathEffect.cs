using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathEffect : MonoBehaviour {

    public float impulseX;
    public float impulseY;

    private void Start()
    {
        Rigidbody2D myRigidbody = GetComponent<Rigidbody2D>();
        transform.localScale = FindObjectOfType<Character>().transform.localScale; //поворачиваем спрайт в сторону куда смотрел игрок при гибели

        if (transform.localScale.x > 0)
        {
            myRigidbody.AddRelativeForce(Vector2.left * (impulseX + Character.rapidsPicked * Character.projectileSpeedKoef), ForceMode2D.Impulse);
        }
        else
        {
            myRigidbody.AddRelativeForce(Vector2.right * (impulseX + Character.rapidsPicked * Character.projectileSpeedKoef), ForceMode2D.Impulse);
        }
        myRigidbody.AddRelativeForce(Vector2.up * (impulseY + Character.rapidsPicked * Character.projectileSpeedKoef), ForceMode2D.Impulse);

    }


}
