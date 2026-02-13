using TMPro;
using UnityEngine;

public class UiSettings : MonoBehaviour
{
    public TextMeshProUGUI cointext;
    public GameObject firstkeyslot;
    public KeySlot[] keyslot;
    public GameObject keyslotobject;
    public GameObject keyslotparent;


    void Start()
    {
        AddCoin(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKey()
    {

    }

    public void SetupKeys(int count)
    {
        keyslot = new KeySlot[count];
        float dist = 15;
        for (int i = 0; i < count; i++)
        {
            GameObject slotobject = Instantiate(keyslotobject, Vector2.zero, Quaternion.identity, keyslotparent.transform);
            RectTransform recttransform = slotobject.GetComponent<RectTransform>();
            recttransform.anchoredPosition = new Vector2(dist,0);
            slotobject.transform.localEulerAngles = Vector3.zero;
            dist = dist + 25;
            keyslot[i] = slotobject.GetComponent<KeySlot>();
        }
    }

    public void AddCoin(int count)
    {
        cointext.text = "Coins: " + count;
    }
}
