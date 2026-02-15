using UnityEngine;

public class KeySlot : MonoBehaviour
{
    [SerializeField] private GameObject filledsprite;

    void Start()
    {
        filledsprite.SetActive(false);
    }


    public void FilledIn()
    {
        Debug.Log("keyslot filled in");
        filledsprite.SetActive(true);
    }
}
