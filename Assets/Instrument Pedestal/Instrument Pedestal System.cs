using UnityEngine;

public class InstrumentPedestalSystem : MonoBehaviour
{
    public Camera playerCamera; // Assign your player camera in the Inspector
    public Vector3 newCameraPosition = new Vector3(0, 5, -10); // Modify this to set the new position
    public Vector3 newCameraRotation = new Vector3(30, 0, 0); // Modify this to set the new rotation
    public float transitionSpeed = 2f; // Speed of camera transition

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isModified = false;
    private bool isTransitioning = false;

    void Start()
    {
        if (playerCamera != null)
        {
            originalPosition = playerCamera.transform.position;
            originalRotation = playerCamera.transform.rotation;
        }
        else
        {
            Debug.LogError("Player Camera is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            isModified = !isModified; // Toggle between perspectives
            StartCoroutine(ChangeCameraPerspective(isModified));
        }
    }

    private System.Collections.IEnumerator ChangeCameraPerspective(bool modify)
    {
        isTransitioning = true;

        Vector3 targetPosition = modify ? newCameraPosition : originalPosition;
        Quaternion targetRotation = modify ? Quaternion.Euler(newCameraRotation) : originalRotation;

        float elapsedTime = 0f;

        while (elapsedTime < transitionSpeed)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetPosition, elapsedTime / transitionSpeed);
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, targetRotation, elapsedTime / transitionSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation are set
        playerCamera.transform.position = targetPosition;
        playerCamera.transform.rotation = targetRotation;

        isTransitioning = false;
    }
}
