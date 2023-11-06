using System.Collections;
using UnityEngine;

public class EagleVisionWave : MonoBehaviour
{
    [SerializeField] private EagleVisionManager interactionManager;
    private readonly int pointCounts = 50;
    private readonly float startWidth = 1f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCounts + 1;
    }

    private IEnumerator Wave()
    {
        float currentRadius = 0f;
        while (currentRadius < interactionManager.maxDetectionRadius)
        {
            currentRadius += Time.deltaTime * interactionManager.radiusIncreaseSpeed;
            Draw(currentRadius);
            yield return null;
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointCounts;
        for (int i = 0; i <= pointCounts; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / interactionManager.maxDetectionRadius);
    }

    private void Update()
    {
        if (interactionManager.isExpanding)
        {
            StartCoroutine(Wave());
        }
    }
}
