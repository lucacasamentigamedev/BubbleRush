using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    // Funzione per avviare lo shake
    public void Shake(float shakeMagnitude, float shakeDuration)
    {
        StartCoroutine(ShakeCoroutine(shakeMagnitude, shakeDuration));
    }

    private IEnumerator ShakeCoroutine(float shakeMagnitude, float shakeDuration)
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Movimento casuale per il camera shake
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ripristina la posizione originale
        transform.position = originalPosition;
    }
}
