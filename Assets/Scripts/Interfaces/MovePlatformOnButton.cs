using UnityEngine;

public class MovePlatformOnButton : MonoBehaviour, IBridgeController
{
    public Transform targetObject; 
    public Transform pointA; 
    public Transform pointB; 
    public float moveSpeed = 1.0f;

    private Color originalColor;
    private Renderer originalColorRenderer;
    private bool isMoving = false; 
    private float t = 0.0f; 

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void ActivateBridge()
    {
        if (!isMoving)
        {
            isMoving = true;
            t = 0.0f;
        }
    }

    private void Update()
    {
        if (isMoving && targetObject != null)
        {
            MovePlatformToNextPoint();
        }
    }

    void MovePlatformToNextPoint()
    {
        t += Time.deltaTime * moveSpeed;

        targetObject.position = Vector3.Lerp(pointA.position, pointB.position, t);

        if (t >= 1.0f)
        {
            isMoving = false;
        }
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

    public bool IsPlatformInTheRightPosition()
    {
        return !isMoving && t >= 1.0f; // Jeœli targetObject osi¹gn¹³ punkt B i przesta³ siê poruszaæ
    }
}
