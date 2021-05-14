using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BosseController : EnemyController
{
    public bool summoning = false;
    public GameObject _enemy; //remember to set it in the gameobject
    public Transform _boss_pos;

    // Reference to animator for scene transition
    public Animator _transition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;
    public GameObject _heart;//remember to set it in the gameobject
    public GameObject _note;//remember to set it in the gameobject

    private int _hp;
    // Start is called before the first frame update
    new void Start()
    {
        hitPoints.currentHealth = hitPoints.startHealth;
        direction = Vector2.zero;
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        //collider = GetComponent<BoxCollider2D>();
        _boss_pos = GetComponent<Transform>();

        InvokeRepeating("SummonEnemy", 7.0f, 7.0f);
        InvokeRepeating("SummonHeart", 10.0f, 10.0f);
        InvokeRepeating("SummonNote", 4.0f, 4.0f);
    }

    // Update is called once per frame
    new void Update()
    {
        

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("bosse_summon") || _anim.GetCurrentAnimatorStateInfo(0).IsName("bosse_hurt")) { direction.x = 0; }
        //movement on the X axis
        else if ((_player.transform.position.x - _boss_pos.position.x) <= 0) //boss is to the right of the player (or in the same pos X-wise)
        {
            _sprite.flipX = true;
            if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) > 6) { direction.x = -1; } //farther than 6 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) < 5) { direction.x = 1; }//closer than 4 units, tries to get to 4
            else { direction.x = 0; }
        }
        else //player is to the left of player
        {
            _sprite.flipX = false;
            if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) > 6) { direction.x = 1; } //farther than 6 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) < 5) { direction.x = -1; }//closer than 5 units, tries to get to 5
            else { direction.x = 0; }
        }
        
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("bosse_summon") || _anim.GetCurrentAnimatorStateInfo(0).IsName("bosse_hurt")) { direction.y = 0; }
        //movement on the Y axis
        else if ((_player.transform.position.y - _boss_pos.position.y) <= 0) //boss 
        {
            //_sprite.flipX = true;
            if (Mathf.Abs(_player.transform.position.y - _boss_pos.position.y) > 2) { direction.y = -0.5f; } //farther than 2 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.y - _boss_pos.position.y) < 1) { direction.y = 0.5f; }//closer than 1 units, tries to get to 1
            else { direction.y = 0; }
        }
        else //
        {
            //_sprite.flipX = false;
            if (Mathf.Abs(_player.transform.position.y - _boss_pos.position.y) > 2) { direction.y = 0.5f; } //farther than 2 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.y - _boss_pos.position.y) < 1) { direction.y = -0.5f; }//closer than 1 units, tries to get to 1
            else { direction.y = 0; }
        }
        


        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        _rb2d.velocity = (Vector2.one * direction * speed);
    }

    public override void TakeDamage(int damage)
    {
        // Inherit from parent TakeDamage()
        base.TakeDamage(damage);

        // Play hit animation (NOT DOING ANIMATION ANYMORE)
        _anim.SetTrigger("hurt");
    }

    protected override void EnemyDeath()
    {
        // Play death animation
        _anim.SetBool("dead", true);
        CancelInvoke();
        // Inherit from parent EnemyDeath()
        base.EnemyDeath();
        // Play death scene transition ROLL END CREDITS
        StartCoroutine(DeathScreen(SceneManager.GetActiveScene().buildIndex + 1));
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

    void SummonEnemy()
    {
        _anim.SetTrigger("summon");
        Invoke("Summoned", 0.5f);
    }
    void SummonHeart()
    {
        _anim.SetTrigger("summon");
        Invoke("SummonedHeart", 0.5f);
    }
    void SummonNote()
    {
        _anim.SetTrigger("summon");
        Invoke("SummonedNote", 0.5f);
    }
    void Summoned() {
        if ((_player.transform.position.x - _boss_pos.position.x) <= 0) { Instantiate(_enemy, new Vector2(_boss_pos.position.x - 2.0f, _boss_pos.position.y), Quaternion.identity); }
        else { Instantiate(_enemy, new Vector2(_boss_pos.position.x + 2.0f, _boss_pos.position.y), Quaternion.identity); }
         
    
    }//Summons Enemy with delay, according to animation


    void SummonedHeart() { Instantiate(_heart, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity); }
    void SummonedNote() { Instantiate(_note, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity); }
}
