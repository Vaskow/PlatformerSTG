using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classic_EnemyRunner : MonoBehaviour {

    public bool onGround;
    public bool turn_around;
    public Transform groundSensor;
    public Transform SideGroundSensor;

    private bool cliffAhead;
    public Transform cliffSensor;

    public LayerMask ground;

    private Rigidbody2D myBody;
    private Animator myAnimator;

    public float movespeed;
    public float jumpHeight;

    private bool reacted;

    private bool isActive = false; //активность бегуна

    // Use this for initialization
    void Start () {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!isActive) return; //если игрок не в поле зрения, то бегун неактивен 

        onGround = Physics2D.OverlapCircle(groundSensor.position, 0.1f, ground); //проверка на землю
        cliffAhead = !Physics2D.OverlapCircle(cliffSensor.position, 0.1f, ground); //проверка на обрыв
        turn_around = Physics2D.OverlapCircle(SideGroundSensor.position, 0.1f, ground); //проверка на землю сбоку

        //отреагировать на обрыв
        if (onGround && cliffAhead && !reacted)
        {
            ReactToCliff(Random.Range(0, 3));
        }

        if (onGround && !cliffAhead && reacted)
        {
            reacted = false;
        }

        //отреагировать на землю сбоку
        if (onGround && turn_around)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }

        if (onGround && !turn_around)
        {
            turn_around = false;
        }

        //двигаться
        myBody.velocity = new Vector2(movespeed * transform.localScale.x, myBody.velocity.y);
        myAnimator.SetBool("OnGround", onGround); //брал из значения переменной

    }

    //разные реакции на обрыв
    void ReactToCliff(float r)
    {
        if (r == 0)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, jumpHeight);
        }
        if (r == 1)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, jumpHeight / 3);
        }
        if (r > 1)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
        reacted = true;
    }

    private void OnBecameVisible()
    {
        isActive = true;
    }

    //private void OnBecameInvisible()
    //{
    //    isActive = false;
    //}

}

