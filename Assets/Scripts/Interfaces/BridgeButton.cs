using UnityEngine;

public class BridgeButton : MonoBehaviour, IBridgeController
{
    private Color originalColor;
    private Renderer originalColorRenderer;

    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject platform;

    private int currentPoint = 0;
    private bool isActivated = false;


    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    void Update()
    {
        if (isActivated && platform != null)
        {
            MovePlatformToNextPoint(platform);
        }
    }

    void MovePlatformToNextPoint(GameObject platform)
    {
        if (points.Length == 0)
            return;



        Vector3 direction = points[currentPoint].position - platform.transform.position;

        direction.Normalize();

        platform.transform.Translate(speed * Time.deltaTime * direction);

        float distanceToNextPoint = Vector3.Distance(platform.transform.position, points[currentPoint].position);

        if (distanceToNextPoint < 0.1f)
        {
            currentPoint = (currentPoint + 1) % points.Length;

            if (currentPoint == points.Length - 1)
            {
                isActivated = false;
            }
        }
    }

    public void ActivateBridge()
    {
        if (currentPoint == points.Length - 1)
        {
            isActivated = false;
        }
        else
        {
            isActivated = true;
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
        return currentPoint == points.Length - 1;
    }
}
