using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PianoSoundManager : MonoBehaviour
{
    [System.Serializable]
    public class PianoKeyMapping
    {
        public KeyCode keyCode;      // Input key
        public AudioClip audioClip; // Sound sample
    }

    [SerializeField] private List<PianoKeyMapping> pianoKeyMappings; // Assign inputs and sounds in Inspector
    [SerializeField] private float fadeOutDuration = 0.5f; // Time to fade out

    private Dictionary<KeyCode, List<AudioSource>> activeNotes; // Tracks active sounds for each key

    private void Start()
    {
        // Initialize the dictionary
        activeNotes = new Dictionary<KeyCode, List<AudioSource>>();

        foreach (var mapping in pianoKeyMappings)
        {
            activeNotes[mapping.keyCode] = new List<AudioSource>();
        }
    }

    private void Update()
    {
        foreach (var mapping in pianoKeyMappings)
        {
            KeyCode key = mapping.keyCode;

            // Start playing on key press
            if (Input.GetKeyDown(key))
            {
                PlayKey(mapping);
            }

            // Fade out all notes for this key on key release
            if (Input.GetKeyUp(key))
            {
                FadeOutKey(key);
            }
        }
    }

    private void PlayKey(PianoKeyMapping mapping)
    {
        // Create a new AudioSource for this press
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = mapping.audioClip;
        newSource.playOnAwake = false;
        newSource.volume = 1.0f; // Start at full volume
        newSource.loop = false;

        // Play the sound
        newSource.Play();

        // Add it to the list of active notes for this key
        activeNotes[mapping.keyCode].Add(newSource);
    }

    private void FadeOutKey(KeyCode key)
    {
        if (activeNotes.TryGetValue(key, out List<AudioSource> sources))
        {
            foreach (var source in new List<AudioSource>(sources)) // Copy list to safely iterate
            {
                // Fade out and destroy the AudioSource after fade
                source.DOFade(0, fadeOutDuration)
                    .OnComplete(() =>
                    {
                        DOTween.Kill(source); // Ensure no active tweens
                        sources.Remove(source); // Remove from active list
                        Destroy(source);        // Destroy the component
                    });
            }
        }
    }
}
