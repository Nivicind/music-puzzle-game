using UnityEngine;
using Unity.Cinemachine;

public class PedestalCameraSwitch : MonoBehaviour
{
    public CinemachineCamera playerFollowCamera;
    public CinemachineCamera pedestalCamera;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject pedestalInterface; // UI panel for the interface
    [SerializeField] private GameObject drawNotesGameObject; // DrawNotesManager GameObject

    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private bool isPedestalView = false;
    private bool isPlayerInRange = false;
    PedestalUISlide PedestalUISlide;

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
            pedestalInterface.SetActive(false); // Make sure the interface starts disabled
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
            playerMovement.EnableMovement(false);
        }

        if (pedestalInterface != null)
        {
            pedestalInterface.SetActive(true); // Enable the interface, triggers OnEnable()
        }

        if (drawNotesGameObject != null)
        {
            drawNotesGameObject.SetActive(true);
        }

        isPedestalView = true;
    }

    void ExitPedestalMode()
    {
        playerFollowCamera.Priority = 10;
        pedestalCamera.Priority = 5;

        if (playerMovement != null)
        {
            playerMovement.EnableMovement(true);
        }

        if (pedestalInterface != null)
        {
            PedestalUISlide uiSlide = pedestalInterface.GetComponentInChildren<PedestalUISlide>();
            if (uiSlide != null)
            {
                // Trigger slide-out and disable
                uiSlide.SlideOutAndDisable();
            }
            else
            {
                // Fallback: Disable immediately
                pedestalInterface.SetActive(false);
            }
        }

        if (drawNotesGameObject != null)
        {
            drawNotesGameObject.SetActive(false);
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
