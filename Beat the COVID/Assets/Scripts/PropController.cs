using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Prop collided!");
    }
}
