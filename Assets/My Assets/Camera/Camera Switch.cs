using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera playerFollowCamera;
    public CinemachineCamera pedestalCamera;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    [SerializeField] private GameObject playerGameObject;

    private MonoBehaviour[] playerScripts;
    private bool isPedestalView = false;
    private bool isPlayerInRange = false;

    void Start()
    {
        // Find all MonoBehaviour scripts on the player
        if (playerGameObject != null)
        {
            playerScripts = playerGameObject.GetComponents<MonoBehaviour>();
        }
    }

    void Update()
    {
        // Handle toggling when player is in range
        if (isPlayerInRange && Input.GetKeyDown(switchKey))
        {
            if (isPedestalView)
            {
                ExitPedestalMode();
            }
            else
            {
                EnterPedestalMode();
            }
        }

        // Automatically exit pedestal mode if the player leaves the trigger
        if (!isPlayerInRange && isPedestalView)
        {
            ExitPedestalMode();
        }
    }

    void EnterPedestalMode()
    {
        // Switch to Pedestal Camera
        playerFollowCamera.Priority = 5;
        pedestalCamera.Priority = 10;
        EnablePlayerScripts(false);
        isPedestalView = true;
    }

    void ExitPedestalMode()
    {
        // Switch to Player-Follow Camera
        playerFollowCamera.Priority = 10;
        pedestalCamera.Priority = 5;
        EnablePlayerScripts(true);
        isPedestalView = false;
    }

    private void EnablePlayerScripts(bool enable)
    {
        if (playerScripts == null) return;

        foreach (var script in playerScripts)
        {
            if (script != null)
            {
                script.enabled = enable;
            }
        }
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