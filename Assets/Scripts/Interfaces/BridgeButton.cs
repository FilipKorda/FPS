using System.Collections;
using UnityEngine;

public class BridgeButton : MonoBehaviour, IBridgeController
{
    private Color originalColor;
    private Renderer originalColorRenderer;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject platform;
    [SerializeField] public float speed = 5f;
    [SerializeField] private CharacterController characterController;
    private int currentPointIndex = 0;
    public bool isPlatformActive = false;

    public Vector3 direction;
    [SerializeField] private int[] stopIndexes;

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to Activate Bridge!");
        originalColorRenderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        NotificationSystem.Instance.HideInfiniteNotification();
        originalColorRenderer.material.color = originalColor;
    }

    public void ActivateBridge()
    {
        isPlatformActive = true;
        StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        while (isPlatformActive)
        {
            MovePlatformTowardsPoint(points[currentPointIndex]);

            if (Vector3.Distance(platform.transform.position, points[currentPointIndex].position) < 0.1f)
            {
                currentPointIndex++;

                if (IsStopIndex(currentPointIndex))
                {
                    yield return new WaitForSeconds(2f);
                }

                if (currentPointIndex >= points.Length)
                {
                    StopMovementAndRestartIndex();
                }
            }

            yield return null;
        }
    }

    public void MovePlatformTowardsPoint(Transform targetPoint)
    {
        direction = (targetPoint.position - platform.transform.position).normalized;
        platform.transform.Translate(speed * Time.deltaTime * direction);
    }

    private void StopMovementAndRestartIndex()
    {
        isPlatformActive = false;
        currentPointIndex = 0;
    }

    private bool IsStopIndex(int index)
    {
        foreach (var stopIndex in stopIndexes)
        {
            if (index == stopIndex)
            {
                return true;
            }
        }

        return false;
    }

}
