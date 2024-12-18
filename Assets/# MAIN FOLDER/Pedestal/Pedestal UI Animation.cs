using UnityEngine;
using DG.Tweening;

public class PedestalUISlide : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel; // Reference to the UI panel RectTransform
    [SerializeField] private float slideDuration = 0.5f; // Duration of the slide animation
    [SerializeField] private Vector2 offScreenPosition; // Position off-screen
    [SerializeField] private Vector2 onScreenPosition; // Position on-screen
    [SerializeField] private GameObject parentToDisable; // Reference to the parent object to disable after transition

    void Start()
    {
        // Ensure the panel starts off-screen
        if (uiPanel != null)
        {
            uiPanel.anchoredPosition = offScreenPosition;
        }
    }

    void OnEnable()
    {
        // Start slide-in animation when the panel is enabled
        if (uiPanel != null)
        {
            uiPanel.anchoredPosition = offScreenPosition; // Reset position to off-screen
            uiPanel.DOAnchorPos(onScreenPosition, slideDuration).SetEase(Ease.OutQuad);
        }
    }

    public void SlideOutAndDisable()
    {
        if (uiPanel != null)
        {
            // Perform the slide-out animation
            uiPanel.DOAnchorPos(offScreenPosition, slideDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    // Disable the parent after the animation completes
                    if (parentToDisable != null)
                    {
                        parentToDisable.SetActive(false);
                    }
                });
        }
    }
}
