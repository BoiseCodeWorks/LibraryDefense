using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [HideInInspector]
    public Transform SpawnLocation;

    AIMotor motor;
    Animator anim;

	// Use this for initialization
	void Start () {
        motor = GetComponent<AIMotor>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("Speed", motor.ActualSpeed);
	}
}
