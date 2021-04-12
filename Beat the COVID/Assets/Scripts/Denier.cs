using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Denier : EnemyController
{
    enum States {patrol, pursuit}

    States state = States.patrol;
    float searchRange = 1f;
    float stoppingDistance = 0.3f;


    Transform player;
    Vector3 target;

    protected override void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
       
    }

    void SetTarget() {
        if (state != States.patrol) {
            return;
            
        }
    }
}
