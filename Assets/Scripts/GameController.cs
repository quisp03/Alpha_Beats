using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Include this to interact with UI elements
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject retryButton;  // Public reference to the Retry button GameObject

    void Start()
    {
        // Ensure the retry button is hidden at the start
        retryButton.SetActive(false);
    }

    // Method to show the retry button
    public void ShowRetryButton()
    {
        retryButton.SetActive(true);
    }

    // Method to hide the retry button
    public void HideRetryButton()
    {
        retryButton.SetActive(false);
    }

    // Method to retry the game, which must be public and without parameters
    public void RetryGame()
    {
        // Example: Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}