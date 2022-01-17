using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBar : MonoBehaviour {

    private Transform[] hearts = new Transform[5]; //массив иконок здоровья

    private Character character;

    private void Awake()
    {
        character = FindObjectOfType<Character>(); //находим объект типа character

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = transform.GetChild(i); //передаем индекс объекта, который хотим получить
        }
    }

    public void Refresh() //обновляем поле с жизнями
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < character.Lives) hearts[i].gameObject.SetActive(true); //делаем активыми только те, которые есть у игрока
            else hearts[i].gameObject.SetActive(false); //остальные делаем неактивными 
        }
    }
}
