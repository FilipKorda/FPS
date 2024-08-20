using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    [SerializeField] private float cameraShakeDuration = 0.75f;
    [SerializeField] private float cameraShakeMagnitude = 0.1f;

    void Awake()
    {
        Instance = this;
    }

    public void AlarmPlayer()
    {
        StartCoroutine(Shake(cameraShakeDuration, cameraShakeMagnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
