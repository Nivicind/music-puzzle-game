// MachineCameraSwitch.cs
using UnityEngine;
using Unity.Cinemachine;

public class MachineCameraSwitch : MonoBehaviour
{
    public CinemachineCamera cameraWhenEnabled; // Camera to use when PianoController is enabled
    public CinemachineCamera cameraWhenDisabled; // Camera to use when PianoController is disabled
    public EnableDisablePianoController pianoControllerScript; // Reference to the EnableDisablePianoController script

    void Start()
    {
        if (cameraWhenEnabled != null)
        {
            cameraWhenEnabled.Priority = 5; // Set initial priority for the enabled camera
        }

        if (cameraWhenDisabled != null)
        {
            cameraWhenDisabled.Priority = 10; // Set initial priority for the disabled camera
        }
    }

    void Update()
    {
        if (pianoControllerScript != null && pianoControllerScript.pianoController != null)
        {
            // Switch camera priorities based on the state of the PianoController
            if (pianoControllerScript.pianoController.enabled)
            {
                if (cameraWhenEnabled != null) cameraWhenEnabled.Priority = 10;
                if (cameraWhenDisabled != null) cameraWhenDisabled.Priority = 5;
            }
            else
            {
                if (cameraWhenEnabled != null) cameraWhenEnabled.Priority = 5;
                if (cameraWhenDisabled != null) cameraWhenDisabled.Priority = 10;
            }
        }
    }
}