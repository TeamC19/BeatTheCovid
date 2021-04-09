using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : EnemyController
{   
    [SerializeField] float searchRange = 3;
    [SerializeField] float stoppingDistance = 1;
    [SerializeField] float waitTime;
    [SerializeField] float startWaitTime;
    Vector2 direction_patrol;
    GameObject _player;
    Transform _enemy_pos;
    
    //BoxCollider2D collider;

    new void Start() 
    {
        patrol = true;
        pursuit = false;
        waitTime = 20;
        startWaitTime = waitTime;
        direction = Vector2.zero;
        direction_patrol = Vector2.zero;
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        //collider = GetComponent<BoxCollider2D>();
        _enemy_pos = GetComponent<Transform>();
    }

    new void Update()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_punch")
                || _anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_hit")) 
        { 
            direction.x = 0; direction.y = 0; 
        }

        // Movement depending on State
        // Patrol state
        if(patrol) 
        {
            waitTime--;
            // Patrol movement
            if(waitTime == 0) {
                waitTime = startWaitTime;
                direction.x = (int)Random.Range(-2, 2.1f);
                direction.y = (int)Random.Range(-1, 1.2f);
            }
            if(direction_patrol.x < 0) {_sprite.flipX = true;}
             //movimiento
            _anim.SetFloat("speed", Mathf.Abs(direction_patrol.magnitude));
            transform.Translate(Vector2.one *direction_patrol  * Time.deltaTime * speed);
            // If player is detected, change state to Pursuit
            if((_player.transform.position.x -_enemy_pos.position.x) <= searchRange)
            {
                patrol = false;
                pursuit = true;
                return;
            }
        }
        // Pursuit state
        else if(true) 
        {
            // Pursuit movement
            if ((_player.transform.position.x - _enemy_pos.position.x) <= 0) //boss is to the right of the player (or in the same pos X-wise)
            {
                _sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) > stoppingDistance) { direction.x = -1; } //farther than 6 units, closes in on player
                else { _anim.SetTrigger("PlayerNear"); }
            }
            else //player is to the left of player
            {
                _sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) > stoppingDistance) { direction.x = 1; } //farther than 6 units, closes in on player
                else { _anim.SetTrigger("PlayerNear"); }
            }
            
            //movement on the Y axis
            if ((_player.transform.position.y - _enemy_pos.position.y) <= 0) //boss 
            {
                //_sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = -1; } //farther than 2 units, closes in on player
                else { direction.y = 0; }
            }
            else //
            {
                //_sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = 1; } //farther than 2 units, closes in on player
                else { direction.y = 0; }
            }
            
            // If player leaves range, change state to Patrol
            if((_player.transform.position.x -_enemy_pos.position.x) > searchRange * 1.2f)
            {
                pursuit = false;
                patrol = true;
                return;
            }
        }

        //movimiento
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }
}
