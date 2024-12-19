using UnityEngine;
using DG.Tweening;

public class PianoAudioPlayer : MonoBehaviour
{
    [System.Serializable]
    public class Note
    {
        public KeyCode Key;
        public AudioClip Clip;
    }

    [SerializeField] private Note[] notes; // Array of notes with keys and audio clips
    [SerializeField] private float fadeDuration = 0.5f; // Duration of the fade-out

    private bool[] keyStates; // Tracks which keys are currently pressed
    private Coroutine[] fadeOutCoroutines; // Array to track fade-out coroutines for each note

    void Start()
    {
        keyStates = new bool[notes.Length];
        fadeOutCoroutines = new Coroutine[notes.Length];
    }

    void Update()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            if (Input.GetKeyDown(notes[i].Key))
            {
                PlayNote(i);
            }
            else if (Input.GetKeyUp(notes[i].Key))
            {
                StopNote(i);
            }
        }
    }

    public void PlayNote(int noteIndex)
    {
        if (!keyStates[noteIndex] && noteIndex < notes.Length)
        {
            keyStates[noteIndex] = true;
            AudioSource.PlayClipAtPoint(notes[noteIndex].Clip, transform.position, 1f);

            // Stop any ongoing fade-out to avoid conflicts
            if (fadeOutCoroutines[noteIndex] != null)
            {
                StopCoroutine(fadeOutCoroutines[noteIndex]);
                fadeOutCoroutines[noteIndex] = null;
            }
        }
    }

    public void StopNote(int noteIndex)
    {
        if (noteIndex < notes.Length)
        {
            // Start fading out
            fadeOutCoroutines[noteIndex] = StartCoroutine(FadeOut(notes[noteIndex].Clip, fadeDuration));
            keyStates[noteIndex] = false;
        }
    }

    public void StopAllNotes()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            StopNoteImmediately(i); // Stop all notes and reset states
        }
    }

    private void StopNoteImmediately(int noteIndex)
    {
        if (noteIndex < notes.Length)
        {
            // Stop the note immediately
            keyStates[noteIndex] = false;
        }
    }

    private System.Collections.IEnumerator FadeOut(AudioClip clip, float duration)
    {
        // Since we can't directly fade out an AudioClip, this is a placeholder for any custom fade-out logic
        yield return new WaitForSeconds(duration);
    }
}
