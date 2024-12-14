using UnityEngine;
using Unity.Cinemachine;

public class CameraZoneSwitcher : MonoBehaviour
{
    private CinemachineCamera activeCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Camera Zone"))
        {
            // Disable the currently active camera
            if (activeCamera != null)
            {
                activeCamera.Priority = 0;
            }

            // Enable the new camera
            CinemachineCamera newCamera = other.GetComponent<CinemachineCamera>();
            if (newCamera != null)
            {
                newCamera.Priority = 10; // Higher priority to activate
                activeCamera = newCamera;
            }
        }
    }
}
