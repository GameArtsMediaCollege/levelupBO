using UnityEngine;
using System.Collections;

public class MusicFadeTrigger : MonoBehaviour 
{
    [Header("Verplichte Onderdelen")]
    public AudioSource audioSource;    // Sleep hier je AudioSource heen
    public AudioClip newTrack;         // Sleep hier het nieuwe liedje heen

    [Header("Instellingen")]
    public float fadeDuration = 2.0f;  // Hoe lang de overgang duurt
    
    private bool hasSwitched = false;  // Zorgt dat het maar 1x gebeurt
    private bool isFading = false;     // Beveiliging tegen freezes

    private void OnTriggerEnter(Collider other) 
    {
        // 1. Check of het de speler is
        // 2. Check of we niet al gewisseld zijn
        // 3. Check of er niet al een fade bezig is
        if (other.CompareTag("Player") && !hasSwitched && !isFading) 
        {
            if (audioSource != null && newTrack != null)
            {
                Debug.Log("Speler gedetecteerd! Start muziek fade naar: " + newTrack.name);
                StartCoroutine(SafeFade(newTrack));
                hasSwitched = true; 
            }
            else
            {
                Debug.LogError("Oeps! Je bent vergeten de AudioSource of AudioClip in de Inspector te slepen.");
            }
        }
    }

    private IEnumerator SafeFade(AudioClip nextClip)
    {
        isFading = true;
        float startVolume = audioSource.volume;

        // Stap A: Volume omlaag (Fade Out)
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            // We gebruiken timer/fadeDuration om van 1 naar 0 te gaan
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null; // Wacht op het volgende frame (DIT VOORKOMT FREEZES)
        }

        // Wissel de clip
        audioSource.volume = 0;
        audioSource.clip = nextClip;
        audioSource.Play();
        Debug.Log("Nieuwe clip gestart.");

        // Stap B: Volume omhoog (Fade In)
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            // We gaan van 0 naar het originele volume
            audioSource.volume = Mathf.Lerp(0, startVolume, timer / fadeDuration);
            yield return null; // Wacht op het volgende frame
        }

        audioSource.volume = startVolume;
        isFading = false;
        Debug.Log("Fade succesvol afgerond.");
    }
}
