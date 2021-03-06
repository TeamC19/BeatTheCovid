using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : EnemyController
{
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    // Attack variables
    [Header("Attack variables")]
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int attackDamage = 1;

    protected override void Start()
    {
        // Call EnemyController Start() method
        base.Start();
        // Start everything else from this Class
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        // Call EnemyController Update() method
        base.Update();
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
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_punch")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_hit")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        else
        {
            // Call EnemyController EnemyPursuit() method
            base.EnemyPursuit();
        }
    }

    // NoMask Attack player
    protected override void Attack()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_punch")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_hit")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        else 
        {
            // Call EnemyController Attack() method
            base.Attack();
            // Play enemy attack animation
            _anim.SetTrigger("playerNear");
            // Detect player in range of attack
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            // Damage player
            _player?.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
        }
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

}
