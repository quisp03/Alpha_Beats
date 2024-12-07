using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject helpPanel; // Reference to the Help Panel GameObject

    void Start()
    {
        // Ensure the help panel is not visible at the start
        // This is crucial to prevent it from showing unintentionally when the game loads
        helpPanel.SetActive(false);
    }

    // Method to start the game, loading the next scene
    public void Play()
    {
        // Load the next scene in the build index
        // This assumes that your scenes are set up sequentially in the build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method to quit the game
    public void Quit()
    {
        // Log quit action, useful for debugging especially in the editor
        Debug.Log("The Player has Quit the game");
        
        // Quit the application
        // Note: This will only work in a built game; in the Unity editor, it won't "quit"
        Application.Quit();
    }

    // Method to show the help panel
    public void ShowHelp()
    {
        // Set the help panel to active, making it visible
        helpPanel.SetActive(true);
    }

    // Method to hide the help panel
    public void HideHelp()
    {
        // Set the help panel to inactive, making it invisible
        helpPanel.SetActive(false);
    }
}
