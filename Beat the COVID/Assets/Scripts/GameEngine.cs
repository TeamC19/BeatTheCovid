﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameEngine : MonoBehaviour
{
    public CameraController cameraController;
    public GameObject cinemachine;
    public GameObject cinemachine2;
    public Animator cameraAnimator;
    public Transform background;
    public bool fightMode = false;
    public float val = 0;
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Esto sirve para que la camara haga un pan (WIP)
        if (background.position.x < -104)
        {
            cameraAnimator.SetFloat("Zoom", val);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            val += speed;
            print("PULSADO ARRIBA val: "+val);

        } else if (Input.GetKeyDown(KeyCode.L))
        {
            val -= speed;
            print("PULSADO ABAJO val: " + val);
        }


        // Modo combate, por ahora se activa con espacio, pero lo suyo es que se active en momentos concretos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!fightMode)
            {
                cinemachine.SetActive(true);
                cinemachine.transform.position = cinemachine2.transform.position + Vector3.up*1.6f+Vector3.right*1f;
               cinemachine2.SetActive(false);
                print("Modo combate activado");
                fightMode = true;
            }
            else
            {
                cinemachine.SetActive(false);
                cinemachine2.SetActive(true);
                fightMode = false;
                print("Modo combate desactivado");
            }
        }
    }
}
