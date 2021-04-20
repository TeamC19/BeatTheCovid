using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosseNoteController : MonoBehaviour
{
    public float speed = 1f;
    private SpriteRenderer _sprite;
    private Animator _anim;
    public Rigidbody2D _rb2d;
    public GameObject checkgroundGameObject;
    public GameObject _player;
    private Transform _note_pos;
    private Vector2 direction;
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
        _note_pos = GetComponent<Transform>();

        Invoke("destroy", 5.0f);

        if ((_player.transform.position.x - _note_pos.position.x) <= 0) { direction.x = -1; }//heart is to the right of the player (or in the same pos X-wise)
        else { direction.x = 1; }
    }

    // Update is called once per frame
    void Update()
    {

        if ((_player.transform.position.y - _note_pos.position.y) <= 0) { direction.y += -0.006f; }
        else { direction.y += 0.006f; }

        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one * direction * Time.deltaTime * speed);
        //damage
        if (Mathf.Abs(_player.transform.position.x - _note_pos.position.x) < attack_range
                && Mathf.Abs(_player.transform.position.y - _note_pos.position.y) < attack_range) { Attack(); }
    }

    void destroy() { Destroy(this.gameObject); }

    void Attack()
    {
        // Damage player
        _player?.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
        destroy();
    }
}
