using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //variables para todos los enemigos
    float hp;
    float maxHp;
    // Start is called before the first frame update
    public void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    public void GetDamage(float dmg) 
    {
        hp -= dmg;
        if (hp <= 0)
            hp = 0;
    }
}
