using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThis", 0.4f);
    }

    // Update is called once per frame
    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
