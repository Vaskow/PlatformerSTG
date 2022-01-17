using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //добавляем для проверки игрока в коллайдерах монстра 


public class ShootableMonster : Monster {

    public GameObject target; //объект игрока

    [SerializeField]
    private float rate = 2.0f;
    private bool directionBool = true; // направление для поворота относительно игрока

    public float shootDelay; //интервал, через который будет стрелять персонаж
    private float shootDelayCounter; //счетчик стрельбы 

    [SerializeField]
    private Color bulletColor = Color.white; //цвет для пули монстра

    private BulletEnemy bulletEnemy;

    private SpriteRenderer sprite;

    private Vector3 direction; //направление


    protected override void Awake() //получаем пулю
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        bulletEnemy = Resources.Load<BulletEnemy>("BulletEnemy");
    }

    protected override void Update()
    {

        //Shoot(); // стрельба юнита с таймером внутри

        //if (target.transform.position.x < transform.position.x && directionBool) //поворот, в замисимости от положения игрока
        //{
        //    direction = -direction;
        //    sprite.flipX = -direction.x < 0.0F;
        //    directionBool = false;
        //}
        //else if (target.transform.position.x > transform.position.x && !directionBool)
        //{
        //    direction = -direction;
        //    sprite.flipX = -direction.x < 0.0F;
        //    directionBool = true;
        //}
        if (FindObjectOfType<Character>().transform.position.x < transform.position.x) //разворот на игрока
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void Start()
    {
        target = GameObject.Find("Character"); //получили объект игрока
        direction = -transform.right;
        shootDelayCounter = 0; //счетчик ставим на 0 изначально.
        
        //InvokeRepeating("Shoot", rate, rate); //метод для постоянной стрельбы
       // InvokeRepeating("Shoot", 2.10f, 2.10f); //метод для постоянной стрельбы
    }

    //private void Shoot()
    //{
    //    if (shootDelayCounter <= 0)
    //    {
    //        Vector3 position = transform.position; position.x = position.x + (sprite.flipX ? -0.8f : 0.8f);//position.x -= 0.6f; //позиция вылета пули
    //        BulletEnemy newBullet = Instantiate(bulletEnemy, position, bulletEnemy.transform.rotation) as BulletEnemy;

    //        newBullet.Parent = gameObject; //родитель пули наш объект монстр
    //                                       //newBullet.Direction = newBullet.transform.right * direction.x; //будет стрелять влево
    //        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.5f : 1.5f);
    //        newBullet.Color = bulletColor; //задаем цвет для пули монстра

    //        shootDelayCounter = shootDelay;
    //    }
    //    shootDelayCounter -= Time.deltaTime;

    //}

    protected override void OnTriggerEnter2D(Collider2D collider)
    {//переопределили метод, чтобы монстр не убивал себя и добавим уничтожение прыжком
        Bullet bullet = collider.GetComponent<Bullet>();
        Unit unit = collider.GetComponent<Unit>(); //проверка коллайдеров 

        if (bullet && gameObject != bullet.Parent ) //если это пуля и её родитель не наш юнит
        {
            ReceiveDamage(); //получаем дамаг
        }
        if (unit && unit is Character) //если игрок коснулся монстра
        {
            unit.ReceiveDamage(); //нам наносится дамаг
        }
    }

   
}
