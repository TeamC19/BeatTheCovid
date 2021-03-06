using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammerController : EnemyController
{
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] LayerMask playerLayer;
    // Attack variables
    [Header("Attack variables")]
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int attackDamage = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        // Call EnemyController Start() method
        base.Start();
        // Start everything else from this Class
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Call EnemyController Update() method
        base.Update();
    }

    // Enemy Wait
    protected override void EnemyWait()
    {
        // Call EnemyController EnemyWait() method
        base.EnemyWait();
    }

    // Enemy pursuit movement
    protected override void EnemyPursuit()
    
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("spitter_attack")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("spitter_hurt")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        else
        {
            // Call EnemyController EnemyPursuit() method
            base.EnemyPursuit();
            if(_sprite.flipX) _sprite.flipX = false;
            else _sprite.flipX = true;
        }
    }

    // Spitter Attack player
    protected override void Attack()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("rammer_attack")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("rammer_hurt")) 
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
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(_enemy_pos.position, attackRange, playerLayer);
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

        // Destroy dead enemy after playing animation---------(DOES NOT DESTROY)
        Destroy(gameObject, 1f);
    }
}
