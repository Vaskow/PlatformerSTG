using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChecked : MonoBehaviour {

    private Character player;
    private float angle;
    private Vector2 A, B, C;
    
    
    // Use this for initialization
	void Start () {
        player = FindObjectOfType<Character>();
	}

    //пушка поворачивается в 12 разных направлениях (30 град на поворот)
    public float checkAngle() //измеряем угол м/у пушкой и игроком
    {
        A = new Vector2(transform.position.x, transform.position.y); //положение пушки
        B = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f); //игрока + небольшая поправка роста игрока
        C = B - A;  //находим длину катетов

        angle = Mathf.Atan2(C.y, C.x) * Mathf.Rad2Deg; //угол получаем в градусах с помощью тангенса в прямоугольной треугольнике
        angle = Mathf.Round(angle / 30) * 30; //округляем деленное значение и переводим в градусы

        return angle;
    }
}
