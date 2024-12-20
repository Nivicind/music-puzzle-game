using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

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
    private bool isResetting; // Tracks whether a reset is in progress

    void Start()
    {
        int noteCount = noteKeys.Length;
        isDrawing = new bool[noteCount];
        currentCellPositions = new Vector3Int[noteCount];
        nextDrawTimes = new float[noteCount];

        StartCoroutine(InitializeGlobalFurthestX()); // Defer initialization of globalFurthestX
        isDrawingLocked = false; // Drawing starts unlocked
        isResetting = false; // Resetting starts false
    }

    private IEnumerator InitializeGlobalFurthestX()
    {
        yield return new WaitForEndOfFrame(); // Wait for Unity to finalize all positions

        // Recalculate globalFurthestX
        Vector3 initialLocalPosition = transform.localPosition;
        globalFurthestX = transform.InverseTransformPoint(tilemap.transform.position).x + initialLocalPosition.x;

        Debug.Log($"Initialized globalFurthestX: {globalFurthestX}"); // Debugging output
    }

    void Update()
    {
        if (pedestalInterface != null && pedestalInterface.activeSelf)
        {
            if (!isDrawingLocked && !isResetting)
            {
                for (int i = 0; i < noteKeys.Length; i++)
                {
                    if (Input.GetKeyDown(noteKeys[i]))
                    {
                        StartDrawing(i);
                    }

                    if (Input.GetKeyUp(noteKeys[i]))
                    {
                        StopDrawing(i);
                    }

                    if (isDrawing[i])
                    {
                        DrawNoteIncrementally(i);
                    }
                }
            }

            if (Input.GetKeyDown(resetKey))
            {
                ResetNotes();
            }
        }
        else
        {
            StopAllDrawing();
        }
    }

    private void UpdateGlobalFurthestX(float newValue)
    {
        globalFurthestX = Mathf.Max(globalFurthestX, newValue);
        Debug.Log($"Updated globalFurthestX to: {globalFurthestX}");
    }

    void StartDrawing(int noteIndex)
    {
        if (isDrawingLocked || isResetting) return; // Prevent drawing if locked or resetting

        isDrawing[noteIndex] = true;

        float noteY = noteIndex * noteSpacing; // Vertical offset based on note index
        Vector3 localStartPosition = new Vector3(globalFurthestX, noteY, 0);
        Vector3 worldStartPosition = transform.TransformPoint(localStartPosition);

        currentCellPositions[noteIndex] = tilemap.WorldToCell(worldStartPosition);
        nextDrawTimes[noteIndex] = Time.time;

        Debug.Log($"StartDrawing: Note {noteIndex} at globalFurthestX: {globalFurthestX}");
    }

    void DrawNoteIncrementally(int noteIndex)
    {
        if (Time.time < nextDrawTimes[noteIndex]) return;

        Vector3 worldPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);

        if (drawArea.bounds.Contains(worldPosition))
        {
            tilemap.SetTile(currentCellPositions[noteIndex], bridgeTile);

            Vector3 worldCellPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);
            float newFurthestX = transform.InverseTransformPoint(worldCellPosition).x;
            UpdateGlobalFurthestX(newFurthestX);

            currentCellPositions[noteIndex] += new Vector3Int(1, 0, 0);
            nextDrawTimes[noteIndex] = Time.time + (1 / drawSpeed);

            Debug.Log($"DrawNote: Note {noteIndex} at {currentCellPositions[noteIndex]}");
        }
        else
        {
            StopDrawing(noteIndex);
            LockDrawing();
        }
    }

    void StopDrawing(int noteIndex)
    {
        isDrawing[noteIndex] = false;

        Vector3 worldPosition = tilemap.CellToWorld(currentCellPositions[noteIndex]);
        float newFurthestX = transform.InverseTransformPoint(worldPosition).x;
        UpdateGlobalFurthestX(newFurthestX);

        Debug.Log($"StopDrawing: Note {noteIndex}");
    }

    public void StopAllDrawing()
    {
        for (int i = 0; i < isDrawing.Length; i++)
        {
            isDrawing[i] = false;
        }
    }

    public void UnlockDrawing()
    {
        isDrawingLocked = false;
    }

    void LockDrawing()
    {
        isDrawingLocked = true;
    }

    void ResetNotes()
    {
        if (isResetting) return; // Prevent multiple resets
        isResetting = true;

        tilemap.ClearAllTiles();

        Vector3 initialLocalPosition = transform.localPosition;
        globalFurthestX = transform.InverseTransformPoint(tilemap.transform.position).x + initialLocalPosition.x;

        for (int i = 0; i < isDrawing.Length; i++)
        {
            isDrawing[i] = false;
            currentCellPositions[i] = Vector3Int.zero;
            nextDrawTimes[i] = 0;
        }

        StartCoroutine(UnlockAfterReset());
    }

    private IEnumerator UnlockAfterReset()
    {
        yield return null; // Wait for the current frame
        isResetting = false;
        isDrawingLocked = false;
        Debug.Log("Reset complete, drawing unlocked.");
    }
}
