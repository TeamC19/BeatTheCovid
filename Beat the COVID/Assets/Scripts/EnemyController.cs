using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected float speed = 1f;
    protected SpriteRenderer _sprite;
    protected Animator _anim;

    // _rb2d hace referencia al rigidbody del personaje(en los pies)
    protected Rigidbody2D _rb2d;
    protected GameObject checkgroundGameObject;
    protected BoxCollider2D colliderLimites;
    protected float damage;
    protected Vector2 direction;

    //variables para todos los enemigos
    protected float hp;
    protected float maxHp = 3;
    
    // Start is called before the first frame update
    protected void Start()
    {
        hp = maxHp;
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        checkgroundGameObject = transform.Find("ground check").gameObject;
        colliderLimites = GetComponent<BoxCollider2D>();

    }

    protected void Update() 
    {

    }
    public void GetDamage(float dmg) 
    {
        hp -= dmg;
        if (hp <= 0)
            hp = 0;
    }
}
