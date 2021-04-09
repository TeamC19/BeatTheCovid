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
    Transform _enemy_pos;

    new void Update()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_punch")
                || _anim.GetCurrentAnimatorStateInfo(0).IsName("nomask_hit")) 
        { 
            direction.x = 0; direction.y = 0; 
        }

        // Movement depending on State
        // Patrol state
        if(state ==  States.patrol) 
        {
            // Patrol movement
            direction.x = Random.Range(-1, 1);
            direction.y = Random.Range(-1, 1);
            if(direction.x < 0) {_sprite.flipX = true;}

            // If player is detected, change state to Pursuit
            if((_player.transform.position.x -_enemy_pos.position.x) <= searchRange)
            {
                state = States.pursuit;
                return;
            }
        }
        // Pursuit state
        if(state ==  States.pursuit) 
        {
            // Pursuit movement
            // movement on the X axis, enemy tries to get closer to player
            if((_player.transform.position.x -_enemy_pos.position.x) <= searchRange)
            {
                _sprite.flipX = true;
                direction.x = Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) + 1; 
            }
            else
            {
                _sprite.flipX = false;
                direction.x = Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) - 1; 
            }
            // movement on the Y axis, enemy tries to get closer to player
            if((_player.transform.position.y -_enemy_pos.position.y) <= searchRange)
            {
                direction.y = Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) + 1; 
            }
            else
            {
                direction.y = Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) - 1; 
            }
            // puñetazo
            if (Mathf.Abs(_player.transform.position.y - _enemy_pos.position.y) == 1
                && Mathf.Abs(_player.transform.position.x - _enemy_pos.position.x) == 1) 
            {
                _anim.SetTrigger("PlayerNear");
            }
            // If player leaves range, change state to Patrol
            if((_player.transform.position.x -_enemy_pos.position.x) > searchRange * 1.2f)
            {
                state = States.patrol;
                return;
            }
        }

        //siempre va a poder colisionar porque no salta
        colliderLimites.enabled = true;
        _rb2d.velocity = Vector3.zero;
        direction = Vector2.zero;

        //movimiento
        _anim.SetFloat("speed", Mathf.Abs(direction.magnitude));
        transform.Translate(Vector2.one *direction  * Time.deltaTime * speed);
    }
}
