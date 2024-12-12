using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera playerFollowCamera;
    public CinemachineCamera pedestalCamera;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject pedestalInterface;

    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private bool isPedestalView = false;
    private bool isPlayerInRange = false;

    void Start()
    {
        // Get the PlayerMovement component from the playerGameObject
        if (playerGameObject != null)
        {
            playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        }

        // Ensure the Pedestal Interface is disabled initially
        if (pedestalInterface != null)
        {
            pedestalInterface.SetActive(false);
        }
    }

    void Update()
    {
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

        if (!isPlayerInRange && isPedestalView)
        {
            ExitPedestalMode();
        }
    }

    void EnterPedestalMode()
    {
        playerFollowCamera.Priority = 5;
        pedestalCamera.Priority = 10;

        if (playerMovement != null)
        {
            playerMovement.EnableMovement(false); // Disable player movement
        }

        if (pedestalInterface != null)
        {
            pedestalInterface.SetActive(true); // Show pedestal interface
        }

        isPedestalView = true;
    }

    void ExitPedestalMode()
    {
        playerFollowCamera.Priority = 10;
        pedestalCamera.Priority = 5;

        if (playerMovement != null)
        {
            playerMovement.EnableMovement(true); // Enable player movement
        }

        if (pedestalInterface != null)
        {
            pedestalInterface.SetActive(false); // Hide pedestal interface
        }

        isPedestalView = false;
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
