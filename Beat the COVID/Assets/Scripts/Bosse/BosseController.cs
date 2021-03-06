using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BosseController : EnemyController
{
    public bool summoning = false;
    public bool gonnatp = false;
    public GameObject _enemy; //remember to set it in the gameobject
    public GameObject _enemy_spitter;
    public GameObject _enemy_rammer;
    public GameObject _enemy_summoner;
    public GameObject _note_hazard;
    public GameObject _smokepoof;

    public Transform _boss_pos;
    private static int PHASE2_THR = 33;
    private static int PHASE3_THR = 17;
    // Reference to animator for scene transition
    public Animator _transition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;
    public GameObject _heart;//remember to set it in the gameobject
    public GameObject _note;//remember to set it in the gameobject

    private int _hp;
    private int phase;
    // Start is called before the first frame update
    new void Start()
    {
        _audio = GetComponent<AudioSource>();
        hitPoints.currentHealth = hitPoints.startHealth;
        direction = Vector2.zero;
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        //collider = GetComponent<BoxCollider2D>();
        _boss_pos = GetComponent<Transform>();
        phase = 1;
        //InvokeRepeating("SummonEnemy", 7.0f, 7.0f);
        InvokeRepeating("SummonHeart", 5.0f, 5.0f);
        InvokeRepeating("SummonNote", 3.0f, 3.0f);
    }

    // Update is called once per frame
    new void Update()
    {

        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("bosse_walk")) { direction.x = 0; }
        //movement on the X axis
        else if ((_player.transform.position.x - _boss_pos.position.x) <= 0) //boss is to the right of the player (or in the same pos X-wise)
        {
            _sprite.flipX = true;
            if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) > 7) { direction.x = -1; } //farther than 6 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) < 6) { direction.x = 1; }//closer than 4 units, tries to get to 4
            else { direction.x = 0; }
        }
        else //player is to the left of player
        {
            _sprite.flipX = false;
            if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) > 7) { direction.x = 1; } //farther than 6 units, closes in on player
            else if (Mathf.Abs(_player.transform.position.x - _boss_pos.position.x) < 6) { direction.x = -1; }//closer than 5 units, tries to get to 5
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
        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.Play();
        // Play hit animation (NOT DOING ANIMATION ANYMORE)
        if (_boss_pos.position.x > 59.0f || _boss_pos.position.x < 46.0f)
        {
            if (!gonnatp) { Invoke("SafetyTeleport", 1.5f); gonnatp = true; }
        }

        if (hitPoints.currentHealth <= PHASE2_THR && phase == 1)
        {
            phase = 2;
            CancelInvoke();
            SafetyTeleport();
            SummonEnemy();
            InvokeRepeating("SummonHeart", 5.0f, 5.0f);
            InvokeRepeating("SummonNote", 3.0f, 3.0f);
            hitPoints.currentHealth = PHASE2_THR;
        }

        else if (hitPoints.currentHealth <= PHASE3_THR && phase == 2)
        {
            phase = 3;
            CancelInvoke();
            SafetyTeleport();
            SummonEnemy();
            InvokeRepeating("SummonHeart", 5.0f, 5.0f);
            InvokeRepeating("SummonNote", 3.0f, 3.0f);
            hitPoints.currentHealth = PHASE3_THR;
        }
        else
        {
            _anim.SetTrigger("hurt");
        }

    }

    protected override void EnemyDeath()
    {
        // Play death animation
        _anim.SetBool("dead", true);
        CancelInvoke();
        // Inherit from parent EnemyDeath()
        base.EnemyDeath();
        //Stops moving when ded
        direction.x = 0;
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        _rb2d.velocity = (Vector2.one * direction * speed);
        // Play death scene transition ROLL END CREDITS
        Invoke("ChangeScene", 2.0f);
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
        _anim.SetTrigger("call");
        if (phase == 2)
        {
            Instantiate(_enemy_spitter, new Vector2(45.0f, 0.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(45.0f, 0.0f), Quaternion.identity);
            Instantiate(_enemy_rammer, new Vector2(50.0f, -3.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(50.0f, -3.0f), Quaternion.identity);
            Instantiate(_enemy_rammer, new Vector2(55.0f, -3.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(55.0f, -3.0f), Quaternion.identity);
            Instantiate(_enemy_spitter, new Vector2(60.0f, 0.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(60.0f, 0.0f), Quaternion.identity);
        }
        else if (phase == 3)
        {
            Instantiate(_enemy_summoner, new Vector2(45.0f, 0.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(45.0f, 0.0f), Quaternion.identity);
            Instantiate(_enemy, new Vector2(53.0f, -3.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(53.0f, -3.0f), Quaternion.identity);
            Instantiate(_enemy_summoner, new Vector2(60.0f, 0.0f), Quaternion.identity);
            Instantiate(_smokepoof, new Vector2(60.0f, 0.0f), Quaternion.identity);
            InvokeRepeating("NoteFan", 4.0f, 4.0f);
        }
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

    void SummonedHeart() { Instantiate(_heart, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity); }
    void SummonedNote() { Instantiate(_note, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity); }

    void SafetyTeleport()
    {
        Instantiate(_smokepoof, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity);
        gonnatp = false;
        if (_boss_pos.position.x > 52.0f) { _boss_pos.position = new Vector2(49.0f, -0.6f); }
        else { _boss_pos.position = new Vector2(56.0f, -0.6f); }
        Instantiate(_smokepoof, new Vector2(_boss_pos.position.x, _boss_pos.position.y), Quaternion.identity);
    }

    void NoteFan()
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(_note_hazard, new Vector2(67.0f , 1 - i), Quaternion.identity);
            i++;
            Instantiate(_note_hazard, new Vector2(38.0f, 1 - i), Quaternion.identity);
        }
    }

    void ChangeScene()
    {
        StartCoroutine(DeathScreen(SceneManager.GetActiveScene().buildIndex + 1));
    }
}
