using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public AttackTrigger Sword;

    PlayerMotor motor;
    Animator anim;

	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("StartAttack");
        }
        anim.SetFloat("Speed", motor.ForwardSpeed);
	}

    public void EnableSword()
    {
        Sword.EnableTrigger();
    }

    public void DisableSword()
    {
        Sword.DisableTrigger();
    }



}
