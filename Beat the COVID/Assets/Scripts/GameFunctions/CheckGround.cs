using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = GameEngine.instance.player;
        //8 referencia a la layer de ground
        if (other.gameObject.layer == 8 && !player.jumped && !player.grounded)
        {
            player.PlayGroundedSound();
            player.grounded = true;
            Physics2D.SetLayerCollisionMask(8, 8);

            player._rb2d.velocity = Vector3.zero;
            player.transform.position = new Vector2(player.transform.position.x, player.startJumpPos);
       


        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = GameEngine.instance.player;
        //8 referencia a la layer de ground
        if (other.gameObject.layer == 8)
        {
            player.grounded = false;
            player.jumped = false;
        }
    }
}
