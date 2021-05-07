using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemy : MonoBehaviour
{
    public Rigidbody2D _rb2d;
    public float shadowdistance = 1.1f;
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + Vector3.down * shadowdistance;
    }
}
