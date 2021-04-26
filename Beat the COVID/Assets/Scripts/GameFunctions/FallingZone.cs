using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("gund");
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.grounded = false;
        }
    }
}
