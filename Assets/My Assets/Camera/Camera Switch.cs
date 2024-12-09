using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera playerFollowCamera;
    public CinemachineCamera pedestalCamera;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;

    private bool isPedestalView = false;
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(switchKey))
        {
            ToggleCameraView();
        }
    }

    void ToggleCameraView()
    {
        if (isPedestalView)
        {
            // Switch to Player-Follow Camera
            playerFollowCamera.Priority = 10;
            pedestalCamera.Priority = 5;
        }
        else
        {
            // Switch to Pedestal Camera
            playerFollowCamera.Priority = 5;
            pedestalCamera.Priority = 10;
        }

        isPedestalView = !isPedestalView;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
