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

    private float globalFurthestX; // Tracks the furthest local X position drawn
    private bool[] isDrawing; // Tracks drawing state for each note
    private Vector3Int[] currentCellPositions; // Tracks current cell positions for each note
    private float[] nextDrawTimes; // Tracks the next draw time for each note
    private bool isDrawingLocked; // Tracks whether drawing is locked

void Start()
{
    int noteCount = noteKeys.Length;
    isDrawing = new bool[noteCount];
    currentCellPositions = new Vector3Int[noteCount];
    nextDrawTimes = new float[noteCount];

    // Initialize the global furthest X based on the DrawNotesManager's initial local position
    Vector3 initialLocalPosition = transform.localPosition;
    globalFurthestX = transform.InverseTransformPoint(tilemap.transform.position).x + initialLocalPosition.x;

    isDrawingLocked = false; // Drawing starts unlocked
}

    void Update()
    {
        if (pedestalInterface != null && pedestalInterface.activeSelf)
        {
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
                        DrawNoteIncrementally(i);
                    }
                }
            }

            // Reset all notes when the reset key is pressed
            if (Input.GetKeyDown(resetKey))
            {
                ResetNotes();
            }
        }
        else
        {
            // Stop all drawing when pedestal mode is exited
            StopAllDrawing();
        }
    }

void StartDrawing(int noteIndex)
{
    if (isDrawingLocked) return; // Prevent drawing if locked

    isDrawing[noteIndex] = true;

    // Always start at the global furthest X position
    float noteY = noteIndex * noteSpacing; // Vertical offset based on note index
    Vector3 localStartPosition = new Vector3(globalFurthestX, noteY, 0); // Start from global furthest X
    Vector3 worldStartPosition = transform.TransformPoint(localStartPosition);

    // Convert to Tilemap cell position
    currentCellPositions[noteIndex] = tilemap.WorldToCell(worldStartPosition);

    // Set the next draw time
    nextDrawTimes[noteIndex] = Time.time;
}

    void DrawNoteIncrementally(int noteIndex)
    {
        if (Time.time < nextDrawTimes[noteIndex]) return;

        // Get the current world position of the note
        Vector3 worldPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);

        // Check if the current position is within the draw area
        if (drawArea.bounds.Contains(worldPosition))
        {
            // Place the tile at the current cell position
            tilemap.SetTile(currentCellPositions[noteIndex], bridgeTile);

            // Update the furthest global X position
            Vector3 worldCellPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);
            globalFurthestX = Mathf.Max(globalFurthestX, transform.InverseTransformPoint(worldCellPosition).x);

            // Move to the next cell in the X direction
            currentCellPositions[noteIndex] += new Vector3Int(1, 0, 0);

            // Update the next draw time
            nextDrawTimes[noteIndex] = Time.time + (1 / drawSpeed);
        }
        else
        {
            // Stop drawing if out of bounds
            StopDrawing(noteIndex);
            LockDrawing();
        }
    }

    void StopDrawing(int noteIndex)
    {
        isDrawing[noteIndex] = false;

        // Update the furthest X position when stopping
        Vector3 worldPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);
        globalFurthestX = Mathf.Max(globalFurthestX, transform.InverseTransformPoint(worldPosition).x);
    }

    public void StopAllDrawing()
    {
        for (int i = 0; i < isDrawing.Length; i++)
        {
            isDrawing[i] = false; // Stop all drawing processes
        }
    }

    public void UnlockDrawing()
    {
        isDrawingLocked = false; // Allow drawing to resume
    }

    void LockDrawing()
    {
        isDrawingLocked = true;
    }

void ResetNotes()
{
    // Clear all tiles from the tilemap
    tilemap.ClearAllTiles();

    // Reset global furthest X to the DrawNotesManager's initial local position
    Vector3 initialLocalPosition = transform.localPosition;
    globalFurthestX = transform.InverseTransformPoint(tilemap.transform.position).x + initialLocalPosition.x;

    // Reset all drawing states
    for (int i = 0; i < isDrawing.Length; i++)
    {
        isDrawing[i] = false;
        currentCellPositions[i] = Vector3Int.zero;
        nextDrawTimes[i] = 0;
    }

    // Unlock drawing
    isDrawingLocked = false;
}
}
