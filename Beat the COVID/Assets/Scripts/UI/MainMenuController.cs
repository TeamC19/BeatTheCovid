using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Reference to animator for scene transition
    public Animator _levelTransition;
    public Animator _bossTransition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;

    
    // Function to load next level when we hit play
    public void StartGame()
    {
        // Load the next scene, with menu unpaused
        StartCoroutine(LoadGameLevel(SceneManager.GetActiveScene().buildIndex + 1));
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    // Coroutine to play transition animation to go to either play level or boss level
    IEnumerator LoadGameLevel(int levelIndex)
    {
        // Play transition animation
        // Determine what transition it should be
        // THIS IS WHAT IT SHOULD BE
        /*
            // If it's an even index or 1, it's a level
        if(levelIndex == 1 || levelIndex % 2 == 0)
        {
            _levelTransition.SetTrigger("Start");
        }
            // If it's an odd index (other than 1) it's a boss
        else 
        {
            _bossTransition.SetTrigger("Start");
        }
        */
        // THIS IS WHAT WE NEED FOR NOW ----- WILL BE ERASED LATER
        if(levelIndex == 1)
        {
            _levelTransition.SetTrigger("Start");
        }
        else if(levelIndex == 2)
        {
            _bossTransition.SetTrigger("Start");
        }

        // Wait for transition animation to end
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(levelIndex);
    }


    // Function to quit program completely
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
