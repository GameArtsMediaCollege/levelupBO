using UnityEngine;

public class KeySlot : MonoBehaviour
{
    [SerializeField] private GameObject filledsprite;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FilledIn()
    {
        filledsprite.SetActive(true);
    }
}
