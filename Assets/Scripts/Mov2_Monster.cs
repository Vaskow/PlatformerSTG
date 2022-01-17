using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //добавляем для проверки игрока в коллайдерах монстра (методе move)

public class Mov2_Monster : Monster
{

    [SerializeField]
    private float speed = 2.0f;

    private Vector3 direction; //для движения монстра

    private Bullet bullet;

    private SpriteRenderer sprite;

    protected override void Awake() //переопределили метод
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet"); //получаем из ресурсов объект пуля 
    }

    protected override void Start()
    {
        direction = transform.right; //при старте направление будет вправо
    }

    protected override void Update()
    {
        Move(); //вызываем метод движения могнстра
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
     
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.collider.GetComponent<Unit>();

        if (unit && unit is Character) //если игрок коснулся монстра 
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3f) ReceiveDamage(); //наносим дамаг монстру (прыгнул на него)
                                                                                                     // (смотрим, что модуль расстояния может быть лишь тогда, когда мы над монстром
            else unit.ReceiveDamage(); //нам наносится дамаг
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction.x * 0.4F, 0.1F);
        //смотрим все коллайдеры (добавл. в массив) через круг выше и правее нашей опорной точки (* на direction.x ,где -1 или 1 в зависимости от направления)
        //домножили на 0.4, чтобы центр круга не был слишком далеко, с радиусом 0.1  (параметры брать осторожно, т.к. может свой коллайдер посчитать) 

        //Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position - transform.up * 0.2f + transform.right * direction.x * 0.1F, 0.1F);
        //if (colliders2.Length < 1) direction *= -1.0F; //если нет коллайдеров, то меняем направление

        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()) && colliders.All(x => !x.GetComponent<Bullet>())) direction *= -1.0F; //если есть, то разворачиваем монстра
        //true, если для всех элементов массива предикарт выведет true , а если будет коллайдер игрока, то будет false и он не будет менять направление
        //добавили также для пули
        sprite.flipX = -direction.x < 0.0F; //делаем разворот спрайта, чтобы повернут был по направлению движения
        //для проверки от падения, делать таким же образом, но круг располагать ниже и проверять на отсутствие в if

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
