using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : EnemyController
{
    

    // Update is called once per frame
    void Update()
    {
        colliderLimites.enabled = grounded;
        if (grounded)
            _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        // puñetazo
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _anim.SetTrigger("IsPunching");
        }

        //movimiento
        
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }
}
