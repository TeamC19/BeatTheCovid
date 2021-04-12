using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : EnemyController
{   
    Vector2 direction_patrol;
    // This is to know the Player object to figure out it's position
    GameObject _player;
    // This is to know the enemy's current position
    Transform _enemy_pos;
    // This is to track the player's distance and enemy's searching area
    [SerializeField] float searchRange = 5;
    [SerializeField] float stoppingDistance = 3;
    // This is to wait a certain amount of time to change directions while patroling
    [SerializeField] float waitTime;
    [SerializeField] float startWaitTime;
    //Enemy's speed
    [SerializeField]  float speed = 3f;

    protected override void Start() 
    {   
        // Call EnemyController Start() method
        base.Start();
        // Start everything else from this Class
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _enemy_pos = GetComponent<Transform>();
        // Starting states
        patrol = true;
        pursuit = false;
        // Starting time it waits for changing direction in Patrol
        waitTime = 20;
        startWaitTime = waitTime;
        direction = Vector2.zero;
        direction_patrol = Vector2.zero;
        
    }

    protected void Update()
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
                direction.y = (int)Random.Range(-1, 1.1f);
            }
            if(direction_patrol.x < 0) {_sprite.flipX = true;}
             // Movement
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
        else if(pursuit) 
        {
            // Pursuit movement
            if ((_player.transform.position.x - _enemy_pos.position.x) <= 0) //boss is to the right of the player (or in the same pos X-wise)
            {
                _sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) > stoppingDistance) { direction.x = -stoppingDistance; } //farther than 6 units, closes in on player
                else { _anim.SetTrigger("playerNear"); }
            }
            else //player is to the left of player
            {
                _sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) > stoppingDistance) { direction.x = stoppingDistance; } //farther than 6 units, closes in on player
                else { _anim.SetTrigger("playerNear"); }
            }
            
            //movement on the Y axis
            if ((_player.transform.position.y - _enemy_pos.position.y) <= 0) //boss 
            {
                //_sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = -stoppingDistance; } //farther than 2 units, closes in on player
                else { direction.y = 0; }
            }
            else //
            {
                //_sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = stoppingDistance; } //farther than 2 units, closes in on player
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

    // Method that will be called by PlayerController - needs to be Public
    public override void TakeDamage(int damage) 
    {
        // Inherit from parent TakeDamage()
        base.TakeDamage(damage);

        // Play hit animation
        _anim.SetTrigger("nomaskHurt");
    }

    // Method to kill enemy
    protected override void Death()
    {
        // Play death animation
        // _anim.SetBool("nomaskDead");

        // Inherit from parent Death()
        base.Death();

    }

}
