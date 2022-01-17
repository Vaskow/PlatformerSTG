using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOverTime : MonoBehaviour {


    public float timeToDie;
	
	// Update is called once per frame
	void Update () {
        if (timeToDie <= 0)
        {
            Destroy(gameObject);
        }
        else timeToDie -= Time.deltaTime;
	}
}
