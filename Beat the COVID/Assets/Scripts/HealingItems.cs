using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItems : MonoBehaviour
{
    // Different healing items
    [SerializeField] int healBig = 75;
    [SerializeField] int healMedium = 50;
    [SerializeField] int healSmall = 25;

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
