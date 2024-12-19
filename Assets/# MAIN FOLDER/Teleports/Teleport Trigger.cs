using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination; // Target location to teleport to
    [SerializeField] private string playerTag = "Player"; // Tag to identify the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            // Teleport the player to the destination
            collision.transform.position = teleportDestination.position;
        }
    }
}
