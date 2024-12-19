using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TriggerButtonWithKey : MonoBehaviour
{
    public KeyCode key; // The KeyCode to trigger this button
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            button.onClick.Invoke(); // Trigger the button's click event
            SetButtonState(ButtonState.Pressed); // Simulate "Pressed" state
        }

        if (Input.GetKeyUp(key))
        {
            SetButtonState(ButtonState.Normal); // Return to "Normal" state
        }
    }

    // Method to change the button's state
    private void SetButtonState(ButtonState state)
    {
        var pointer = new PointerEventData(EventSystem.current);

        switch (state)
        {
            case ButtonState.Pressed:
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
                break;

            case ButtonState.Normal:
                ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
                break;
        }
    }

    private enum ButtonState
    {
        Normal,
        Pressed
    }
}
