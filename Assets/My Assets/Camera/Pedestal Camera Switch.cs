using UnityEngine;
using Unity.Cinemachine;

public class PedestalCameraSwitch : MonoBehaviour
{
    public CinemachineCamera playerFollowCamera;
    public CinemachineCamera pedestalCamera;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject pedestalInterface;
    [SerializeField] private GameObject drawNotesGameObject; // Reference to the DrawNotesManager GameObject

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

        // Ensure the Pedestal Interface and DrawNotesManager are disabled initially
        if (pedestalInterface != null)
        {
            pedestalInterface.SetActive(false);
        }

        if (drawNotesGameObject != null)
        {
            drawNotesGameObject.SetActive(false);
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

        if (drawNotesGameObject != null)
        {
            drawNotesGameObject.SetActive(true); // Enable DrawNotesManager for this pedestal
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

        if (drawNotesGameObject != null)
        {
            drawNotesGameObject.SetActive(false); // Disable DrawNotesManager for this pedestal
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
