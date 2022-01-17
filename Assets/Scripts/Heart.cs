using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>(); //находим коллайдер игрока
         
        if (character) //если нашли
        {
            character.Lives++; //добавляем ему жизней через set
            Destroy(gameObject); //удаляем объект
        }
    }

}
