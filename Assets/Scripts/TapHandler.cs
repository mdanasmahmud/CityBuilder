using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TapHandler : MonoBehaviour
{
    public GameObject targetObject; // The game object to scale
    public DollarManager dollarManager; // Reference to the DollarManager script
    public float scaleDuration = 0.1f; // Duration of the scale transition
    public GameObject shrinkEffectPrefab; // Prefab to play when shrinking
    public AudioClip tapAudioClip; // Audio clip to play on tap
    public float effectDuration = 2f; // Duration the smoke particles should be visible

    private Vector3 originalScale;
    private bool isScaling = false;
    private AudioSource audioSource;

    void Start()
    {
        if (targetObject != null)
        {
            originalScale = targetObject.transform.localScale;
        }

        // Add an AudioSource component to the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPosition = Input.mousePosition;
            if (!IsTapOnButton(tapPosition))
            {
                if (!isScaling)
                {
                    StartCoroutine(ShrinkObject());
                }
                if (dollarManager != null)
                {
                    dollarManager.IncreaseDollarRateByTap(3f);
                }

                // Play the tap audio clip
                if (tapAudioClip != null)
                {
                    audioSource.PlayOneShot(tapAudioClip);
                }
            }
        }
    }

    bool IsTapOnButton(Vector2 tapPosition)
    {
        foreach (Button button in dollarManager.buttons)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, tapPosition))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator ShrinkObject()
    {
        isScaling = true;
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * 0.8f; // Shrink to 80% of the original size

        // Instantiate the shrink effect prefab
        if (shrinkEffectPrefab != null)
        {
            GameObject effectInstance = Instantiate(shrinkEffectPrefab, targetObject.transform.position, Quaternion.identity);
            Destroy(effectInstance, effectDuration); // Destroy the effect after the specified duration
        }

        while (elapsedTime < scaleDuration)
        {
            targetObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            targetObject.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.localScale = originalScale;
        isScaling = false;
    }
}
