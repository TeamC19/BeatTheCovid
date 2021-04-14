﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    SpriteRenderer _sprite;
    Animator _anim;
    BoxCollider2D colliderLimits;
    Vector2 direction;
    // _rb2d references the Character's Rigidbody(placed on feet)
    Rigidbody2D _rb2d;
    GameObject checkgroundGameObject;
    [SerializeField] float speed = 1f;
    // Health variables
    [SerializeField] HealthBar healthBar;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int punchDamage = 1;
    [SerializeField] int kickDamage = 2;
    [SerializeField] int attackRate = 2; // Attack rate to not be able to spam attacks
    float nextAttackTime = 0f;
     
    // Jumping variables
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float gravity = -9.8f * 10;
    [SerializeField] float startJumpPos;
    public bool grounded; // Grounded and Jumped are public because they are used by CheckGround Script
    public bool jumped;
    
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        checkgroundGameObject = transform.Find("ground check").gameObject;
        colliderLimits = GetComponent<BoxCollider2D>();
        // Player Health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        colliderLimits.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;
        _anim.SetBool("IsJumping", !grounded);
        // Make attacks limited to 2 per second aprox.
        if (Time.time >= nextAttackTime)
        {
            // GoTo Punch() method for all Punch funtionality
            if (Input.GetKeyDown(KeyCode.J)) 
            {
                Punch();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // GoTo Kick() method for all Kick funtionality
            if (Input.GetKeyDown(KeyCode.K)) 
            {
                Kick();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // GoTo Block() method for all Block funtionality
        if (Input.GetKey(KeyCode.L)) 
        {
            Block();
        }

        // GoTo Throw() method for all Throw funtionality
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            Throw();
        }

        // Check if grounded to be able to jump
        if (!grounded) 
        {
            _rb2d.AddForce(Vector2.down * gravity);
            Vector3 position = checkgroundGameObject.transform.position;
            checkgroundGameObject.transform.position = new Vector3(position.x, position.y,transform.position.y - startJumpPos);
        }
        // Jump animation and physics
        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        {
            _anim.SetBool("IsJumping", grounded);
            startJumpPos = transform.position.y;
            _rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumped = true;
            grounded = false;
        }
        
        // Player cannot move on Y axis while jumping
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_jump")) 
        {  
            direction.y = 0; 
        }

        // Movement
        else 
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
            _sprite.flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
            _sprite.flipX = true;
        }

        // Player movement animation
         _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }

    // Method to punch enemy
    void Punch()
    {
        // Play attack animation
        _anim.SetTrigger("IsPunching");
        // Player cannot move while attacking (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_punch")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        // Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(punchDamage);
        }
    }

    // Method to kick enemy
    void Kick()
    {
        // Play attack animation
        _anim.SetTrigger("IsKicking");

        // Player cannot move while attacking (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }

        // Detect enemies in range of attack
         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange,enemyLayer);
        
        // Damage enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(kickDamage);
        }
    }

    void Throw()
    {
        // Play throw animation
        _anim.SetTrigger("IsThrowing");

        // Player cannot move while throwing (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_throw")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
    }

    void Block()
    {
        // Play block animation
        _anim.SetTrigger("IsBlocking");

        // Player cannot move while blocking (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }

        // Not take damage if blocking

    }

    // Method for player taking damage (animations and health depletion)
    public void PlayerTakeDamage(int damage) 
    {
        // Subtract health
        currentHealth -= damage;
        Debug.Log("Player health: " + currentHealth);
        //healthBar.SetHealth(currentHealth);

        // Play hit animation
        _anim.SetTrigger("IsHit");

        // Player cannot move while getting damaged (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_damaged")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }

        // Call death function
        if(currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    // Method to kill enemy
    void PlayerDeath()
    {
        Debug.Log("Player died");
        // OPTIONAL FOR NOW
        // Play death animation 

        // Destroy dead enemy
        Destroy(this.gameObject);

    }

    // Use Gizmos to know where things are to make adjustments
    void onDrawGizmosSelected()
    {   
        // If no Gizmo assigned, exit
        if(attackPoint == null) return;
        // Player attack Gizmo
        Gizmos.color = Color.white;
        Vector3 atkPoint = attackPoint.position;
        Gizmos.DrawWireSphere(atkPoint, attackRange);
    }
    
}
