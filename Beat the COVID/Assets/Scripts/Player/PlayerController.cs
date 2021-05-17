using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    SpriteRenderer _sprite;
    Animator _anim;
    BoxCollider2D colliderLimits;
    Vector2 direction;
    // Reference to animator for scene transition
    public Animator _transition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;
    // _rb2d references the Character's Rigidbody(placed on feet)
    [Header("Rigidbody references")]
    public Rigidbody2D _rb2d;
    GameObject checkgroundGameObject;
    [SerializeField] float speed = 1f;
    // Health variables

    [Header("Health variables")]
    public HitPoints hitPoints;
    [SerializeField] HealthBar healthBar;
    // Attack variables(I put only one attack point - could be one for kick and one for punch)
    [Header("Attack variables")]
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int punchDamage = 1;
    [SerializeField] int kickDamage = 2;
    [SerializeField] int attackRate = 2; // Attack rate to not be able to spam attacks
    float nextAttackTime = 0f;

    // Jumping variables
    [Header("Jumping variables")]
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float gravity = -9.8f * 10;
    public float startJumpPos; //guarda la posicion de cuando empieza a saltar, valor de inicio 0
    public bool grounded; // Grounded and Jumped are public because they are used by CheckGround Script
    public bool jumped;

    // Inventory variables
    [Header("Inventory variables")]
    [SerializeField] int injectionNumber;
    [SerializeField] GameObject injectionPrefab;
    [SerializeField] Text injectionIndicator;
    [SerializeField] Sprite syringe_active;
    [SerializeField] Sprite syringe_disabled;
    [SerializeField] GameObject[] syringes;

    private AudioSource _audio;
    // Sounds effects
    [Header("Sounds effects")]
    [SerializeField] AudioClip groundHitSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip collectSound;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        checkgroundGameObject = transform.Find("ground check").gameObject;
        colliderLimits = GetComponent<BoxCollider2D>();
        // Player Health
        hitPoints.currentHealth = hitPoints.startHealth;
        healthBar.SetMaxHealth(hitPoints.maxHealth, hitPoints.startHealth);
        UpdateSyringe(injectionNumber);
    }


    // Update is called once per frame
    void Update()
    {
        // Cannot do anything if game is Paused
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        //
        colliderLimits.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;
        _anim.SetBool("IsJumping", !grounded);
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block") || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")) { direction.y = 0; direction.x = 0; }
        // Make attacks limited to 2 per second aprox.
        else if (Time.time >= nextAttackTime)
        {
            // GoTo Punch() method for all Punch funtionality
            if (Input.GetKeyDown(KeyCode.J))
            {
                Punch();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // GoTo Kick() method for all Kick funtionality
            if (Input.GetKeyDown(KeyCode.K))
            {
                Kick();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        // GoTo Block() method for all Block funtionality
        if (Input.GetKey(KeyCode.L))
        {
            Block();
        }

        // GoTo Throw() method for all Throw funtionality
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block") || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")) { direction.y = 0; direction.x = 0; }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Throw();
        }

        // Check if grounded to be able to jump
        if (!grounded)
        {
            _rb2d.AddForce(Vector2.down * gravity);
            Vector3 position = checkgroundGameObject.transform.position;
            checkgroundGameObject.transform.position = new Vector3(position.x, position.y, transform.position.y - startJumpPos);
        }

        // Jump animation and physics
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block") || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")) { direction.y = 0; direction.x = 0; }
        else if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            PlaySound(jumpSound);
            _rb2d.velocity = Vector2.zero;
            _anim.SetBool("IsJumping", grounded);
            startJumpPos = transform.position.y;
            _rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumped = true;
            grounded = false;
            Physics2D.IgnoreLayerCollision(8, 8);

        }



        // Movement
        else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block") || _anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick")) { direction.y = 0; direction.x = 0; }
        else
        {
            if (Input.GetKey(KeyCode.W) && grounded)
            {
                direction.y = 1;
            }
            else if (Input.GetKey(KeyCode.S) && grounded)
            {
                direction.y = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction.x = 1;
                transform.localScale = Vector3.right + Vector3.up + Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                direction.x = -1;
                transform.localScale = Vector3.left + Vector3.up + Vector3.forward;
            }
        }
        // Player movement animation
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one * direction * Time.deltaTime * speed);
    }

    // Function to punch enemy
    void Punch()
    {
        // Play attack animation
        _anim.SetTrigger("IsPunching");
        //PlaySound(hitSound);
        // Player cannot move while attacking (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_punch"))
        {
            direction.x = 0;
            direction.y = 0;
        }

        // Detect enemies in range of attack ------------------------------------(THIS MUST CHANGE FROM LAYER TO TAG)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(punchDamage);
        }
    }

    // Function to kick enemy
    void Kick()
    {
        // Play attack animation
        _anim.SetTrigger("IsKicking");
       // PlaySound(hitSound);
        // Player cannot move while attacking (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_kick"))
        {
            direction.x = 0;
            direction.y = 0;
        }

        // Detect enemies in range of attack ------------------------------------(THIS MUST CHANGE FROM LAYER TO TAG)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(kickDamage);
        }
    }

    // Function to throw injection
    void Throw()
    {
        // Play throw animation
        _anim.SetTrigger("IsThrowing");

        // Player cannot move while throwing (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_throw"))
        {
            direction.x = 0;
            direction.y = 0;
        }

        if (injectionNumber > 0)
        {
            UpdateSyringe(--injectionNumber);
            GameObject vaccine = Instantiate(injectionPrefab, attackPoint.position, transform.rotation);
            InjectionController injectionController = vaccine.GetComponent<InjectionController>();
            injectionController.thrown = true;
            injectionController.y = transform.position.y - 0.91f;
            vaccine.GetComponent<Rigidbody2D>().gravityScale = 1;
            vaccine.transform.localScale = transform.localScale;
        }

    }

    // Function to block attacks
    void Block()
    {
        // Play block animation
        _anim.SetTrigger("IsBlocking");


    }

    // Method for player taking damage (animations and health depletion)
    public void PlayerTakeDamage(int damage)
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_block"))
        {
            // No take damage
        }
        else
        {
            PlaySound(hitSound);
            // Subtract health
            hitPoints.currentHealth -= damage;
            Debug.Log("Player health: " + hitPoints.currentHealth);
            healthBar.SetHealth(hitPoints.currentHealth);

            // Play hit animation
            _anim.SetTrigger("IsHit");
        }
        // Player cannot move while getting damaged (NOT WORKING AS INTENDED)-----------
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("player_damaged"))
        {
            direction.x = 0;
            direction.y = 0;
        }

        // Call death function
        if (hitPoints.currentHealth <= 0)
        {
            hitPoints.currentHealth = 0;
            PlayerDeath();
        }
    }

    // Function to kill player
    void PlayerDeath()
    {
        Debug.Log("Player died");
        // OPTIONAL FOR NOW
        // Play death animation 

        // Disable dead player
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Play death scene transition GO BACK TO MAIN MENU FOR NOW
        StartCoroutine(DeathScreen(5));
        // Reload scene to respawn player
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    // Coroutine to play transition animation to go to main menu after player death
    IEnumerator DeathScreen(int levelIndex)
    {
        // Play transition animation
        _transition.SetTrigger("Start");

        // Wait for transition animation to end
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(levelIndex);
    }

    // Function to respawn character when killed (UNUSED FOR NOW)
    void RespawnCharacter()
    {
        hitPoints.currentHealth = hitPoints.startHealth;
    }

    // When player runs into healing item or injection, despawn item
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CanBePickedUp"))
        {
            // What type of item is this
            Item item = other.gameObject.GetComponent<Consumable>().item;
            if (item != null)
            {
                // Show what type of item this is
                Debug.Log("Item: " + item.objectName);

                // Need to know if this type of item should disappear
                bool shouldDisappear = false;

                switch (item.itemType)
                {
                    case Item.ItemType.BIG_HEAL:
                        if (hitPoints.currentHealth < hitPoints.maxHealth)
                        {
                            Debug.Log("You healed with BIG HEAL!");
                            shouldDisappear = true;
                            AdjustHitPoints(item.quantity);
                        }
                        else { Debug.Log("Greedy. No heal for u"); }
                        break;
                    case Item.ItemType.MEDIUM_HEAL:
                        if (hitPoints.currentHealth < hitPoints.maxHealth)
                        {
                            Debug.Log("You healed with MEDIUM HEAL!");
                            shouldDisappear = true;
                            AdjustHitPoints(item.quantity);
                        }
                        else { Debug.Log("Greedy. No heal for u"); }
                        break;
                    case Item.ItemType.SMALL_HEAL:
                        if (hitPoints.currentHealth < hitPoints.maxHealth)
                        {
                            Debug.Log("You healed with SMALL HEAL!");
                            shouldDisappear = true;
                            AdjustHitPoints(item.quantity);
                        }
                        else { Debug.Log("Greedy. No heal for u"); }
                        break;
                    case Item.ItemType.INJECTION:
                        if (!other.gameObject.GetComponent<InjectionController>().thrown)
                        {
                            if (injectionNumber < 3)
                            {
                                UpdateSyringe(++injectionNumber);
                                shouldDisappear = true;
                            }
                            else print("Greedy! Only 3 vaccines");

                        }
                        break;
                }

                // If item should be consumed: Deactivate item from scene (item consumed)
                if (shouldDisappear) { other.gameObject.SetActive(false); PlaySound(collectSound); }
            }

        }

        // If collision is with End-of-Level wall
        // Transition into next scene
    }

    // Function to heal player
    public void AdjustHitPoints(int amount)
    {
        // Make sure player cannot over heal in amount
        if ((hitPoints.currentHealth + amount) > hitPoints.maxHealth)
        {
            hitPoints.currentHealth = hitPoints.maxHealth;
        }
        else
        {
            hitPoints.currentHealth += amount;
        }
        healthBar.SetHealth(hitPoints.currentHealth);
        Debug.Log("Health: " + hitPoints.currentHealth);
    }

    // Use Gizmos to know where things are to make adjustments
    void onDrawGizmosSelected()
    {
        // If no Gizmo assigned, exit
        if (attackPoint == null) return;
        // Player attack Gizmo
        Gizmos.color = Color.white;
        Vector3 atkPoint = attackPoint.position;
        Gizmos.DrawWireSphere(atkPoint, attackRange);
    }

    void UpdateSyringe(int val)
    {

        for (int i = 0; i < syringes.Length; i++)
        {
            Image syringeImage = syringes[i].GetComponent<Image>();
            if (i < val)
            {
                syringeImage.sprite = syringe_active;
                syringeImage.color = new Color(1, 1, 1, 1);

            }
            else
            {
                syringeImage.sprite = syringe_disabled;
                syringeImage.color = new Color(0.753f, 0.753f, 0.753f, 0.753f);
            }
        }
    }

    public void PlayGroundedSound()
    {
        PlaySound(groundHitSound);
    }

    public void PlaySound(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.Play();
    }
}
