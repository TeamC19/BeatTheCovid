using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Common
    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected BoxCollider2D colliderLimits;
    protected Vector2 direction;
    // _rb2d references the Character's Rigidbody(placed on feet)
    [Header("Rigidbody references")]
    protected Rigidbody2D _rb2d;
    protected GameObject checkgroundGameObject;
    // For all enemies
    // This is to know the Player object to figure out it's position
    protected GameObject _player;
    // This is to know the enemy's current position
    protected Transform _enemy_pos;
    // This is to track the player's distance and enemy's searching area
    public float searchRange = 3;
    public float stoppingDistance = 1;
    //Enemy's speed
    public float speed = 3f;
    
    // Health variables
    [Header("Health variables")]
    public HitPoints hitPoints;
    // Enemy States
    [Header("Enemy states")]
    protected bool wait, pursuit;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        colliderLimits = GetComponent<BoxCollider2D>();
        // Enemy health
        hitPoints = Instantiate(hitPoints);
        hitPoints.currentHealth = hitPoints.startHealth;
        // Starting states
        wait = true;
        pursuit = false;
        //
        _player = GameObject.Find("Player");
        _enemy_pos = GetComponent<Transform>();
        // Commented for now because it generates problems (Null Reference)
        //checkgroundGameObject = transform.Find("ground check").gameObject;
    }

    protected virtual void Update()
    {
        // Behavior depending on State
        // Wait state
        if(wait) 
        {
            EnemyWait();
        }
        // Pursuit state
        else if(pursuit) 
        {
            EnemyPursuit();
        }
        // Movement
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }

    // Enemy Wait 
    protected virtual void EnemyWait()
    {
        // Play wait animation in Child
        // If player is detected, change state to Pursuit
        if(Mathf.Abs(_player.transform.position.x -_enemy_pos.position.x) <= searchRange)
        {
            wait = false;
            pursuit = true;
            return;
        }
    }

    protected virtual void EnemyPursuit()
    {
        // Pursuit movement
        // X axis
        // Enemy is to the right of (or same position as) the player
        if ((_player.transform.position.x - _enemy_pos.position.x) <= 0)
        {
            _sprite.flipX = true;
            // If near enough, attack
            if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) <= stoppingDistance
                && Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) <= stoppingDistance) { Attack(); } 
            // Go towards player  
            else 
            { 
                direction.x = -stoppingDistance;
            }
        }

        // Enemy is to the left of player
        else 
        {
            _sprite.flipX = false;
            // If near enough, attack
            if (Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) < stoppingDistance
                && Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) < stoppingDistance) { Attack(); } 
            // Go towards player
            else 
            { 
                direction.x = stoppingDistance;
            }
        }

        // Y axis
        // Enemy is above (or same position as) the player
        if ((_player.transform.position.y - _enemy_pos.position.y) <= 0)
        {
            if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = -stoppingDistance; } 
            else { direction.y = 0; }
        }
        // Enemy is below the player
        else 
        {
            if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) > stoppingDistance) { direction.y = stoppingDistance; } 
            else { direction.y = 0; }
        }

        // If player leaves range, change state to Wait
        if ((_player.transform.position.x - _enemy_pos.position.x) > searchRange * 1.2f)
        {
            pursuit = false;
            wait = true;
            return;
        }
    }

    protected virtual void Attack()
    {
        // Play attack animation in Child
    }

    // Method that will be called by PlayerController - needs to be Public
    public virtual void TakeDamage(int damage)
    {
        // Subtract health
        hitPoints.currentHealth -= damage;

        // Play hit animation in Child
        
        // Call death function
        if(hitPoints.currentHealth <= 0)
        {
            EnemyDeath();
        }
    }

    // Method to kill enemy
    protected virtual void EnemyDeath()
    {
        Debug.Log("Enemy died");
        
        // Play death animation in Child

        // Disable dead enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
