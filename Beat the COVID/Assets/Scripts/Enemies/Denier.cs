using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Denier : EnemyController
{

    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    // Attack variables



    public Transform _denier_pos;
    public GameObject _covidRammer;//remember to set it in the gameobject
    public GameObject _covidSpitter;//remember to set it in the gameobject
    public GameObject _covidExploder;//remember to set it in the gameobject

    public bool isSummoning= false;

    protected override void Start()
    {
        // Call EnemyController Start() method
        base.Start();
        // Start everything else from this Class
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _denier_pos = GetComponent<Transform>();
       // InvokeRepeating("SummonRammer", 4.0f, 4.0f);
       // InvokeRepeating("SummonSpitter", 40.0f, 40.0f);
        //InvokeRepeating("SummonExploder", 4.0f, 4.0f);

    }

    protected override void Update()
    {
        // Call EnemyController Update() method
        //base.Update();
        if (wait)
        {
            EnemyWait();
        }
        // Pursuit state
        else if (pursuit)
        {
            //int n = rand.Next(101);
            if (!isSummoning) {
                InvokeRepeating("Summon", 1.0f, 10.0f);
                //InvokeRepeating("SummonRammer", 4.0f, 4.0f);
                isSummoning = true;
            }
            EnemyPursuit();
        }
    }

    // Enemy Wait
    protected override void EnemyWait()
    {
        // Pose in idle
        _anim.SetBool("waiting", true);
        // Call EnemyController EnemyWait() method
        base.EnemyWait();
    }

    // Enemy pursuit movement
    protected override void EnemyPursuit()
    {
        _anim.SetBool("waiting", false);
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_denier_sneeze")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_denier_take_dmg"))
        {
            direction.x = 0;
        }
            //movement on the X axis
            else if ((_player.transform.position.x - _denier_pos.position.x) <= 0) //boss is to the right of the player (or in the same pos X-wise)
            {
                _sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.x - _denier_pos.position.x) > 5) { direction.x = -1;
                 } //farther than 6 units, closes in on player
                else if (Mathf.Abs(_player.transform.position.x - _denier_pos.position.x) < 4) { direction.x = 1; }//closer than 4 units, tries to get to 4
                else { direction.x = 0;}
            }
            else //player is to the left of player
            {
                _sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.x - _denier_pos.position.x) > 5) { direction.x = 1; } //farther than 6 units, closes in on player
                else if (Mathf.Abs(_player.transform.position.x - _denier_pos.position.x) < 4) { direction.x = -1; }//closer than 5 units, tries to get to 5
                else { direction.x = 0;
            }
            }

            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_denier_sneeze")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_denier_take_dmg")) 
            { 
                direction.y = 0;

            }
            //movement on the Y axis
            else if ((_player.transform.position.y - _denier_pos.position.y) <= 0) //boss 
            {
                //_sprite.flipX = true;
                if (Mathf.Abs(_player.transform.position.y - _denier_pos.position.y) > 2) { direction.y = -0.5f; } //farther than 2 units, closes in on player
                else if (Mathf.Abs(_player.transform.position.y - _denier_pos.position.y) < 1) { direction.y = 0.5f; }//closer than 1 units, tries to get to 1
                else { direction.y = 0;}
            }
            else //
            {
                //_sprite.flipX = false;
                if (Mathf.Abs(_player.transform.position.y - _denier_pos.position.y) > 2) { direction.y = 0.5f; } //farther than 2 units, closes in on player
                else if (Mathf.Abs(_player.transform.position.y - _denier_pos.position.y) < 1) { direction.y = -0.5f; }//closer than 1 units, tries to get to 1
                else { direction.y = 0;}
            }


            _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
            transform.Translate(Vector2.one * direction * Time.deltaTime * speed);
        }
    

    // Denier Invoke covid
    protected void Summon()
    {
        System.Random rnd = new System.Random();
        int n = rnd.Next(1,4);
        if (n == 1) { SummonRammer();  }
        if (n == 2) { SummonSpitter(); }
        if (n == 3) { SummonExploder(); }

        // Play enemy attack animation

        //_anim.SetTrigger("playerNear");
        //SummonRammer();

        //InvokeRepeating("SummonRammer", 4.0f, 4.0f);

        // Detect player in range of attack


    }

    // Method that will be called by PlayerController - needs to be Public
    public override void TakeDamage(int damage)
    {
        // Inherit from parent TakeDamage()
        base.TakeDamage(damage);

        // Play hit animation
        _anim.SetTrigger("enemyHurt");
    }

    // Method to kill enemy
    protected override void EnemyDeath()
    {
        // Play death animation
        _anim.SetBool("dead", true);

        // Inherit from parent EnemyDeath()
        base.EnemyDeath();

    }
    void SummonRammer()
    {
        _anim.SetTrigger("summon");        
        Invoke("SummonedRammer", 0.5f);
 
         
      
    }
    void SummonSpitter()
    {
        _anim.SetTrigger("summon");
        Invoke("SummonedSpitter", 0.5f);
    }
    void SummonExploder()
    {
        _anim.SetTrigger("summon");
        Invoke("SummonedExploder", 0.5f);
    }
    void SummonedRammer() { Instantiate(_covidRammer, new Vector2(_denier_pos.position.x - 2.0f, _denier_pos.position.y), Quaternion.identity); }//Summons Enemy with delay, according to animation
    void SummonedSpitter() { Instantiate(_covidSpitter, new Vector2(_denier_pos.position.x - 2.0f, _denier_pos.position.y), Quaternion.identity); }
    void SummonedExploder() { Instantiate(_covidExploder, new Vector2(_denier_pos.position.x - 2.0f, _denier_pos.position.y), Quaternion.identity); }
}

