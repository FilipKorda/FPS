using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton : MonoBehaviour, IBridgeController
{
    private Color originalColor;
    private Renderer originalColorRenderer;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform[] points;
    [SerializeField] private GameObject platform;
    [SerializeField] private float speed = 5f;
    private int currentPointIndex = 0;
    public bool isPlatformActive = false;

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

                if (currentPointIndex == 1 || currentPointIndex == 2)
                {
                    yield return new WaitForSeconds(2f);
                }

                if (currentPointIndex >= points.Length)
                {
                    StopMovementAndRestartIndex();
                }
            }

            if (playerController != null && playerController.isGrounded)
            {
                Vector3 moveWithPlatform = platform.transform.position - playerController.transform.position;
                playerController.Move(moveWithPlatform);
                
            }

            yield return null;
        }
    }

    private void MovePlatformTowardsPoint(Transform targetPoint)
    {
        Vector3 direction = (targetPoint.position - platform.transform.position).normalized;
        platform.transform.Translate(speed * Time.deltaTime * direction);
    }

    private void StopMovementAndRestartIndex()
    {
        isPlatformActive = false;
        currentPointIndex = 0;
    }


}
