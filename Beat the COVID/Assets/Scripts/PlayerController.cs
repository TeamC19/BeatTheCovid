using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public float speed = 1f;
    private SpriteRenderer _sprite;
    private Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX;
        if (Input.GetKey(KeyCode.Space) && transform.position.y == 0) {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
            _anim.SetBool("IsJumping",true);
        }else
        if (Input.GetKey(KeyCode.W) )
        {
                deltaX = 1f;
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }
        else 
        if (Input.GetKey(KeyCode.S))
        {
            deltaX = 1f;

            transform.Translate(Vector3.down * Time.deltaTime * speed);
            _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }else

        if (Input.GetKey(KeyCode.D))
        {
            deltaX = 1f;
            _sprite.flipX = false;
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }else

        if (Input.GetKey(KeyCode.A))
        {
            deltaX = 1f;
            _sprite.flipX = true;
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }else
        {
            deltaX = 0f;
            _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }
       
            
        
       
    }

    
}
