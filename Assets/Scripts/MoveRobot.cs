using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class MoveRobot : Monster {

    [SerializeField]
    private float rate = 2.0f;
    private float speed = 2.5f;

    public GameObject target; //объект игрока
    //private bool directionBool = true; // направление для поворота относительно игрока
    private float MaxDistance = 8.0f; //максимальное расстояние между игроком и юнитом

    [SerializeField]
    private Color bulletColor = Color.white; //цвет для пули
    private Vector3 direction; //для движения монстра

    private BulletEnemy bulletEnemy;

    private int HealthRobot = 3;

    [SerializeField]
    private float jumpForce = 6.0f;
    private bool isGrounded = false; //на поверхности или нет

    private SpriteRenderer sprite;

    new private Rigidbody2D rigidbody;

    protected override void Awake() //переопределили метод
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        bulletEnemy = Resources.Load<BulletEnemy>("BulletEnemy"); //получаем из ресурсов объект пуля 
    }

    private void FixedUpdate() //вызывается в фиксированное время для проверки физики
    {
        CheckGround(); //наличие земли под юнитом
    }

    protected override void Start()
    {
        target = GameObject.Find("Character"); //получили объект игрока
        direction = -transform.right; //при старте направление будет влево
        InvokeRepeating("Shoot", rate, 0.7f * rate); //метод для постоянной стрельбы
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.55f; position.x = position.x + (sprite.flipX ? 0.6f : -0.6f);//position.x -= 0.6f; //позиция вылета пули
        BulletEnemy newBullet = Instantiate(bulletEnemy, position, bulletEnemy.transform.rotation) as BulletEnemy;

        newBullet.Parent = gameObject; //родитель пули наш объект монстр
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? 1.3f : -1.3f);
        newBullet.Color = bulletColor; //задаем цвет для пули монстра
    }

    protected override void Update()
    {
       // ChangeDirection(); //привязка к игроку на определенное расстояние добавить таймер
        Move(); //вызываем метод движения могнстра
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        Bullet bullet = collider.GetComponent<Bullet>();

        if (bullet && HealthRobot == 0) //если это пуля
        {
            ReceiveDamage(); //получаем дамаг
        }
        else if (bullet && HealthRobot != 0)
        {
            HealthRobot--;
        }

        if (unit && unit is Character) //если игрок коснулся монстра 
        {
            unit.ReceiveDamage(); //нам наносится дамаг
        }
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce /** 0.4f*/ + /*0.4f **/ transform.right * (sprite.flipX ? -0.1f : 0.1f), ForceMode2D.Impulse);
    }

    private void CheckGround() //есть ли земля под нами 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F); //проверять будем наличие коллайдеров( в массив добавит)

        isGrounded = colliders.Length > 1; //свой коллайдер учитываем (поэтому > 1) true false

      
    }

    private void ChangeDirection() //привязка к игроку на определенное расстояние
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > MaxDistance /*&& directionBool*/) //поворот, в замисимости от расстояния до игрока
        {
            direction = -direction;
            sprite.flipX = -direction.x < 0.0F;
            //directionBool = false;
        }
        //else if (Mathf.Abs(target.transform.position.x - transform.position.x) > MaxDistance && !directionBool)
        //{
        //    direction = -direction;
        //    sprite.flipX = -direction.x < 0.0F;
        //    directionBool = true;
        //}
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.collider.GetComponent<Unit>();
        Bullet bullet = collision.collider.GetComponent<Bullet>();

        if (bullet ) //если это пуля
        {
            ReceiveDamage(); //получаем дамаг
        }

        if (unit && unit is Character) //если игрок коснулся монстра 
        {
           unit.ReceiveDamage(); //нам наносится дамаг
        }

        if(collision.gameObject.name == "DieCollider")
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction.x * 0.4F, 0.1F);
        //смотрим все коллайдеры (добавл. в массив) через круг выше и правее нашей опорной точки (* на direction.x ,где -1 или 1 в зависимости от направления)
        //домножили на 0.4, чтобы центр круга не был слишком далеко, с радиусом 0.1  (параметры брать осторожно, т.к. может свой коллайдер посчитать) 

        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position + transform.up * 1.15f + transform.right * direction.x * 0.7F, 0.4F);
        //делаем прыжки, если есть озможность перепрыгнуть препятствие
        if (colliders2.Length == 1 && colliders.All(x => !x.GetComponent<Unit>()) && colliders.All(x => !x.GetComponent<Bullet>()) && colliders.All(x => !x.GetComponent<Heart>()) && isGrounded) { Jump(); }

        //чтобы не прыгнул в пропасть, но нужно сделать таймер поворота, чтобы не менял направление часто про возникновении ситуации
        //Collider2D[] colliders3 = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.4f + transform.right * direction.x * 3.6F, 2.4F);
        //if (colliders3.Length == 0 && colliders.All(x => !x.GetComponent<Unit>()) && colliders.All(x => !x.GetComponent<Bullet>())) direction *= -1.0F;

        if (colliders.Length > 0 && colliders.All(x => x.GetComponent<Block>()) /*&& colliders.All(x => !x.GetComponent<Bullet>()) && colliders.All(x => !x.GetComponent<Heart>())*/) direction *= -1.0F; //если есть, то разворачиваем монстра
        //true, если для всех элементов массива предикарт выведет true , а если будет коллайдер игрока, то будет false и он не будет менять направление
        //добавили также для пули
        sprite.flipX = -direction.x < 0.0F; //делаем разворот спрайта, чтобы повернут был по направлению движения
        //для проверки от падения, делать таким же образом, но круг располагать ниже и проверять на отсутствие в if

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
