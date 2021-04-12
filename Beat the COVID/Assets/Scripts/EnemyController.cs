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

    // Method that will be called by PlayerController - needs to be Public
    public virtual void TakeDamage(int damage) 
    {
        // Subtract health
        currentHealth -= damage;

        // Play hit animation

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
        // Play death animation

        //Disable collider from dead enemy
        //GetComponent<Collider2D>().enabled = false; 

        //Disable the enemy
        //this.enabled = false;

        // Destroy dead enemy
        Destroy(this.gameObject);

    }
}
