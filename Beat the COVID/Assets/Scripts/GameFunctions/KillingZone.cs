using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingZone : MonoBehaviour
{
    float timer = 0;
    PlayerController player;
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("ON");
        if (collision.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer %10 == 0) {
                
                collision.GetComponent<PlayerController>().PlayerTakeDamage(15);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            player.grounded = true;
            timer = 0;
            StartCoroutine(DamagePlayer());
        }
    }

    IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(0.3f);
        player.PlayerTakeDamage(20);
        // then, we change the clip to the one that must be looped
        // it corresponds to the sound of the engines continously working
        StartCoroutine(DamagePlayer());
    }
}
