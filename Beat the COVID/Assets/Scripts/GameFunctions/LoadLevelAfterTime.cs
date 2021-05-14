using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelAfterTime : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 10f;
    private float timeElapsed;

    private void Update() 
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >  delayBeforeLoading)
        {
            SceneManager.LoadScene(0);
        }
    }
}
