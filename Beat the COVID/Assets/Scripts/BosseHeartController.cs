using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosseHeartController : MonoBehaviour
{
    public float speed = 1f;
    private SpriteRenderer _sprite;
    private Animator _anim;
    public Rigidbody2D _rb2d;
    public GameObject checkgroundGameObject;
    public GameObject _player;
    private Transform _heart_pos;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        _player = GameObject.Find("Player");
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _heart_pos = GetComponent<Transform>();

        Invoke("destroy",10.0f);
    }

    // Update is called once per frame
    void Update()
    {

        //movement start
        if ((_player.transform.position.x - _heart_pos.position.x) <= 0) { direction.x = -1; }//heart is to the right of the player (or in the same pos X-wise)
        else { direction.x = 1; } //heart is to the left of player

        //movement on the Y axis
        if ((_player.transform.position.y - _heart_pos.position.y) <= 0) { direction.y = -1; }
        else { direction.y = 1; } //

        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one * direction * Time.deltaTime * speed);
        //movement end
    }
    void destroy() { Destroy(this.gameObject); }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")) { destroy(); }
    }*/
}

