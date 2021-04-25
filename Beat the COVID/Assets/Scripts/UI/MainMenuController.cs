using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Function to load next level when we hit play
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level3");
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        // The correct code would be:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // But since we don't have all the levels, we will load only Level 3
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
