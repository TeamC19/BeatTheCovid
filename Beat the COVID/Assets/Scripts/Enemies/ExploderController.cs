using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderController : EnemyController
{
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [SerializeField] LayerMask playerLayer;
    // Attack variables
    [Header("Attack variables")]
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int attackDamage = 100;
    private bool soundPlaying = false;
    public AudioClip explodeSound;

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
        Debug.DrawRay(transform.position, new Vector3(attackRange, 0, 0), Color.red);
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("exploder_exploding"))
        {
            // Play enemy attack animation
            //_anim.SetTrigger("playerNear");
            // Detect player in range of attack
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
            // Damage player
            if (!soundPlaying)
            {
                PlaySound(explodeSound);
            }
            foreach (Collider2D player in hitPlayers)
            {
                print(player.gameObject.name);
                player.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
            }

            base.EnemyDeath();
            Destroy(gameObject, 2.317f);
        }
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
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("exploder_attack")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("exploder_hurt"))
        {
            direction.x = 0;
            direction.y = 0;
        }
        else
        {
            // Call EnemyController EnemyPursuit() method
            base.EnemyPursuit();
            if (_sprite.flipX) _sprite.flipX = false;
            else _sprite.flipX = true;
        }
    }

    // Spitter Attack player
    protected override void Attack()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("exploder_attack")
            || _anim.GetCurrentAnimatorStateInfo(0).IsName("exploder_hurt"))
        {
            direction.x = 0;
            direction.y = 0;
        }
        else
        {
            // Call EnemyController Attack() method
           // base.Attack();
            // Play enemy attack animation
            _anim.SetBool("touched", true);
            //EXPLOSION GOES HERE
            //DESTROY BALL AFTER EXPLODING
            
        }


    }
    public void HitByAVaccine()
    {
        Attack();
    }

    public override void TakeDamage(int damage)
    {
        Attack();
    }
}
