using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking; // Required for UnityWebRequest
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Core;
using System.Linq;
using UnityEngine.SceneManagement;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;
    public int inputDelayInMilliseconds;

    public string fileLocation; // Ensure this is just the file name located in StreamingAssets
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
        StartCoroutine(ReadFromFile());
    }

private IEnumerator ReadFromFile()
{
    string filePath = Path.Combine(Application.streamingAssetsPath, fileLocation);
    UnityWebRequest www = UnityWebRequest.Get(filePath);
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
    {
        Debug.LogError("Error loading MIDI file: " + www.error);
    }
    else
    {
        // Create a new MemoryStream over the received data
        MemoryStream midiStream = new MemoryStream(www.downloadHandler.data);

        try
        {
            midiFile = MidiFile.Read(midiStream);
            GetDataFromMidi();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to read MIDI file: " + e.Message);
        }
    }
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
