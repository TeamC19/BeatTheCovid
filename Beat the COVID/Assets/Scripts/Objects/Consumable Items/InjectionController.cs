using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionController : MonoBehaviour
{
    [SerializeField] float speed = 2;
    // public variables because they're changed by the player
    public bool thrown;
   // public bool exploded;
    public float y;
    [SerializeField] Vector3 LaunchOffset;
    [SerializeField] float despawnTime = 5;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange = 2.55f;
    [SerializeField] int damagePoints = 2;
    [SerializeField] Sprite smoke;
    private Animator animator;
    [SerializeField] AudioClip breakingSound;
    [SerializeField] AudioClip explosionSound;
    private AudioSource _audio;
    private bool _soundPlaying = false;


    void Start()
    {
        _audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        if (thrown)
        {
            transform.Translate(Vector3.Scale(LaunchOffset, transform.localScale));
            var direction = transform.localScale.x * transform.right + Vector3.up;
            rb2d.AddForce(direction * speed, ForceMode2D.Impulse);
            animator = gameObject.GetComponent<Animator>();
            Destroy(gameObject, despawnTime);
        } else
        {
            transform.eulerAngles = Vector3.forward * Random.Range(0, 360);
        }

    }

    private void Update()
    {
        // Uncomment to check in the Scene window (editor) the 
        //Debug.DrawRay(transform.position, new Vector3(attackRange, 0, 0), Color.green);
        if (thrown)
        {
            if (transform.position.y <= y)
            {
                //var direction = transform.localScale.x * transform.right + Vector3.up;
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true;

                //spriteRenderer.sprite = smoke;
                transform.localScale = Vector3.one * 2;
                if (!_soundPlaying)
                {
                    _soundPlaying = true;
                    StartCoroutine(PlayExplosionSound());
                }
                animator.SetTrigger("Explode");
                DamageInArea();
                Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
            } else {
                transform.position += transform.localScale.x * transform.right * speed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Player") && !thrown)
        {
            GameEngine.instance.player.GetAnInjection();
            Destroy(gameObject);
        }*/
    }

    public void DamageInArea() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Exploder"))
            {
                enemy.GetComponent<ExploderController>().HitByAVaccine();
            }
            else enemy.gameObject.GetComponent<EnemyController>().TakeDamage(damagePoints);
        }
    }
    
    IEnumerator PlayExplosionSound()
    {
        // We set start of engines sound
        print(breakingSound);
        _audio.clip = breakingSound;
        _audio.Play();
        // We wait until it's over
        yield return new WaitForSeconds(_audio.clip.length);
        print(explosionSound);
        // then, we change the clip to the one that must be looped
        // it corresponds to the sound of the engines continously working
        _audio.clip = explosionSound;
        // The AudioSource plays in loop the sound of engines running
        _audio.Play();
    }
}
