using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_runner : EnemyManager {

    public LayerMask Player;
    bool isPlayer = false;
    public Transform Sensor_player;

    // Use this for initialization
    void Start () {
        //target = GameObject.Find("Character"); //получили объект игрока
    }
	
	// Update is called once per frame
	void Update () {

        isPlayer = Physics2D.OverlapCircle(Sensor_player.position, 0.45f, Player); //проверка на землю
        if (isPlayer)
        {
            Die();
        }
       
    }
}
