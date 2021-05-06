using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBugPreventer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().grounded = true;
            Vector3 pos = collision.transform.position;
            collision.transform.position = new Vector3(pos.x, 0, pos.z);
        }
    }
}
