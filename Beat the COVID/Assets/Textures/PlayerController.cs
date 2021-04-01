using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) )
        {
            print("CLICK");
            if (transform.position.y < 1)
            {
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            print("CLICK");
            if (transform.position.y > -3.5)
            {
                transform.Translate(Vector3.down * Time.deltaTime * speed);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            print("CLICK");
           /* if (transform.position.y < 1)
            {*/
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            //}
        }

        if (Input.GetKey(KeyCode.A))
        {
            print("CLICK");
            // if (transform.position.y > -3.5)
            //{
            transform.localScale = Vector3.left;
                transform.Translate(Vector3.left * Time.deltaTime * speed);
           // }
        }
        //if ()
    }
}
