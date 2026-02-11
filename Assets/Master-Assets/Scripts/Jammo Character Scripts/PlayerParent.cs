using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    //moving platofrm variables
    ControllerColliderHit previousplatform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Alleen als we echt "bovenop" iets staan
        if (hit.moveDirection.y < -0.5f)
        {
            if (hit.transform.tag == "MovingPlatform")
            {
                if (hit != previousplatform)
                {
                    this.transform.SetParent(hit.transform);
                    previousplatform = hit;
                }
            }
            else
            {
                this.transform.SetParent(null);
            }
        }
    }
}
