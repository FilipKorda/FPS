using UnityEngine;
using UnityEngine.Localization;
using System.Collections;

public class MovePlatformOnButton : MonoBehaviour, IBridgeController
{
    public Transform buttonObejct;
    public Transform targetObject; 
    public Transform pointA; 
    public Transform pointB; 
    public float moveSpeed = 1.0f;

    [Header("Button Press Animation")]
    public float pressDepth = 0.05f;        // jak g³êboko wciska siê przycisk (w jednostkach œwiata rodzica)
    public float pressDuration = 0.08f;     // czas wciskania i odbicia (osobno)

    private Color originalColor;
    private Renderer originalColorRenderer;
    private bool isMoving = false; 
    private float t = 0.0f;

    public LocalizedString localizeStringEventPress;

    // stan animacji przycisku
    private Vector3 buttonInitialLocalPos;
    private Coroutine buttonPressCoroutine;

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        if (buttonObejct != null)
            buttonInitialLocalPos = buttonObejct.localPosition;
    }

    public void ActivateBridge()
    {
        // animacja wciœniêcia przycisku
        AnimateButtonPress();

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
        NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEventPress, "Press [E] to Activate Bridge!");
        originalColorRenderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        NotificationSystem.Instance.HideInfiniteNotification();
        originalColorRenderer.material.color = originalColor;
    }

    public bool IsPlatformInTheRightPosition()
    {
        return !isMoving && t >= 1.0f; 
    }

    // --- Animacja przycisku ---

    [ContextMenu("ASFDASDFASDF")]
    public void AnimateButtonPress()
    {
        if (buttonObejct == null)
            return;

        if (buttonPressCoroutine != null)
            StopCoroutine(buttonPressCoroutine);

        // upewnij siê, ¿e mamy poprawn¹ pozycjê startow¹ (np. po respawnie/enable)
        buttonInitialLocalPos = buttonObejct.localPosition;

        buttonPressCoroutine = StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation()
    {
        // kierunek wciskania: w dó³ wzglêdem lokalnej osi Up przycisku (przekszta³conej do przestrzeni rodzica)
        Vector3 pressDirParentSpace;
        if (buttonObejct.parent != null)
            pressDirParentSpace = buttonObejct.parent.InverseTransformDirection(buttonObejct.up) * -1f;
        else
            pressDirParentSpace = -buttonObejct.up; // bez rodzica – u¿yj przestrzeni œwiata

        pressDirParentSpace = pressDirParentSpace.normalized;

        Vector3 start = buttonInitialLocalPos;
        Vector3 down = start + pressDirParentSpace * pressDepth;

        // faza wciskania
        float elapsed = 0f;
        while (elapsed < pressDuration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / pressDuration));
            buttonObejct.localPosition = Vector3.LerpUnclamped(start, down, k);
            yield return null;
        }

        // faza odbicia
        elapsed = 0f;
        while (elapsed < pressDuration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / pressDuration));
            buttonObejct.localPosition = Vector3.LerpUnclamped(down, start, k);
            yield return null;
        }

        buttonObejct.localPosition = start;
        buttonPressCoroutine = null;
    }

    private void OnDisable()
    {
        if (buttonPressCoroutine != null)
        {
            StopCoroutine(buttonPressCoroutine);
            buttonPressCoroutine = null;
        }

        if (buttonObejct != null)
            buttonObejct.localPosition = buttonInitialLocalPos;
    }
}
