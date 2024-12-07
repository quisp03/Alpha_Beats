using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Ensure this namespace is included for TextMeshPro support

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX, missSFX;
    public TextMeshPro scoreText;  // Changed from TextMeshProUGUI to TextMeshPro
    public TextMeshPro comboText;  // Changed from TextMeshProUGUI to TextMeshPro
    public int currentScore;  // Stores the current score
    public int comboScore;  // Stores the current combo

    void Start()
    {
        Instance = this;
        currentScore = 0;
        comboScore = 0;
        UpdateScoreDisplay();  // Update the score display at start
        UpdateComboDisplay();  // Update the combo display at start
    }

    public void Hit(int scoreToAdd)
    {
        comboScore += 1;  // Increment the combo count
        currentScore += scoreToAdd;  // Increment the score
        hitSFX.Play();  // Play hit sound effect
        UpdateScoreDisplay();  // Refresh score display
        UpdateComboDisplay();  // Refresh combo display
    }

    public void Miss()
    {
        comboScore = 0;  // Reset combo count
        missSFX.Play();  // Play miss sound effect
        UpdateComboDisplay();  // Refresh combo display to show reset
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + currentScore;  // Display current score
    }

    private void UpdateComboDisplay()
    {
        comboText.text = "Combo: " + comboScore;  // Display current combo
    }
}
