using UnityEngine;

public class MovePlatformOnButton : MonoBehaviour, IBridgeController
{
    public Transform targetObject; // Obiekt, który chcemy poruszaæ
    public Transform pointA; // Punkt A
    public Transform pointB; // Punkt B
    public float moveSpeed = 1.0f; // Prêdkoœæ ruchu

    private Color originalColor;
    private Renderer originalColorRenderer;
    private bool isMoving = false; // Flaga, czy obiekt siê porusza
    private float t = 0.0f; // Czas do Lerp

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
            t = 0.0f; // Resetowanie czasu
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
        // Zwiêkszamy czas t w zale¿noœci od prêdkoœci
        t += Time.deltaTime * moveSpeed;

        // Przesuwamy targetObject miêdzy punktami A i B
        targetObject.position = Vector3.Lerp(pointA.position, pointB.position, t);

        // Zatrzymanie ruchu, gdy osi¹gniemy punkt B
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
