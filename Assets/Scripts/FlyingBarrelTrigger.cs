using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBarrelTrigger : MonoBehaviour {

    public GameObject barrel;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && barrel)
        {
            barrel.GetComponent<FlyingBarrel>().y = transform.position.y; //заменяем координату бочки y на y триггера
            barrel.GetComponent<FlyingBarrel>().Activate();
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
