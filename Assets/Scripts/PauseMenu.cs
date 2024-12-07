using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;  // Tracks the pause state
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel
    public AudioSource[] allAudioSources; // Array to hold all audio sources in the scene

    void Start()
    {
        // Find all audio sources you want to control during pause
        allAudioSources = FindObjectsOfType<AudioSource>();
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ToggleAudio(true);  // Resume all audio sources
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ToggleAudio(false);  // Pause all audio sources
    }

    private void ToggleAudio(bool play)
    {
        foreach (var audio in allAudioSources)
        {
            if (play)
                audio.UnPause();
            else
                audio.Pause();
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Ensure time is reset
        SceneManager.LoadScene("MainMenu");  // Load main menu scene
    }
}
