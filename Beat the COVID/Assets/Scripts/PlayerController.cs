using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    SpriteRenderer _sprite;
    Animator _anim;
    BoxCollider2D colliderLimites;
    Vector2 direction;
    // _rb2d references the Character's Rigidbody(placed on feet)
    public Rigidbody2D _rb2d;
    public GameObject checkgroundGameObject;
    public float speed = 1f;
    // Health variables
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage; 
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
     
    // Jumping variables
    public float jumpForce = 6.5f;
    public float gravity = -9.8f * 10;
    public bool grounded;
    public float startJumpPos;
    public bool jumped;
    
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        checkgroundGameObject = transform.Find("ground check").gameObject;
        colliderLimites = GetComponent<BoxCollider2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        colliderLimites.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        // Take damage trial
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }

        // GoTo Punch() method for all Punch funtionality
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            Punch();
        }

        // GoTo Kick() method for all Kick funtionality
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            Kick();
        }

        // Block animation
        if (Input.GetKey(KeyCode.L)) 
        {
            _anim.SetTrigger("IsBlocking");
        }

        // Throw animation
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            _anim.SetTrigger("IsThrowing");
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
        // Player cannot move when performing other actions
        /*
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_punch")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_block")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_throw")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_damaged")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        */
        // Player cannot move on Y axis while jumping
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_jump")) 
        {  
            direction.y = 0; 
        }
        // Movement
        else if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }
        else if (Input.GetKey(KeyCode.D))
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

    // Method to take damage and deplete health bar
    void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetHealth(currentHealth);
    }

    // Method to punch enemy
    void Punch()
    {
        // Play attack animation
        _anim.SetTrigger("IsPunching");
        // Player cannot move while attacking
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_punch")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        // Detect enemies in range of attack
        // Damage enemies
    }

    // Method to kick enemy
    void Kick()
    {
        // Play attack animation
        _anim.SetTrigger("IsKicking");
        // Player cannot move while attacking
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick ")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        // Detect enemies in range of attack
        // Damage enemies
    }
    
}
