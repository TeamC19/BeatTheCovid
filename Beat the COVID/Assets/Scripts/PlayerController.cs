using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public float speed = 1f;
    private SpriteRenderer _sprite;
    private Animator _anim;

    // _rb2d hace referencia al rigidbody del personaje(en los pies)
    public Rigidbody2D _rb2d;
    public GameObject checkgroundGameObject;
    public bool grounded;
    public float startJumpPos;
    public bool jumped;
    BoxCollider2D colliderLimites;
    public float damage;
    Vector2 direction;

    // health 
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;


    public float jumpForce = 6.5f;
    public float gravity = -9.8f * 10;
    
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

    void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetHealth(currentHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        colliderLimites.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        //prueba recibir daño 
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }

        // puñetazo
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _anim.SetTrigger("IsPunching");
        }

        // patada
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            _anim.SetTrigger("IsKicking");
        }

        // block
        if (Input.GetKey(KeyCode.L)) 
        {
            _anim.SetTrigger("IsBlocking");
        }

        // lanzar
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            _anim.SetTrigger("IsThrowing");
        }

        //en el aire
        if (!grounded) 
        {
            _rb2d.AddForce(Vector2.down * gravity);
            Vector3 position = checkgroundGameObject.transform.position;
            checkgroundGameObject.transform.position = new Vector3(position.x, position.y,transform.position.y - startJumpPos);
        }
        //saltar
        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        {
            _anim.SetBool("IsJumping", grounded);
            startJumpPos = transform.position.y;
            _rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumped = true;
            grounded = false;
        }
        //no se puede mover si está haciendo otra acción
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_punch")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_block")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_throw")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_damaged")) 
        { 
            direction.x = 0; 
            direction.y = 0; 
        }
        //no se puede mover en el eje Y si está saltando
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_jump")) 
        {  
            direction.y = 0; 
        }
        //movimiento
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
         _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }

    
}
