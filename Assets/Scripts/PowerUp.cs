using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    // 0 - R
    // 1 - M
    // 3 - F
    // 4 - L

    public int type; // тип оружия
    public float height = 4; //высота вылета

    private void Start()
    {
       //при выпадении оружия из бочки создаем импульс
       GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * height, ForceMode2D.Impulse); 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && type != 0)
        {
            collision.GetComponent<Character>().ChangeWeapon(type);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && type == 0)
        {
            if (Character.rapidsPicked < 2) Character.rapidsPicked++; //берем не больше 2 ускорителей R
            Destroy(gameObject);
        }

    }
}
