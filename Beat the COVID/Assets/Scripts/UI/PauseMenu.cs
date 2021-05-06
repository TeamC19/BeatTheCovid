using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Reference to animator for scene transition
    public Animator _transition;
    // Time it takes to wait for animation to end
    public float transitionTime = 1f;
    // Pause menu references
    public static bool GameIsPaused = false;
    protected GameObject pauseMenuUI;
    private void Awake()
    {
        pauseMenuUI = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void ReloadLevel()
    {
        Debug.Log("Reloading game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    // This function will take you to the MainMenu Scene
    public void LoadMenu()
    {
        Debug.Log("Loading menu");
        // Load the next scene, with menu unpaused
        StartCoroutine(LoadGameMenu(0));
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    // Coroutine to play transition animation to go to main menu
    IEnumerator LoadGameMenu(int levelIndex)
    {
        // Play transition animation
        _transition.SetTrigger("Start");

        // Wait for transition animation to end
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(levelIndex);
    }
}
