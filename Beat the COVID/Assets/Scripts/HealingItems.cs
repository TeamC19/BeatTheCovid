using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Big,
    Medium,
    Small
}
public class HealingItems : MonoBehaviour
{
    // Different healing items
    [SerializeField] int healBig = 75;
    [SerializeField] int healMedium = 50;
    [SerializeField] int healSmall = 25;
    [SerializeField] Type type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            int healPoints = 0;
            switch(type)
            {
                case Type.Big:
                    healPoints = healBig;
                    break;
                case Type.Medium:
                    healPoints = healMedium;
                    break;
                case Type.Small:
                    healPoints = healSmall;
                    break;
            }
            player.Heal(healPoints);
            Destroy(gameObject);
        }
    }
}
