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


    void Start()
    {
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
        if (thrown)
        {
            if (transform.position.y <= y)
            {
                //var direction = transform.localScale.x * transform.right + Vector3.up;
                rb2d.velocity = Vector2.zero;
                rb2d.isKinematic = true;
                
                //spriteRenderer.sprite = smoke;
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
            enemy.gameObject.GetComponent<EnemyController>().TakeDamage(damagePoints);
        }
    }


}
