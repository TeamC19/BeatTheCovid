using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    public bool thrown; // public variable because it's changed by the player
    [SerializeField] Vector3 LaunchOffset;
    [SerializeField] float despawnTime = 5;

    void Start()
    {
        

        if (thrown)
        {
            transform.Translate(Vector3.Scale(LaunchOffset, transform.localScale));
            var direction = transform.localScale.x * transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);

            Destroy(gameObject, despawnTime);
        } else
        {
            transform.eulerAngles = Vector3.forward * Random.Range(0, 360);
        }
        
    }

    private void Update()
    {
        if (thrown)
        {
            transform.position += transform.localScale.x * transform.right * speed * Time.deltaTime;
        }
    }

}
