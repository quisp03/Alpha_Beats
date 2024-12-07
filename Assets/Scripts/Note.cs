using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    public ParticleSystem hitEffect; // Add this line to reference the particle system

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else 
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t);  
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Method to play hit effect
    public void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.Play();
        }
    }
}
