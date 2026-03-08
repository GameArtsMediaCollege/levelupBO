using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstCamera;   // Reference to the first camera
    public Camera secondCamera;  // Reference to the second camera

    void Start()
    {
        // Disable the second camera initially
        secondCamera.enabled = false;
    }

    void Update()
    {
        // Check for input to switch between cameras
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Switch cameras
            SwitchCameras();
        }
    }

    void SwitchCameras()
    {
        // Enable or disable cameras based on their current state
        firstCamera.enabled = !firstCamera.enabled;
        secondCamera.enabled = !secondCamera.enabled;
    }
}