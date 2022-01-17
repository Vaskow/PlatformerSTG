using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    //Новый скрипт для камеры с учетом размеров экрана и ограничениями
    //public BoxCollider2D cameraBounds; //границы камеры

    //public bool isFollowing; // разрешение движения за игроком

    //public GameObject LeftBorder;

    //private Transform player;

    //private Vector2 min;
    //private Vector2 max;

    //private void Start()
    //{
    //    player = FindObjectOfType<Character>().transform;
    //}

    //private void Update()
    //{
    //    min = cameraBounds.bounds.min;
    //    max = cameraBounds.bounds.max;

    //    var x = transform.position.x;

    //    if (isFollowing)
    //    {
    //        if (player.position.x > x)
    //        {
    //            x = player.position.x;
    //        }
    //    }

    //    var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);  //половина горизонтального размера камеры
    //    x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth); //ограничили x
    //    transform.position = new Vector3(x, transform.position.y, transform.position.z);
    //    LeftBorder.transform.position = new Vector2(x - cameraHalfWidth, transform.position.y);

    //}






    //Старый скрипт камеры
    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    private Character character;
    [SerializeField]
    private Transform target; //за кем летает камера
    public bool isFollowing; // разрешение движения за игроком

    //public float smoothTime = 0.2f;
    //private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        if (!target) target = character.transform;
        //if (!target) target = FindObjectOfType<Character>().transform; //делаем автоматически, чтобы камера переносилась на игрока
    }

    private void LateUpdate() //для движения камеры (испольняется после работы update со всеми кадрами)
    {
        //для движения только по x
        //var x = transform.position.x;

        //if (isFollowing)
        //{
        //    if (target.position.x > x)
        //    {
        //        x = target.position.x;
        //    }
        //}
        //transform.position = new Vector3(x, transform.position.y, transform.position.z);


        Vector3 position = target.position; position.z = -10.0f; //чтобы камера сохранила координаты по z и не приближалась на 0 к плоскости
        position.y = -20.38f; //фиксированная высота

        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        //lerp плавго передвигает камеру (окуда, куда, скорость) 

        //transform.position = Vector3.SmoothDamp(transform.position, position, ref _velocity, smoothTime);
        //ещё один метод для плавного перемещения камеры
    }

}
