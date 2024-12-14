using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawNotesManager : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap
    public Tile bridgeTile; // Tile to use for drawing
    public float drawSpeed = 1.0f; // Speed of drawing; higher = faster
    public Collider2D drawArea; // Area where drawing is allowed
    public GameObject pedestalInterface; // Pedestal Interface GameObject
    public KeyCode[] noteKeys; // Array of keys for each note
    public KeyCode resetKey = KeyCode.Backspace; // Key to reset all notes
    public float noteSpacing = 1f; // Vertical distance between notes

    private float globalFurthestX; // Tracks the furthest X position drawn
    private bool[] isDrawing; // Tracks drawing state for each note
    private Vector3Int[] currentCellPositions; // Tracks current cell positions for each note
    private float[] nextDrawTimes; // Tracks the next draw time for each note
    private bool anyDrawingActive; // Tracks whether any note is currently being drawn
    private bool isDrawingLocked; // Tracks whether drawing is locked
    private Vector3 startingPosition; // Starting position of the script's GameObject

    void Start()
    {
        startingPosition = transform.position; // Base starting position
        int noteCount = noteKeys.Length;
        isDrawing = new bool[noteCount];
        currentCellPositions = new Vector3Int[noteCount];
        nextDrawTimes = new float[noteCount];
        globalFurthestX = startingPosition.x; // Initialize furthest X to the starting position
        isDrawingLocked = false; // Drawing starts unlocked
    }

    void Update()
    {
        if (pedestalInterface != null && pedestalInterface.activeSelf)
        {
            anyDrawingActive = false; // Reset the active drawing tracker

            if (!isDrawingLocked)
            {
                for (int i = 0; i < noteKeys.Length; i++)
                {
                    // Start drawing when the note key is pressed
                    if (Input.GetKeyDown(noteKeys[i]))
                    {
                        StartDrawing(i);
                    }

                    // Stop drawing when the note key is released
                    if (Input.GetKeyUp(noteKeys[i]))
                    {
                        StopDrawing(i);
                    }

                    // Draw the note incrementally if active
                    if (isDrawing[i])
                    {
                        anyDrawingActive = true; // Mark as active
                        DrawNoteIncrementally(i);
                    }
                }
            }

            // Reset all notes when the reset key is pressed
            if (Input.GetKeyDown(resetKey))
            {
                ResetNotes();
            }

            // Example usage of anyDrawingActive
            if (anyDrawingActive)
            {
                // Perform some action when any drawing is active
                Debug.Log("A note is currently being drawn.");
            }
        }
    }

    void StartDrawing(int noteIndex)
    {
        if (isDrawingLocked) return; // Prevent drawing if locked

        isDrawing[noteIndex] = true;

        // Initialize the starting cell position for the note
        float noteY = startingPosition.y + noteIndex * noteSpacing; // Adjust for vertical spacing
        Vector3 worldStartPosition = new Vector3(globalFurthestX, noteY, 0);
        currentCellPositions[noteIndex] = tilemap.WorldToCell(worldStartPosition);

        // Set the next draw time
        nextDrawTimes[noteIndex] = Time.time;
    }

    void DrawNoteIncrementally(int noteIndex)
    {
        if (Time.time < nextDrawTimes[noteIndex]) return;

        // Check if the current cell position is within the draw area
        Vector3 worldPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);
        if (drawArea.bounds.Contains(worldPosition))
        {
            // Place the tile and update the furthest X position
            tilemap.SetTile(currentCellPositions[noteIndex], bridgeTile);
            globalFurthestX = Mathf.Max(globalFurthestX, worldPosition.x);

            // Move to the next cell to the right
            currentCellPositions[noteIndex] += new Vector3Int(1, 0, 0);

            // Update the next draw time using the inverted drawSpeed
            nextDrawTimes[noteIndex] = Time.time + (1 / drawSpeed);
        }
        else
        {
            StopDrawing(noteIndex);
            LockDrawing(); // Lock drawing if edge is reached
        }
    }

    void StopDrawing(int noteIndex)
    {
        isDrawing[noteIndex] = false;
    }

    void LockDrawing()
    {
        isDrawingLocked = true;
    }

    void ResetNotes()
    {
        // Clear the tilemap and reset all states
        tilemap.ClearAllTiles();
        globalFurthestX = startingPosition.x; // Reset to the starting X position
        for (int i = 0; i < isDrawing.Length; i++)
        {
            isDrawing[i] = false;
            nextDrawTimes[i] = 0;
        }
        isDrawingLocked = false; // Unlock drawing
    }
}
