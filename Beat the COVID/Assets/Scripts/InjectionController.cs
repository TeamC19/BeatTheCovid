using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionController : MonoBehaviour
{
    [SerializeField] float speed = 4;
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
    [SerializeField] Sprite smoke;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        if (thrown)
        {
            transform.Translate(Vector3.Scale(LaunchOffset, transform.localScale));
            var direction = transform.localScale.x * transform.right + Vector3.up;
            rb2d.AddForce(direction * speed, ForceMode2D.Impulse);

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
            transform.position += transform.localScale.x * transform.right * speed * Time.deltaTime;
            if (transform.position.y <= y)
            {
                var direction = transform.localScale.x * transform.right + Vector3.up;
                rb2d.velocity = Vector3.zero;
                rb2d.isKinematic = true;
                thrown = false;
                spriteRenderer.sprite = smoke;
                DamageInArea();
                Destroy(gameObject, 0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }

    public void DamageInArea() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Hacer daño
        }
    }


}
