using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    [System.Serializable]
    public struct KeyTriggerMapping
    {
        public KeyCode key;
        public string animationTrigger;
    }

    [Header("Key Mappings")]
    [Tooltip("Leave empty to auto-populate from Animator triggers (keys 1–9, then wraps).")]
    public KeyTriggerMapping[] keyMappings;

    [Header("UI")]
    public TextMeshProUGUI keyDisplayText;

    [Header("Settings")]
    public float displayDuration = 1f;
    public string displayFormat = "YOU JUST PRESSED {0}";

    private Animator animator;
    private Coroutine displayCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("AnimationController: No Animator component found on this GameObject!", this);
            enabled = false;
            return;
        }

        if (keyMappings == null || keyMappings.Length == 0)
        {
            PopulateTriggerMappings();
        }
    }

    void Update()
    {
        foreach (var mapping in keyMappings)
        {
            if (Input.GetKeyDown(mapping.key))
            {
                //Debug.Log($"Key pressed: {mapping.key} → Trigger: {mapping.animationTrigger}");
                animator.SetTrigger(mapping.animationTrigger);
                DisplayPressedKey(mapping.key.ToString().Replace("Alpha", ""));
            }
        }
    }

    private void PopulateTriggerMappings()
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        List<KeyTriggerMapping> mappings = new List<KeyTriggerMapping>();
        int keyIndex = 1;

        foreach (var param in parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                KeyCode keyCode = KeyCode.Alpha0 + keyIndex; // Maps 1–9 to KeyCode.Alpha1–Alpha9

                mappings.Add(new KeyTriggerMapping
                {
                    key = keyCode,
                    animationTrigger = param.name
                });

                keyIndex++;
                if (keyIndex > 9)
                {
                    keyIndex = 1;
                }
            }
        }

        keyMappings = mappings.ToArray();
    }

    private void DisplayPressedKey(string key)
    {
        if (keyDisplayText == null) return;

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        keyDisplayText.text = string.Format(displayFormat, key);
        displayCoroutine = StartCoroutine(HideKeyAfterDelay());
        
    }

    private IEnumerator HideKeyAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (keyDisplayText != null)
        {
            keyDisplayText.text = string.Empty;
        }
    }
}
