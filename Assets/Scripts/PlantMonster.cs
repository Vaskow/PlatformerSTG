using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonster : Monster {

    private Animator animator;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    } 
    protected override void Start() { }
    protected override void Update() { }

    //private CharStatePlant StatePlant
    //{
    //    get { return (CharStatePlant)animator.GetInteger("StatePlant"); }
    //    set { animator.SetInteger("StatePlant", (int)value); }
    //}

    protected override void OnTriggerEnter2D(Collider2D collider) //сработал триггер у монстра
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        Unit unit = collider.GetComponent<Unit>(); //проверка коллайдеров 

        if (bullet) //если это пуля
        {
            ReceiveDamage(); //получаем дамаг
        }
        if (unit && unit is Character) //если игрок коснулся монстра
        {
            //StatePlant = CharStatePlant.Attack;
            unit.ReceiveDamage(); //нам наносится дамаг
        }
    }
}

public enum CharStatePlant
{
    Idle,
    Attack
}

