using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelHidroalcoholico : MonoBehaviour
{
    public int healthPoints = 4;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
          //  player.Heal(healthPoints);
            Destroy(gameObject);
        }
    }
}
