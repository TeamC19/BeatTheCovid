using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowJump : MonoBehaviour
{
    private Transform playerpos;
    public Rigidbody2D _rb2d;
    private Transform originalpos;
    void Start()
    {
        originalpos = transform;
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerpos = GameEngine.instance.player.transform;
        if (!GameEngine.instance.player.grounded) 
        { 
            transform.position = new Vector3(playerpos.position.x, originalpos.position.y, playerpos.position.z); 
        }
        else 
        { 
            transform.position = playerpos.position + Vector3.down * 1.25f;
            originalpos = transform;
        }
       
        
        
    }
}
