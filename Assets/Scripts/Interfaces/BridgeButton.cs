using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class BridgeButton : MonoBehaviour, IBridgeController
{
    private Color originalColor;
    private Renderer originalColorRenderer;

    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject buttonObject;

    [SerializeField] private float buttonPressDistance = 0.05f;        
    [SerializeField] private float buttonPressDuration = 0.1f;         
    [SerializeField] private AnimationCurve buttonPressCurve = null;    

    private int currentPoint = 0;
    private bool isActivated = false;

    private Vector3 buttonInitialLocalPos;
    private Coroutine buttonAnimRoutine;


    public LocalizedString localizeStringEvent;


    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        if (buttonObject != null)
        {
            buttonInitialLocalPos = buttonObject.transform.localPosition;
        }
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

        platform.transform.Translate(speed * Time.deltaTime * direction, Space.World);

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

        if (buttonObject != null)
        {
            if (buttonAnimRoutine != null)
                StopCoroutine(buttonAnimRoutine);

            buttonAnimRoutine = StartCoroutine(PlayButtonPressAnimation());
        }
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEvent,"Press [E] to Activate Bridge!");
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

    private IEnumerator PlayButtonPressAnimation()
    {
        Transform t = buttonObject.transform;
        Vector3 startPos = buttonInitialLocalPos;

        Vector3 localDownDir;
        if (t.parent != null)
            localDownDir = t.parent.InverseTransformDirection(-t.up); 
        else
            localDownDir = -t.up; 

        localDownDir.Normalize();
        Vector3 downPos = startPos + localDownDir * buttonPressDistance;

        float dur = Mathf.Max(0.0001f, buttonPressDuration);

        float t01 = 0f;
        while (t01 < 1f)
        {
            t01 += Time.deltaTime / dur;
            float k = Mathf.Clamp01(t01);
            k = buttonPressCurve != null ? buttonPressCurve.Evaluate(k) : k;
            t.localPosition = Vector3.LerpUnclamped(startPos, downPos, k);
            yield return null;
        }

        t01 = 0f;
        while (t01 < 1f)
        {
            t01 += Time.deltaTime / dur;
            float k = Mathf.Clamp01(t01);
            k = buttonPressCurve != null ? buttonPressCurve.Evaluate(k) : k;
            t.localPosition = Vector3.LerpUnclamped(downPos, startPos, k);
            yield return null;
        }

        t.localPosition = startPos;
        buttonAnimRoutine = null;
    }
}
