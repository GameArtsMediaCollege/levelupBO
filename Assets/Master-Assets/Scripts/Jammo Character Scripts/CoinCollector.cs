using UnityEngine;
using UnityEngine.Audio;

public class CoinCollector : MonoBehaviour
{
    private UiSettings uisettings;
    private bool uiPresent;
    public int coinCount = 0;
    public int keyCount = 0;
    void Start()
    {
        uisettings = FindFirstObjectByType<UiSettings>();
        if(uisettings == null)
        {
            uiPresent = false;
            Debug.Log("UiSettings component is niet aanwezig in de scène");
        }
        else
        {
            uiPresent = true;
        }
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectible")
        {
            coinCount++;
            if(uiPresent)
            {
                uisettings.AddCoin(coinCount);
            }
        }
    }
}
