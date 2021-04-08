using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        EnemyController enemy = coll.GetComponent<EnemyController>();
        if (enemy != null) 
        {
            enemy.GetDamage(GameEngine.instance.player.damage);
        }
    }
}
