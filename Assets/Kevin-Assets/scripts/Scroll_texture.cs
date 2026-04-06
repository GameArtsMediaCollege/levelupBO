using UnityEngine;

public class Scroll_texture : MonoBehaviour
{
    public float scrollSpeedX;
    public float scrollSpeedY;
    private MeshRenderer MeshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     MeshRenderer = GetComponent <MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup*scrollSpeedX,Time.realtimeSinceStartup*scrollSpeedY);
    }
}
