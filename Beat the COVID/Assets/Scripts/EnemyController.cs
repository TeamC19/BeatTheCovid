using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // For all enemies
    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected BoxCollider2D colliderLimits;
    protected Vector2 direction;
    // _rb2d references the Character's Rigidbody(placed on feet)
    protected Rigidbody2D _rb2d;
    protected GameObject checkgroundGameObject;
    // Health variables
    [SerializeField] int maxHealth = 3;
    protected int currentHealth;
    // Enemy States
    protected bool patrol, pursuit;
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] int attackRate = 2; // Attack rate to not be able to spam attacks
    float nextAttackTime = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        colliderLimits = GetComponent<BoxCollider2D>();
        // Enemy health
        currentHealth = maxHealth;
        // Commented for now because it generates problems (Null Reference)
        //checkgroundGameObject = transform.Find("ground check").gameObject;
    }

    // Generic enemy Attack on player
    protected virtual void Attack()
    {
        // Play enemy attack animation in Child
        
        // Detect player in range of attack
        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange,playerLayer);
        // Damage player
        player.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
    }

    // Method that will be called by PlayerController - needs to be Public
    public virtual void TakeDamage(int damage) 
    {
        // Subtract health
        currentHealth -= damage;

        // Play hit animation in Child

        // Call death function
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    // Method to kill enemy
    protected virtual void Death()
    {
        Debug.Log("Enemy died");
        // OPTIONAL FOR NOW
        // Play death animation in Child

        // Destroy dead enemy
        Destroy(this.gameObject);

    }
}
