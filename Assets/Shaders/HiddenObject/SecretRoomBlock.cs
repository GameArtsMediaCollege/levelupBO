using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class SecretRoomBlock : MonoBehaviour
{
    [Header("Shader Property")]
    [Tooltip("Name of the shader property controlling dither/visibility.")]
    [SerializeField] private string ditherProperty = "_DitherAmount";

    [Header("Visibility Settings")]
     private float visibleOutside = 1f;  // fully opaque
     private float visibleInside = 0; // almost invisible
    [SerializeField] private float fadeDuration = 0.5f; // seconds

    private MaterialPropertyBlock propBlock;
    private Renderer rend;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        SetDither(visibleOutside); // start outside
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jammo"))
        {
            StartFade(visibleInside);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jammo"))
        {
            StartFade(visibleOutside);
        }
    }

    private void StartFade(float target)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(target));
    }

    private IEnumerator FadeTo(float targetValue)
    {
        rend.GetPropertyBlock(propBlock);
        float startValue = propBlock.GetFloat(ditherProperty);
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            float newValue = Mathf.Lerp(startValue, targetValue, t);

            SetDither(newValue);
            yield return null;
        }

        SetDither(targetValue); // snap to final value
        fadeRoutine = null;
    }

    private void SetDither(float value)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat(ditherProperty, value);
        rend.SetPropertyBlock(propBlock);
    }
}
