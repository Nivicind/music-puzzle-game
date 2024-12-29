// EnableDisablePianoController.cs
using UnityEngine;

public class EnableDisablePianoController : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public PianoController pianoController; // Reference to the PianoController script

    private bool isPlayerInRange = false; // Track if the player is in range

    void Start()
    {
        if (pianoController != null)
        {
            pianoController.enabled = false; // Disable the PianoController initially
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (pianoController != null)
            {
                pianoController.enabled = !pianoController.enabled; // Toggle the PianoController
                Debug.Log("Piano Controller is " + (pianoController.enabled ? "enabled" : "disabled")); // Log the state change
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true; // Player is in range
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false; // Player is out of range
            if (pianoController != null)
            {
                pianoController.enabled = false; // Disable the PianoController when out of range
                Debug.Log("Piano Controller disabled"); // Log the state change
            }
        }
    }
}