using UnityEngine;

public class ToggleCameraOnClick : MonoBehaviour
{
    public Camera cameraA; // Assign the first camera (default) in the Inspector
    public Camera cameraB; // Assign the second camera in the Inspector
    public KeyCode toggleKey = KeyCode.E; // Key to toggle cameras
    private bool playerInRange = false; // Track if the player is in range

    void Start()
    {
        if (cameraA == null || cameraB == null)
        {
            Debug.LogError("Assign both cameras in the Inspector!");
            return;
        }

        // Ensure only one camera is active initially
        cameraA.gameObject.SetActive(true);
        cameraB.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(toggleKey))
        {
            ToggleCameras();
        }
    }

    private void ToggleCameras()
    {
        // Switch between the two cameras
        bool isCameraAActive = cameraA.gameObject.activeSelf;

        cameraA.gameObject.SetActive(!isCameraAActive);
        cameraB.gameObject.SetActive(isCameraAActive);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the object has the "Player" tag
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the object has the "Player" tag
        {
            playerInRange = false;
        }
    }
}
