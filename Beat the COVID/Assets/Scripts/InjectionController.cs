using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    public bool thrown; // public variable because it's changed by the player
    [SerializeField] Vector3 LaunchOffset;
    void Start()
    {
        

        if (thrown)
        {
            transform.Translate(LaunchOffset);
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);

            Destroy(gameObject, 5);
        } else
        {
            transform.eulerAngles = Vector3.forward * Random.Range(0, 360);
        }
        
    }

    private void Update()
    {
        if (thrown)
        {
            print("Thr");
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

}
