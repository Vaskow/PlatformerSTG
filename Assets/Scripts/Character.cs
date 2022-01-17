using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit {


    private GameObject currentProjectile; //для пуль, которыми будет стрелять игрок
    public GameObject basicProjectile; //для добавления игроку пули
    public GameObject ProjectileM; //для оружия М
    public GameObject ProjectileF; //для оружия М
    public GameObject ProjectileS; //для оружия М
    public GameObject ProjectileL; //для оружия М

    public static int rapidsPicked = 0;//количество "R", которое можно взять игроку 
    public static float projectileSpeedKoef = 2; //скорость пуль (коэф. ускорения)


    private BoxCollider2D myColl; //коллайдер игрока

    public float shootDelay; // для стрельбы несколькими пулями
    private float shootDelayCounter; //счетчик стрельбы

    private float originColliderSize; //размер начального коллайдера
    private float originColliderOffset; //положение начального коллайдера
    public float duckColliderSize; //для положения лежа
    public float duckColliderOffset;

    [SerializeField]
    private int lives = 5;
    private float spawnX, spawnY; //стартовые координаты игрока

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value <= 5) lives = value; //больше 5 не будет жизней
            livesBar.Refresh(); //обновляем наше поле с lives
        }
    }
    private LivesBar livesBar; //создаем livesBar

    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float jumpForce = 11.0f;

    public LayerMask ground; //слои, которые считаются землей
    public LayerMask OneWay; //поверхность-платформа
    public LayerMask water;
    public Transform groundSensor; //проверка на землю
    private bool isGrounded = false; //на поверхности или нет
    private bool onPlatform; //игрок на платформе
    private bool onWater;
    //private bool animatJump; //если блокировать анимацию прыжка при падении

    private Bullet bullet;
    private Bullet bullet2;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool isActive; //игрок жив
    private bool isDead; //игрок погиб
    public float invincibilityTime; //время неуязвимости
    public float inactivityTime; //время респавна игрока
    private float invCounter; //два счетчика 
    private float inactCounter;
    public GameObject SpawnPoint;
    private int flashing = 0; //мигание игрока после гибели и спавна
    public GameObject DeathEffect;
    private float gravityCharacter = 1.2f; //масса игрока



    private Vector3 position; //вектор появления пули
    public float[] shootAngles; //массив углов стрельбы 
    private Quaternion rot; //переменная для хранения угла 

    public int directionNum; //будет нужна для поворота вверх - 8, вправо - 6, влево - 4, вниз - 2 
    //Направления
    private bool KeyLeft;
    private bool KeyRight;
    private bool KeyUp;
    private bool KeyDown;
    private bool KeyJump;
    private bool KeyAction;
    private bool KeyJumpOff;
    private bool NOjumped; //нельзя прыгать во время получения дамага

    //private float secondTime = 0; //таймер для анимации прыжка
    //private int timersecond = 0;


    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>(); //находим объекты 
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet"); //подгружаем пулю из папки ресурсов
        bullet2 = Resources.Load<Bullet>("Contr (M)");
    }

    private void Start()
    {
        transform.position = new Vector3(SpawnPoint.transform.position.x, SpawnPoint.transform.position.y, transform.position.z);//переносимся на точку спавна
        currentProjectile = basicProjectile; //присваиваем пулю 
        spawnX = transform.position.x;
        spawnY = transform.position.y;
        rot = new Quaternion(0, 0, 0, 0); //инициализировали угол для стрельбы
        shootDelayCounter = 0; //счетчик выстреленных пуль изначально 0
        myColl = GetComponent<BoxCollider2D>();
        originColliderSize = myColl.size.y;
        originColliderOffset = myColl.offset.y;

    }

    private void FixedUpdate() //вызывается в фиксированное время для проверки физики
    {
        CheckGround();
    }

    private void Spawn()
    {
        invCounter = invincibilityTime;
        rapidsPicked = 0; //зануляем бонус на ускорение пуль 
        currentProjectile = basicProjectile;//оружие теряется
        rigidbody.gravityScale = gravityCharacter;

    }

    public void Death()
    {
        //Instantiate(DeathEffect, transform.position, transform.rotation);
        if (KeyDown && onWater) return;
        if (invCounter > 0) return;
        else
        {
            isDead = true;
            isActive = false; //игрок неактивный
            inactCounter = inactivityTime;
            //transform.position = SpawnPoint.transform.position;//переносимся на точку респавна
            Instantiate(DeathEffect, transform.position, transform.rotation); //создаем объект анимации смерти игрока
            gravityCharacter = rigidbody.gravityScale;
            rigidbody.velocity = Vector3.zero; //ускорение обнуляем
            transform.position = new Vector3(SpawnPoint.transform.position.x, SpawnPoint.transform.position.y, transform.position.z);//переносимся на точку респавна
            invCounter = invincibilityTime;
            rigidbody.gravityScale = 0;
            Lives = 5;
        }
    }



    private void Update()
    {

        if (!isActive) //проверка на "активность игрока"
        {
            inactCounter -= Time.deltaTime; //работает счетчик неактивности игрока
            if (inactCounter < 0)
            {
                isActive = true; //игрок опять становится управляемым
                Spawn();//нужно заспавнить игрока
            }
            return;
        }

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        Debug.Log(sprites.Length);
        if (invCounter > 0)
        {
            invCounter -= Time.deltaTime;
            flashing++;
            if (flashing > 15) //мигание
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].GetComponent<SpriteRenderer>().enabled = !sprites[i].GetComponent<SpriteRenderer>().enabled;
                    if (i == 0 && onWater && State == CharState.LayRobot) { sprites[0].GetComponent<SpriteRenderer>().enabled = false; } //если на воде и лежим => не показываем спрайт
                    if (i == 1 && !onWater || i == 1 && State == CharState.LayRobot_Water) { sprites[1].GetComponent<SpriteRenderer>().enabled = false; } //рябь на воде откл не на воде
                    if (i == 1 && onWater && State != CharState.LayRobot_Water) { sprites[1].GetComponent<SpriteRenderer>().enabled = enabled; }
                }
                flashing = 0;

            }

        }
        else
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].GetComponent<SpriteRenderer>().enabled = true;
                if (i == 0 && onWater && State == CharState.LayRobot) { sprites[0].GetComponent<SpriteRenderer>().enabled = false; } //если на воде и лежим => не показываем спрайт
                if (i == 0 && !onWater) { sprites[0].GetComponent<SpriteRenderer>().enabled = true; } //на воде показываем
                if (i == 1 && !onWater || i == 1 && State == CharState.LayRobot_Water) { sprites[1].GetComponent<SpriteRenderer>().enabled = false; }
                if (i == 1 && onWater && State != CharState.LayRobot_Water) { sprites[1].GetComponent<SpriteRenderer>().enabled = enabled; }
            }
            flashing = 0;
        }


        if (isGrounded) { State = CharState.Idle; /*NOjumped = false; */  }
        if (onWater) { State = CharState.Idle_Water; }
        //if (!isGrounded && !Input.GetButtonDown("Jump") && timersecond <= 4)
        //{
        //    State = CharState.Idle;
        //    while (timersecond <= 4)
        //    {
        //        secondTime += Time.deltaTime;
        //        if (secondTime >= 1)
        //        {
        //            timersecond += 1;
        //            secondTime = 0;
        //        }
        //    }
        //}
        //if (Input.GetButtonDown("Fire1")) Shoot(); //если нажата ЛКМ
        if (Input.GetButton("Horizontal")) Run();  //нажаты стрелки влево или вправо
        if ((directionNum == 7 || directionNum == 9) && isGrounded) { State = CharState.ShootRightUP;}
        if ((directionNum == 7 || directionNum == 9) && onWater) { State = CharState.ShootRightUP_Water; }
        if ((directionNum == 1 || directionNum == 3) && isGrounded && !onWater) { State = CharState.ShootRightDown; }
        if (directionNum == 8 && isGrounded) { State = CharState.ShootUP; }
        if (directionNum == 8 && onWater) { State = CharState.ShootUp_Water; }
        //if (isGrounded && KeyDown && !KeyRight && !KeyLeft) { State = CharState.LayRobot; }
        if (isGrounded && KeyDown && Input.GetButtonDown("Fire1") && !KeyRight && !KeyLeft/*&& directionNum == 6*/) { State = CharState.ShootLay; }

        if (isGrounded && Input.GetButtonDown("Jump") /*&& !NOjumped*/)
        {
            if (!onWater) jumpForce = 11.0f;
            else jumpForce = 8.8f;
            Jump();
        } //мы на земле и зажат пробел

        GetInput();
        if (isGrounded && KeyDown && !KeyRight && !KeyLeft)
        {
            myColl.size = new Vector2(myColl.size.x, duckColliderSize);
            myColl.offset = new Vector2(myColl.offset.x, duckColliderOffset);
            if (onWater)
            {
                State = CharState.LayRobot_Water;
            }
            else
            State = CharState.LayRobot;

        }
        else
        {
            myColl.size = new Vector2(myColl.size.x, originColliderSize);
            myColl.offset = new Vector2(myColl.offset.x, originColliderOffset);
        }

        CalculateDirection(); //рассчитыаем направления
        CalculateShootAngles(); //рассчитываем углы стрельбы
        Shoot(); //метод стрельбы
    }

    private void Run()
    {
        float SpeedForce = 1.0f;
        if (isGrounded)
        {
            SpeedForce = 1.0f;
        }
        else
        {
            SpeedForce = 1.5f;
        }

        Vector3 direction = transform.right * Input.GetAxis("Horizontal"); //направление

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * SpeedForce * Time.deltaTime);

        //!!! sprite.flipX = direction.x < 0.0F;

        if (KeyRight) { transform.localScale = new Vector3(1, 1, 1); }
        if (KeyLeft) { transform.localScale = new Vector3(-1, 1, 1); }

        if (isGrounded && !(Input.GetButtonDown("Fire1") || KeyAction)) State = CharState.Run;
        if (onWater && !(Input.GetButtonDown("Fire1") || KeyAction)) State = CharState.Run_Water;
        if (isGrounded && (Input.GetButtonDown("Fire1") || KeyAction)) State = CharState.ShootRightRun;
        if (onWater && (Input.GetButtonDown("Fire1") || KeyAction)) State = CharState.ShootRightRun_water;
    }

    private void Jump()
    {
        State = CharState.Jump;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

    }

    private void Shoot()
    {
        if ((Input.GetButtonDown("Fire1") || KeyAction) && State == CharState.LayRobot_Water) return; //под водой не стреляем

        if (Input.GetButtonDown("Fire1") && currentProjectile == basicProjectile)
        {
            //анимация Shoot используется в воде, а Shoot_Water на земле
            if (isGrounded && onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot;
            if (isGrounded && !onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot_water;
            /* position = transform.position;*/ /*position.y += 0.46f; position.x = position.x + (sprite.flipX ? -0.5f : 0.5f);*/ //добавили, чтобы пуля летела выше опорной точки и правее её, учитывая направление спрайта
            Bullet newBullet = Instantiate(bullet, position, rot) as Bullet; //для создания пули (префаб, в какой позиции, ротацию из префаба взяли) возвращает компонент как пуля

            newBullet.Parent = gameObject; //задали character родителем пули, чтобы она не уничтожалась при соприкосновении с ним
            if (directionNum == 8 || directionNum == 2) { newBullet.Direction = newBullet.transform.right; }
            else
            { newBullet.Direction = newBullet.transform.right * (transform.localScale.x < 0 ? -1.0f : 1.0f); } //направление пули вправо * на наше направление, взятое из flipx (true, если налево, false - направо. т.е. * 1)
        }
        //if (KeyAction && currentProjectile == ProjectileM && shootDelayCounter <= 0/* && FindObjectsOfType<Bullet>().Length < 9*/)
        //{

        //    if (isGrounded) State = CharState.Shoot;

        //    Bullet newBullet2 = Instantiate(bullet2, position, rot) as Bullet; //для создания пули (префаб, в какой позиции, ротацию из префаба взяли) возвращает компонент как пуля

        //    newBullet2.Parent = gameObject; //задали character родителем пули, чтобы она не уничтожалась при соприкосновении с ним
        //    if (directionNum == 8 || directionNum == 2) { newBullet2.Direction = newBullet2.transform.right; }
        //    else
        //    { newBullet2.Direction = newBullet2.transform.right * (sprite.flipX ? -1.0f : 1.0f); } //направление пули вправо * на наше направление, взятое из flipx (true, если налево, false - направо. т.е. * 1)

        //    shootDelayCounter = shootDelay; //обнуляем счетчик
        //    //shootDelayCounter -= Time.deltaTime;
        //}
        if (KeyAction && currentProjectile == ProjectileM && shootDelayCounter <= 0/* && FindObjectsOfType<Bullet>().Length < 9*/)
        {

            if (isGrounded && onWater && !KeyDown && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot; //закомментил отсутствие анимации стрельбы при беге
            if (isGrounded && !onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot_water; //для стрельбы в воде (без нажатия доп. кнопок)

            if ((currentProjectile == ProjectileM) && FindObjectsOfType<ProjectM>().Length < 5)
            {
                Instantiate(currentProjectile, position, rot);
                shootDelayCounter = shootDelay;
            }
        }
        if (KeyAction && currentProjectile == ProjectileF && shootDelayCounter <= 0)
        {
            if (isGrounded && onWater && !KeyDown && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot; //закомментил отсутствие анимации стрельбы при беге
            if (isGrounded && !onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot_water;

            if ((currentProjectile == ProjectileF) && FindObjectsOfType<ProjectM>().Length < 5)
            {
                Instantiate(currentProjectile, position, rot);
                shootDelayCounter = shootDelay;
            }
        }
        if (KeyAction && currentProjectile == ProjectileS && shootDelayCounter <= 0)
        {
            if (isGrounded && onWater && !KeyDown && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot; //закомментил отсутствие анимации стрельбы при беге
            if (isGrounded && !onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot_water;

            if ((currentProjectile == ProjectileS) && FindObjectsOfType<ProjectM>().Length < 10)
            {
                Instantiate(currentProjectile, position, rot);
                shootDelayCounter = shootDelay;
            }
        }
        if (KeyAction && currentProjectile == ProjectileL && shootDelayCounter <= 0)
        {
            if (isGrounded && onWater && !KeyDown && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot; //закомментил отсутствие анимации стрельбы при беге
            if (isGrounded && !onWater && !KeyDown && State != CharState.ShootRightUP && State != CharState.ShootRightUP_Water && State != CharState.ShootUP && State != CharState.ShootUp_Water && State != CharState.ShootRightRun && State != CharState.ShootRightRun_water) State = CharState.Shoot_water;

            if (currentProjectile == ProjectileL) 
            {
                ProjectM[] projectile = FindObjectsOfType<ProjectM>();
                foreach (ProjectM p in projectile) //уничтожаем пули
                {
                    Destroy(p.gameObject);
                }
                ProjectileLazerShell[] shells = FindObjectsOfType<ProjectileLazerShell>();
                foreach (ProjectileLazerShell s in shells) //для каждого уничтожаем оболочку
                {
                    Destroy(s.gameObject);
                }


                Instantiate(currentProjectile, position, rot);
                shootDelayCounter = shootDelay;
            }
        }

        shootDelayCounter -= Time.deltaTime;

    }


    public override void ReceiveDamage() //обрабатываем дамаг 
    {
        NOjumped = true;
        Lives--; //отнимаем жизни связанные с livesBar
        if (lives <= 0)
        {
            Death();
            return;
        }

        //if (Lives == 0) { State = CharState.Die;  transform.position = new Vector3(spawnX, spawnY, transform.position.z); Lives = 5; } //если не осталось здоровья, переносимся в начало пути
        //if (Lives == 0) { State = CharState.Die; }

        rigidbody.velocity = Vector3.zero; //обнуляем ускорение (например, если падаем на obstacle)
        rigidbody.AddForce(transform.up * 6.0f + transform.right * (sprite.flipX ? 1.1f : -1.1f), ForceMode2D.Impulse); //подбрасываем при касании с ним вверх и в сторону, противоположную движению

        Debug.Log(lives); //вывод в консоль
    }

    private void CheckGround() //есть ли земля под нами 
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F); //проверять будем наличие коллайдеров( в массив добавит)
        //Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.3F); 
        //Bullet collBull = collider.GetComponent<Bullet>();
        //ProjectM collProjM = collider.GetComponent<ProjectM>();
        //if (collBull)
        //{
        //    if (!(collBull is Bullet)) { isGrounded = false; } //проверка для отсутствия прыжка при наличии пули под игроком
        //}
        //else if (collProjM)
        //{
        //    if (!(collProjM is ProjectM)) { isGrounded = false; } //проверка для отсутствия прыжка при наличии пули под игроком
        //}
        //else
        //{
        //    isGrounded = colliders.Length > 1; //свой коллайдер учитываем (поэтому > 1) true false
        //}

        isGrounded = Physics2D.OverlapCircle(groundSensor.position, 0.3f, ground); //проверка на землю
        onPlatform = Physics2D.OverlapCircle(groundSensor.position, 0.3f, OneWay); //проверка на платформу
        onWater = Physics2D.OverlapCircle(groundSensor.position, 0.3f, water); //проверка на воду

        if (!isGrounded) State = CharState.Jump;


        //спрыгивание с платформы
        if (onPlatform && KeyDown && Input.GetButtonDown("Jump"))
        {
            //Debug.Log("down");
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            isGrounded = false;
        }



    }



    void GetInput() //получаем информацию по направлениям
    {
        KeyLeft = Input.GetKey(KeyCode.LeftArrow);
        KeyRight = Input.GetKey(KeyCode.RightArrow);
        KeyUp = Input.GetKey(KeyCode.UpArrow);
        KeyDown = Input.GetKey(KeyCode.DownArrow);
        KeyJump = Input.GetKey(KeyCode.Z);
        KeyAction = Input.GetKey(KeyCode.LeftControl);
        KeyJumpOff = KeyDown && Input.GetButtonDown("Jump");

    }

    void CalculateDirection() //рассчитывает направление игрока 
    {
        if (KeyUp && !KeyRight && !KeyLeft && !KeyDown)
        {
            directionNum = 8;
        }
        else if (!isGrounded && KeyDown && !KeyLeft && !KeyRight) directionNum = 2;
        //else if (!sprite.flipX)
        else if (transform.localScale.x > 0)
        {
            if (KeyUp && KeyRight) directionNum = 9;
            else if (KeyDown && KeyRight) directionNum = 3;
            else if (/*KeyDown && !KeyRight &&*/!KeyDown && isGrounded) directionNum = 6; //был добавлен isGround
            else if (KeyDown && isGrounded && !KeyRight) directionNum = 36;
            else directionNum = 6;
        }
        //else if (sprite.flipX)
        else if (transform.localScale.x < 0)
        {
            if (KeyUp && KeyLeft) directionNum = 7;
            else if (KeyDown && KeyLeft) directionNum = 1;
            else if (/*KeyDown && !KeyLeft &&*/!KeyDown && isGrounded) directionNum = 4;
            else if (KeyDown && isGrounded && !KeyLeft) directionNum = 14;
            else directionNum = 4;
        } 
    }

    void CalculateShootAngles() //рассчет углов стрельбы
    {
        if (directionNum == 8) {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[0]);
            position = transform.position;
            position.x = position.x + (transform.localScale.x < 0 ? -0.2f : 0.2f);
            position.y += 0.6f;
        }
        if (directionNum == 9)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[1]);
            position = transform.position;
            position.y += 0.66f; position.x = position.x + 0.4f;
        }
        if (directionNum == 6)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[2]);
            position = transform.position;
            position.y += 0.46f; position.x = position.x + 0.4f;
            //position.y += 0.46f; position.x = position.x + (sprite.flipX ? -0.5f : 0.5f);
        }
        if (directionNum == 3)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootAngles[1]);
            position = transform.position;
            position.y += 0.31f; position.x = position.x + (sprite.flipX ? -0.42f : 0.42f);
        }
        if (directionNum == 2)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootAngles[0]);
            if (currentProjectile == basicProjectile)
            {
                position = transform.position;
            }
            else if (currentProjectile == ProjectileM)
            {
                position = transform.position;
                //position.y += 3f;  position.x = position.x + 3f;
            }
            else if (currentProjectile == ProjectileS)
            {
                position = transform.position;
            }
            else if (currentProjectile == ProjectileL)
            {
                position = transform.position;
            }

            //position.y += 0.46f; 
        }
        if (directionNum == 7)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootAngles[3]);
            position = transform.position;
            position.y += 0.66f; position.x = position.x -0.4f;
        }
        if (directionNum == 4)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[4]);
            position = transform.position;
            position.y += 0.46f; position.x = position.x - 0.4f;
            //position.y += 0.46f; position.x = position.x + (transform.localScale.x < 0 ? -0.5f : 0.5f);
        }
        if (directionNum == 1)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[3]);
            position = transform.position;
            position.y += 0.31f; position.x = position.x + (transform.localScale.x < 0 ? -0.42f : 0.42f);
        }
        if (directionNum == 36)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[2]);
            position = transform.position;
            position.y += 0.32f; position.x = position.x + (transform.localScale.x < 0 ? -0.42f : 0.42f);
        }
        if (directionNum == 14)
        {
            rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, shootAngles[4]);
            position = transform.position;
            position.y += 0.32f; position.x = position.x + (transform.localScale.x < 0 ? -0.42f : 0.42f);
        }

    }

    private void OnTriggerEnter2D(Collider2D collider) //проверка на касание объекта, наносящего урон
    {//закоментировали, т.к. урон дается от монстра (в его скрипте)
        //Unit unit = collider.gameObject.GetComponent<Unit>();
        //if (unit) ReceiveDamage();


        //Если хотим из bullet убрать нанесение урона, прописывать урон у игровых объектов
        //Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        //if (bullet && bullet.Parent != gameObject)
        //{
        //    ReceiveDamage();
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DieCollider")
        {
            transform.position = new Vector3(spawnX, spawnY, transform.position.z); Lives = 5; //коснулся коллайдера и игрока переносит в начало с пополнением здоровья
        }

    }

    public void ChangeWeapon(int type)  //Смена оружия
    {
        //Типы оружия
        // 0 - R
        // 1 - M
        // 2 - F
        // 3 - L

        switch (type)
        {
            case 0: { break; }
            case 1:
                {
                    currentProjectile = ProjectileM;
                    break;
                }
            case 2:
                {
                    currentProjectile = ProjectileF;
                    break;
                }
            case 3:
                {
                    currentProjectile = ProjectileS;
                    break;
                }
            case 4:
                {
                    currentProjectile = ProjectileL;
                    break;
                }
        }

    }

}


public enum CharState
{
    Idle,
    Run,
    Jump,
    Shoot,
    ShootRightDown,
    ShootRightUP,
    Die,
    ShootUP,
    LayRobot,
    ShootLay,
    Idle_Water,
    Run_Water,
    ShootRightUP_Water,
    ShootUp_Water,
    LayRobot_Water,
    ShootRightRun,
    ShootRightRun_water,
    Shoot_water

}

