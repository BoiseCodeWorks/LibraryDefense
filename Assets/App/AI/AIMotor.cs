using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMotor : MonoBehaviour
{
    public GameObject EnemyGoal;
    public float BaseSpeed;
    public float RunSpeed;
    public float StartRunningDistance;
    public float StopRunningDistance = 10;

    public float ActualSpeed
    {
        get
        {
            return agent.speed;
        }
    }

    NavMeshAgent agent;


    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = BaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        var remaingDistanceToGoal = Vector3.Distance(transform.position, EnemyGoal.transform.position);

        if (remaingDistanceToGoal < StartRunningDistance)
        {
            agent.speed = RunSpeed;
        }

        if (remaingDistanceToGoal > StartRunningDistance)
        {
            agent.speed = BaseSpeed;
        }



        agent.SetDestination(EnemyGoal.transform.position);




    }
}
