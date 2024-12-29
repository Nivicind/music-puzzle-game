using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PianoController : MonoBehaviour
{
    [System.Serializable]
    public class KeyNote
    {
        public KeyCode key;          // The key to press
        public string noteName;      // Name of the note
        public Vector3Int startTile; // Base starting position of the note in the Tilemap
        public Tile noteTile;        // Unique tile for this note
    }

    public KeyNote[] keys;         // Array to hold key-note mappings
    public Tilemap noteTilemap;    // Reference to the Tilemap
    public int maxTileHeight = 20; // Maximum height for notes
    public KeyCode resetKey = KeyCode.Backspace; // Key to reset notes
    public float noteSpeed = 1.0f; // Speed at which notes move

    private float globalYLevel = 0.0f;  // Tracks the global y-level for all notes
    private bool notesDrawnThisFrame = false; // Tracks if notes were drawn in the current frame

    void Update()
    {
        notesDrawnThisFrame = false;

        foreach (var keyNote in keys)
        {
            // Check if the key is being held down
            if (Input.GetKey(keyNote.key))
            {
                // Only draw if the global y-level is within bounds
                if (globalYLevel < maxTileHeight)
                {
                    float nextYLevel = globalYLevel + noteSpeed * Time.deltaTime;
                    for (float y = globalYLevel; y < nextYLevel && y < maxTileHeight; y += 1.0f)
                    {
                        Vector3Int position = new Vector3Int(keyNote.startTile.x, Mathf.FloorToInt(y), 0);
                        noteTilemap.SetTile(position, keyNote.noteTile);
                    }

                    // Mark that we drew notes this frame
                    notesDrawnThisFrame = true;
                }
            }
        }

        // Increment the global y-level once per frame if notes were drawn
        if (notesDrawnThisFrame)
        {
            globalYLevel += noteSpeed * Time.deltaTime;
        }

        // Reset all notes when the reset key is pressed
        if (Input.GetKeyDown(resetKey))
        {
            ClearAllNotes();
        }
    }

    void ClearAllNotes()
    {
        // Clear all tiles and reset the global y-level
        noteTilemap.ClearAllTiles();
        globalYLevel = 0.0f;
    }
}
