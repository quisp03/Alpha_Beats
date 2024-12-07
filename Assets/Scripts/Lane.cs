using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    private List<Note> notes = new List<Note>();
    private List<double> timeStamps = new List<double>();

    private int spawnIndex = 0;
    public bool AllNotesProcessed { get; private set; } = false;

    public void StartGame()
    {
        AllNotesProcessed = false;
        Debug.Log($"Game officially started in Lane with {timeStamps.Count} timestamps.");
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] midiNotes)
    {
        timeStamps.Clear();
        foreach (var note in midiNotes)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                double timeStamp = metricTimeSpan.TotalMicroseconds / 1000000.0;
                timeStamps.Add(timeStamp);
            }
        }
    }

    void Update()
    {
        if (!AllNotesProcessed && timeStamps.Count > 0)
        {
            HandleNoteSpawning();
            HandleInput();

            // Check if all notes have been processed or are null
            bool allNotesNull = notes.All(note => note == null);
            Debug.Log($"Current Time: {SongManager.GetAudioSourceTime()}, Spawn Index: {spawnIndex}, Notes Count: {notes.Count}, All notes null: {allNotesNull}");

            if (spawnIndex >= timeStamps.Count && allNotesNull)
            {
                AllNotesProcessed = true;
                Debug.Log("All notes processed, ending game.");
                SongManager.Instance.CheckGameEnd();
            }
        }
    }

    private void HandleNoteSpawning()
    {
        while (spawnIndex < timeStamps.Count && SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
        {
            SpawnNote();
        }
    }

    private void SpawnNote()
    {
        var noteObject = Instantiate(notePrefab, transform);
        Note note = noteObject.GetComponent<Note>();
        if (note != null)
        {
            note.assignedTime = (float)timeStamps[spawnIndex];
            notes.Add(note);
        }
        spawnIndex++;
    }

    private void HandleInput()
{
    for (int i = 0; i < notes.Count; i++)
    {
        Note note = notes[i];
        if (note != null)
        {
            double timeStamp = timeStamps[i];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input) && Math.Abs(audioTime - timeStamp) < marginOfError)
            {
                note.PlayHitEffect();  // Play the particle effect
                ScoreManager.Instance.Hit(100); // Assuming each hit gives 100 points
                DestroyNote(i);
            }
            else if (timeStamp + marginOfError <= audioTime)
            {
                ScoreManager.Instance.Miss();
                DestroyNote(i);
            }
        }
    }
}

    private void DestroyNote(int index)
    {
        if (notes[index] != null)
        {
            Destroy(notes[index].gameObject);
            notes[index] = null;
        }
    }
}
