using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public CameraController cameraController;
    public Animator cameraAnimator;
    public Transform background;
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
        
    }
}
