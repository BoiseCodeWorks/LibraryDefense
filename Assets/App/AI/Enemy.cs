using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [HideInInspector]
    public Transform SpawnLocation;

    AIMotor motor;

	// Use this for initialization
	void Start () {
        motor = GetComponent<AIMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
