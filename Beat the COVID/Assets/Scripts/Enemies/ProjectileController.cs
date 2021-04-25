using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Common
    private SpriteRenderer _sprite;
    private Animator _anim;
    private Vector2 direction;
    // _rb2d references the Character's Rigidbody(placed on feet)
    [Header("Rigidbody references")]
    public Rigidbody2D _rb2d;
    public GameObject checkgroundGameObject;
    // This is to know the Player object to figure out it's position
    public GameObject _player;
    // This is to know the projectile's current position
    private Transform _projectile_pos;
    // Projectile's speed

    public float speed = 1f;
    // Attack variables
    
    private float attack_range = 0.5f;
    [SerializeField] int attackDamage = 2;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _projectile_pos = GetComponent<Transform>();

        Invoke("DestroyProjectile", 5.0f);

        if ((_player.transform.position.x - _projectile_pos.position.x) <= 0) { direction.x = -1; _sprite.flipX = true; }// projectile is to the right of the player (or in the same pos X-wise)
        else { direction.x = 1;  }
    }

    // Update is called once per frame
    void Update()
    {

        if ((_player.transform.position.y - _projectile_pos.position.y) <= 0) { direction.y += -0.006f; }
        else { direction.y += 0.006f; }

        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one * direction * Time.deltaTime * speed);
        //damage
        if (Mathf.Abs(_player.transform.position.x - _projectile_pos.position.x) < attack_range
                && Mathf.Abs(_player.transform.position.y - _projectile_pos.position.y) < attack_range) { Attack(); }
        //Change Sprite According to movement
        
    }

    void DestroyProjectile() { Destroy(this.gameObject); }

    void Attack()
    {
        // Damage player
        _player?.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
        DestroyProjectile();
    }
}
