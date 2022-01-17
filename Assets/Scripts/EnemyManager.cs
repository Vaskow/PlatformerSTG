using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject enemy_paint; //противник, если ему нужно сменить цвет при уроне
    public int health; //max
    private int currentHealth; //cur
    public bool invisible; //неуязвимость (использовалась при закрытом wall_container)

    float green = 1f;
    float blue = 1f;
    public float diff = 0.05f; //переменная для изменения цвета (green и blue) при дальнейших попаданиях
    //выставлено стандартное значение

    public GameObject DealthEffectFirst;
    public GameObject DealthEffectSecond;

    // Use this for initialization
    void Start () {
        currentHealth = health;
        invisible = false;
        if (health < 100)
        {
            diff = (100f / health) / 100f;
        }
    }

    public void TakeDamage() //получает урон
    {
        if (invisible) return; //не получаем урон
        health--;
        if (enemy_paint) //смена цвета спрайта противника при получениии урона
        {
            green -= diff;
            blue -= diff;
            enemy_paint.GetComponent<SpriteRenderer>().color = new Color(1f, green, blue, 1f);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die() //погибает
    {
        if (DealthEffectFirst != null)
        {
            Instantiate(DealthEffectFirst, transform.position, transform.rotation);
        }
        if (DealthEffectSecond != null)
        {
            Instantiate(DealthEffectSecond, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
	
}
