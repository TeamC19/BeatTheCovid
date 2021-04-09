using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMaskController : EnemyController
{
    //Dos estados: estado de patrulla y estado de persecución para ataque
    
    enum States { patrol, pursuit }
    [SerializeField] States state =  States.patrol;
    [SerializeField] float searchRange = 1;
    [SerializeField] float stoppingDistance = 0.3f;
    GameObject _player;
    Transform player;
    Vector3 target;

    void Start()
    {
        player = _player.transform;
        InvokeRepeating("SetTarget", 0, 5);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(target, 0.2f);
    }

    // Función para buscar al jugador
    void SetTarget() 
    {
        // Si no estamos buscando(State.patrol), no nos interesa estar en esta función
        if(state != States.patrol) { return; }
        target =  new Vector2(transform.position.x + Random.Range(-searchRange, searchRange),
            Random.Range(-searchRange, searchRange));
    }
    void Update()
    {
        //siempre va a poder colisionar porque no salta
        colliderLimites.enabled = true;
        _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        // puñetazo
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _anim.SetTrigger("IsPunching");
        }

        //movimiento
        
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }
}
