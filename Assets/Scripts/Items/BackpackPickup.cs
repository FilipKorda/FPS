using FPS.Guns.Demo;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class BackpackPickup : MonoBehaviour, IBackpackPickupable
{
    [SerializeField] private GameObject[] uiElementsToEnable;
    [SerializeField] private GameObject gunGameObject;

    [Header("Sekwencja UI")]
    [SerializeField] private float uiRevealDelay = 0.25f;
    [SerializeField] private bool deactivateAfterSequence = true;

    [Header("Efekty UI")]
    [SerializeField] private float uiFadeDuration = 0.3f;
    [SerializeField] private AnimationCurve uiFadeCurve = null;

    [SerializeField] private Renderer highlightRenderer;
    [SerializeField] private Color highlightColor = Color.cyan;
    private Color? _originalColor;

    private bool _pickedUp;
    private Coroutine _revealRoutine;

    [SerializeField] private GetBackpackQuest getBackpackQuest;
    [SerializeField] private PlayerGunSelector playerGunSelector;

    public LocalizedString localizeStringEvent;

    private void Awake()
    {
        if (gunGameObject != null)
            gunGameObject.SetActive(false);

        if (uiFadeCurve == null)
            uiFadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    public void PickupBackpack()
    {
        if (_pickedUp) return;
        _pickedUp = true;

        getBackpackQuest.isBackpackSet = true;

        HideHint();
        ResetHighlight();

        HideBackpackVisualsAndCollision();

        if (_revealRoutine != null) StopCoroutine(_revealRoutine);
        _revealRoutine = StartCoroutine(RevealUISequence());
    }

    private IEnumerator RevealUISequence()
    {
        if (uiElementsToEnable != null)
        {
            for (int i = 0; i < uiElementsToEnable.Length; i++)
            {
                var go = uiElementsToEnable[i];
                if (go != null)
                {
                    yield return StartCoroutine(FadeInUI(go));

                    if (uiRevealDelay > 0f && i < uiElementsToEnable.Length - 1)
                        yield return new WaitForSeconds(uiRevealDelay);
                }
            }
        }

        yield return new WaitForSeconds(1);

        if (gunGameObject != null)
        {
            gunGameObject.SetActive(true);
        }

        playerGunSelector.PlayDrawAnimationInIntro();

        if (PlayerSingleton.Instance != null)
        {
            PlayerSingleton.Instance.canShoot = true;
        }

        if (deactivateAfterSequence)
        {
            gameObject.SetActive(false);
        }

        _revealRoutine = null;
    }

    private IEnumerator FadeInUI(GameObject go)
    {
        go.SetActive(true);

        var cg = go.GetComponent<CanvasGroup>();
        if (cg == null) cg = go.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        bool prevInteractable = cg.interactable;
        bool prevBlocksRaycasts = cg.blocksRaycasts;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float duration = Mathf.Max(0.0001f, uiFadeDuration);
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float a = uiFadeCurve != null ? uiFadeCurve.Evaluate(p) : p;
            cg.alpha = a;
            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = prevInteractable || true;
        cg.blocksRaycasts = prevBlocksRaycasts || true;
    }

    private void HideBackpackVisualsAndCollision()
    {
        var renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        foreach (var r in renderers) r.enabled = false;

        var colliders = GetComponentsInChildren<Collider>(includeInactive: true);
        foreach (var c in colliders) c.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    public void Highlight()
    {
        if (highlightRenderer != null)
        {
            if (_originalColor == null)
            {
                var mat = highlightRenderer.material;
                _originalColor = mat.HasProperty("_Color") ? mat.color : Color.white;
            }
            var m = highlightRenderer.material;
            if (m.HasProperty("_Color"))
            {
                m.color = highlightColor;
            }
        }
    }

    public void ResetHighlight()
    {
        if (highlightRenderer != null && _originalColor.HasValue)
        {
            var m = highlightRenderer.material;
            if (m.HasProperty("_Color"))
            {
                m.color = _originalColor.Value;
            }
        }
    }

    public void ShowHint()
    {
        NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEvent, "Press [E] to pick Up Backpack");
    }

    public void HideHint()
    {
        NotificationSystem.Instance.HideInfiniteNotification();
    }
}