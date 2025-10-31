using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DirectOfDanger : MonoBehaviour
{
    [Header("Main Images")]
    [SerializeField] private Image upDirectionDamage;
    [SerializeField] private Image downDirectionDamage;
    [SerializeField] private Image rightDirectionDamage;
    [SerializeField] private Image leftDirectionDamage;
    [Header("Damage Indicators Icons")]
    [SerializeField] private Image upDirectionIndicator;
    [SerializeField] private Image downDirectionIndicator;
    [SerializeField] private Image rightDirectionIndicator;
    [SerializeField] private Image leftDirectionIndicator;

    [Header("Animacja")]
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float scaleMultiplier = 1.15f;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.22f;
    [SerializeField] private float shakeMagnitude = 8f; 

    [Header("Throttling")]
    [SerializeField] private float minShowInterval = 0.08f; 

    private Coroutine upCoroutine;
    private Coroutine downCoroutine;
    private Coroutine rightCoroutine;
    private Coroutine leftCoroutine;

    private Coroutine upIndicatorCoroutine;
    private Coroutine downIndicatorCoroutine;
    private Coroutine rightIndicatorCoroutine;
    private Coroutine leftIndicatorCoroutine;

    private float lastShowUp;
    private float lastShowDown;
    private float lastShowRight;
    private float lastShowLeft;

    private void Start()
    {
        HideLeftDirectionDmage();
        HideRightDirectionDmage();
        HideDownDirectionDmage();
        HideUpDirectionDmage();
    }


    public void NotifyDamageFrom(Vector3 sourceWorldPos, Transform playerTransform, Camera cameraToUse = null, bool relativeToCamera = true)
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("DirectOfDanger.NotifyDamageFrom: playerTransform is null");
            return;
        }

        Vector3 dir = sourceWorldPos - playerTransform.position;
        
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        dir.Normalize();

        Transform reference = playerTransform;
        if (relativeToCamera)
        {
            Camera cam = cameraToUse != null ? cameraToUse : Camera.main;
            if (cam != null && cam.transform != null)
                reference = cam.transform;
           
        }

  
        Vector3 local = reference.InverseTransformDirection(dir);
        float absX = Mathf.Abs(local.x);
        float absZ = Mathf.Abs(local.z);

        if (absZ >= absX)
        {
            if (local.z > 0f)
            {
                if (Time.time - lastShowUp >= minShowInterval)
                {
                    PlayUpDirectionOfDanger();
                    lastShowUp = Time.time;
                }
            }
            else
            {
                if (Time.time - lastShowDown >= minShowInterval)
                {
                    PlayDownDirectionOfDanger();
                    lastShowDown = Time.time;
                }
            }
        }
        else
        {
            if (local.x > 0f)
            {
                if (Time.time - lastShowRight >= minShowInterval)
                {
                    PlayRightDirectionOfDanger();
                    lastShowRight = Time.time;
                }
            }
            else
            {
                if (Time.time - lastShowLeft >= minShowInterval)
                {
                    PlayLeftDirectionOfDanger();
                    lastShowLeft = Time.time;
                }
            }
        }
    }

    [ContextMenu("Play Left")]
    public void PlayLeftDirectionOfDanger()
    {
        ShowLeftDirectionDmage();
        if (leftCoroutine != null) StopCoroutine(leftCoroutine);
        leftCoroutine = StartCoroutine(AnimDirectionOfDanger(leftDirectionDamage, () =>
        {
            HideLeftDirectionDmage();
            leftCoroutine = null;
        }));

        if (leftIndicatorCoroutine != null) StopCoroutine(leftIndicatorCoroutine);
        leftIndicatorCoroutine = StartCoroutine(ShakeIndicator(leftDirectionIndicator, shakeDuration, shakeMagnitude, () =>
        {
            leftIndicatorCoroutine = null;
        }));
    }

    [ContextMenu("Play Right")]
    public void PlayRightDirectionOfDanger()
    {
        ShowRightDirectionDmage();
        if (rightCoroutine != null) StopCoroutine(rightCoroutine);
        rightCoroutine = StartCoroutine(AnimDirectionOfDanger(rightDirectionDamage, () =>
        {
            HideRightDirectionDmage();
            rightCoroutine = null;
        }));

        if (rightIndicatorCoroutine != null) StopCoroutine(rightIndicatorCoroutine);
        rightIndicatorCoroutine = StartCoroutine(ShakeIndicator(rightDirectionIndicator, shakeDuration, shakeMagnitude, () =>
        {
            rightIndicatorCoroutine = null;
        }));
    }

    [ContextMenu("Play Down")]
    public void PlayDownDirectionOfDanger()
    {
        ShowDownDirectionDmage();
        if (downCoroutine != null) StopCoroutine(downCoroutine);
        downCoroutine = StartCoroutine(AnimDirectionOfDanger(downDirectionDamage, () =>
        {
            HideDownDirectionDmage();
            downCoroutine = null;
        }));

        if (downIndicatorCoroutine != null) StopCoroutine(downIndicatorCoroutine);
        downIndicatorCoroutine = StartCoroutine(ShakeIndicator(downDirectionIndicator, shakeDuration, shakeMagnitude, () =>
        {
            downIndicatorCoroutine = null;
        }));
    }

    [ContextMenu("Play Up")]
    public void PlayUpDirectionOfDanger()
    {
        ShowUpDirectionDmage();
        if (upCoroutine != null) StopCoroutine(upCoroutine);
        upCoroutine = StartCoroutine(AnimDirectionOfDanger(upDirectionDamage, () =>
        {
            HideUpDirectionDmage();
            upCoroutine = null;
        }));

        if (upIndicatorCoroutine != null) StopCoroutine(upIndicatorCoroutine);
        upIndicatorCoroutine = StartCoroutine(ShakeIndicator(upDirectionIndicator, shakeDuration, shakeMagnitude, () =>
        {
            upIndicatorCoroutine = null;
        }));
    }

    private IEnumerator AnimDirectionOfDanger(Image image, Action onComplete)
    {
        if (image == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        var transformToAnimate = image.transform;
        var startScale = Vector3.one;

        AnimationCurve curve = animCurve;
        float duration;

        if (curve == null || curve.length == 0)
        {
            curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
            duration = 0.24f;
        }
        else
        {
            duration = curve.keys[curve.length - 1].time;
            if (duration <= 0f) duration = 0.24f;
        }

        transformToAnimate.localScale = startScale;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float sampleTime = Mathf.Clamp(elapsed, 0f, duration);
            float eval = curve.Evaluate(sampleTime);
            float currentScale = Mathf.Lerp(1f, scaleMultiplier, eval);
            transformToAnimate.localScale = Vector3.one * currentScale;
            yield return null;
        }

        transformToAnimate.localScale = startScale;
        onComplete?.Invoke();
    }

    private IEnumerator ShakeIndicator(Image indicator, float duration, float magnitude, Action onComplete)
    {
        if (indicator == null)
        {
            onComplete?.Invoke();
            yield break;
        }

    
        RectTransform rt = indicator.transform as RectTransform;
        Vector3 startPos;
        bool useAnchored = false;
        Vector2 startAnchored = Vector2.zero;

        if (rt != null)
        {
            startAnchored = rt.anchoredPosition;
            startPos = rt.localPosition;
            useAnchored = true;
        }
        else
        {
            startPos = indicator.transform.localPosition;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
         
            float currentMag = Mathf.Lerp(magnitude, 0f, t);
            Vector2 offset = UnityEngine.Random.insideUnitCircle * currentMag;

            if (useAnchored)
                rt.anchoredPosition = startAnchored + offset;
            else
                indicator.transform.localPosition = startPos + (Vector3)offset;

            yield return null;
        }

        
        if (useAnchored)
            rt.anchoredPosition = startAnchored;
        else
            indicator.transform.localPosition = startPos;

        onComplete?.Invoke();
    }

    private void ShowUpDirectionDmage()
    {
        upDirectionDamage.gameObject.SetActive(true);
    }

    private void HideUpDirectionDmage()
    {
        upDirectionDamage.gameObject.SetActive(false);
    }

    private void ShowDownDirectionDmage()
    {
        downDirectionDamage.gameObject.SetActive(true);
    }

    private void HideDownDirectionDmage()
    {
        downDirectionDamage.gameObject.SetActive(false);
    }

    private void ShowRightDirectionDmage()
    {
        rightDirectionDamage.gameObject.SetActive(true);
    }

    private void HideRightDirectionDmage()
    {
        rightDirectionDamage.gameObject.SetActive(false);
    }

    private void ShowLeftDirectionDmage()
    {
        leftDirectionDamage.gameObject.SetActive(true);
    }

    private void HideLeftDirectionDmage()
    {
        leftDirectionDamage.gameObject.SetActive(false);
    }
}
