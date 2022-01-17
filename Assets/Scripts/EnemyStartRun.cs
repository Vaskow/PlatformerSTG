using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartRun : MonoBehaviour {

    // Use this for initialization
    bool StartRun;

    private void Start()
    {
        //StartRun = false;
    }

    private void OnBecameVisible()
    {
        //StartRun = true;
        gameObject.SetActive(true);
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //gameObject.SetActive(StartRun);
    }

}
