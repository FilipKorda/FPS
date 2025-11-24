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
    public float pressDepth = 0.05f;        
    public float pressDuration = 0.08f;     

    private Color originalColor;
    private Renderer originalColorRenderer;
    private bool isMoving = false; 
    private float t = 0.0f;

    public LocalizedString localizeStringEventPress;

    private Vector3 buttonInitialLocalPos;
    private Coroutine buttonPressCoroutine;

    [SerializeField] private AudioClip bridgeSound;
    private AudioSource loopIdleAudioSource;

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        if (buttonObejct != null)
            buttonInitialLocalPos = buttonObejct.localPosition;
    }

    public void ActivateBridge()
    {
        AnimateButtonPress();

        loopIdleAudioSource = AudioManager.Instance.PlayClip(bridgeSound, transform.position, 0.05f, true, 1, 500, 1, true, targetObject.transform);

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

            if (loopIdleAudioSource != null)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Stop(loopIdleAudioSource);
                }
                else
                {
                    loopIdleAudioSource.Stop();
                }
                loopIdleAudioSource = null;
            }
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

  

    [ContextMenu("ASFDASDFASDF")]
    public void AnimateButtonPress()
    {
        if (buttonObejct == null)
            return;

        if (buttonPressCoroutine != null)
            StopCoroutine(buttonPressCoroutine);

        buttonInitialLocalPos = buttonObejct.localPosition;

        buttonPressCoroutine = StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation()
    {
        Vector3 pressDirParentSpace;
        if (buttonObejct.parent != null)
            pressDirParentSpace = buttonObejct.parent.InverseTransformDirection(buttonObejct.up) * -1f;
        else
            pressDirParentSpace = -buttonObejct.up;

        pressDirParentSpace = pressDirParentSpace.normalized;

        Vector3 start = buttonInitialLocalPos;
        Vector3 down = start + pressDirParentSpace * pressDepth;

        float elapsed = 0f;
        while (elapsed < pressDuration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / pressDuration));
            buttonObejct.localPosition = Vector3.LerpUnclamped(start, down, k);
            yield return null;
        }

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
