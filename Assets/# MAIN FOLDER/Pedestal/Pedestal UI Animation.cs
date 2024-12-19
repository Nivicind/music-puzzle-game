using UnityEngine;
using DG.Tweening;

public class PedestalUISlide : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel; // Reference to the UI panel RectTransform
    [SerializeField] private float slideDuration = 0.5f; // Duration of the slide animation
    [SerializeField] private Vector2 offScreenPosition; // Position off-screen
    [SerializeField] private Vector2 onScreenPosition; // Position on-screen
    [SerializeField] private GameObject parentToDisable; // Parent object to disable after transition

    private bool isAnimating = false; // Animation lock

    void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.anchoredPosition = offScreenPosition; // Ensure the panel starts off-screen
        }
    }

    void OnEnable()
    {
        if (uiPanel != null && !isAnimating)
        {
            // Prevent overlapping animations
            isAnimating = true;
            uiPanel.anchoredPosition = offScreenPosition; // Reset position
            uiPanel.DOAnchorPos(onScreenPosition, slideDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => isAnimating = false); // Release the lock
        }
    }

    public void SlideOutAndDisable()
    {
        if (uiPanel != null && !isAnimating)
        {
            isAnimating = true; // Lock during animation
            uiPanel.anchoredPosition = onScreenPosition; // Reset position
            uiPanel.DOAnchorPos(offScreenPosition, slideDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    isAnimating = false; // Release the lock
                    if (parentToDisable != null)
                    {
                        parentToDisable.SetActive(false); // Disable parent
                    }
                });
        }
    }

    public bool IsAnimating()
    {
        return isAnimating; // Check if an animation is in progress
    }
}
