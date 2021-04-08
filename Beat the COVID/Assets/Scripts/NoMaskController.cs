using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : MonoBehaviour
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

    }

    // Update is called once per frame
    void Update()
    {
        colliderLimites.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        // puñetazo
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _anim.SetTrigger("IsPunching");
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
