using UnityEngine;

public class LockMouse : MonoBehaviour
{
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
