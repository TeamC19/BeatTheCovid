using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Reference to animator for scene transition
    public Animator _transition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;

    
    // Function to restart game immediately
    public void RestartGame()
    {
        // Load the next scene, with menu unpaused
        StartCoroutine(LoadGameLevel(1));
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    // Function to go to main menu
    public void GoMainMenu()
    {
        // Load the next scene, with menu unpaused
        StartCoroutine(LoadGameLevel(0));
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    // Coroutine to play transition animation to go to either play level or boss level
    IEnumerator LoadGameLevel(int levelIndex)
    {
        // Play transition animation
        _transition.SetTrigger("Start");

        // Wait for transition animation to end
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(levelIndex);
    }

}
