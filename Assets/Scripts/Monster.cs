using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit {

    protected virtual void Awake() { } //Нужны будут в moveble monster
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected virtual void OnTriggerEnter2D(Collider2D collider) //сработал триггер у монстра
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        Unit unit = collider.GetComponent<Unit>(); //проверка коллайдеров 

        if (bullet) //если это пуля
        {
            ReceiveDamage(); //получаем дамаг
        }
        if (unit && unit is Character) //если игрок коснулся монстра
        {
             unit.ReceiveDamage(); //нам наносится дамаг
        }
    }
}
