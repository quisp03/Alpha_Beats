using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Core;
using System.Linq;
//
public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;
    public int inputDelayInMilliseconds;

    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY => noteTapY - (noteSpawnY - noteTapY);
    public static MidiFile midiFile;
    public GameObject retryButton, returnToMenuButton;

    void Start()
    {
        Instance = this;
        retryButton.SetActive(false);
        returnToMenuButton.SetActive(false);
        StartCoroutine(DelayedStart(songDelayInSeconds));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReadFromFile();
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes().ToArray();
        foreach (var lane in lanes)
        {
            lane.SetTimeStamps(notes);
        }
        StartSong();
    }

    public void StartSong()
    {
        audioSource.Play();
        foreach (var lane in lanes)
        {
            lane.StartGame();
        }
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public void CheckGameEnd()
    {
        if (lanes.All(lane => lane.AllNotesProcessed))
        {
            StartCoroutine(DelayStopMusicAndShowOptions(0.5f)); // Delay to allow the last note sound to finish
        }
    }

    private IEnumerator DelayStopMusicAndShowOptions(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        ShowGameOverOptions();
    }

    private void ShowGameOverOptions()
    {
        retryButton.SetActive(true);
        returnToMenuButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
