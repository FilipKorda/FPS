using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WinGame : MonoBehaviour
{
    public string sceneName = "MainMenu";

    [SerializeField] private StatisticManager statisticManager;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private Button[] buttons;

    [SerializeField] private float slowDurationSeconds = 5f;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Image winScreenImage;

    [SerializeField] private Image upImage;
    [SerializeField] private Image downImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Image leftImage;

    [Header("Close screen")]
    [SerializeField] private float closeOverlap = 4f;

    [Header("Counter bump")]
    [SerializeField] private float bumpScale = 1.2f;
    [SerializeField] private float bumpInDuration = 0.08f;
    [SerializeField] private float bumpOutDuration = 0.15f;

    private Coroutine _slowRoutine;
    private Vector3 _counterOriginalScale;

    public bool playerIsWin = false;

    public static float SlowFactor => Time.timeScale;

    private void Start()
    {
        if (counterText != null)
        {
            _counterOriginalScale = counterText.rectTransform.localScale;
            counterText.gameObject.SetActive(false);
        }

        if (winScreenImage != null)
        {
            winScreenImage.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Active Win Game Panel")]
    public void ActivateWinGamePanel()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayWin();

        playerIsWin = true;

        if (_slowRoutine != null)
            StopCoroutine(_slowRoutine);

        _slowRoutine = StartCoroutine(SlowDownToStop(slowDurationSeconds));
    }

    private IEnumerator SlowDownToStop(float duration)
    {
        float startScale = Time.timeScale;
        float fixedDeltaRatio = Time.fixedDeltaTime / Mathf.Max(Time.timeScale, 0.0001f);

        if (counterText != null)
        {
            counterText.gameObject.SetActive(true);
            int startMillis = Mathf.Max(0, Mathf.RoundToInt(duration * 1000f));
            counterText.text = FormatMillis(startMillis);
            PlayCounterBump();
        }

        StartCloseScreen(duration);

        int lastMillis = Mathf.Max(0, Mathf.RoundToInt(duration * 1000f));

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float newScale = Mathf.Lerp(startScale, 0f, t);
            Time.timeScale = newScale;

            Time.fixedDeltaTime = Mathf.Max(fixedDeltaRatio * newScale, 0.00001f);

            if (counterText != null)
            {
                int millis = Mathf.Max(0, Mathf.RoundToInt((duration - elapsed) * 1000f));

                if (millis != lastMillis)
                {
                    counterText.text = FormatMillis(millis);

                    int currentWhole = millis / 1000;
                    int lastWhole = lastMillis / 1000;
                    if (currentWhole != lastWhole)
                        PlayCounterBump();

                    lastMillis = millis;
                }
            }

            yield return null;
        }

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.00001f;

        if (counterText != null)
        {
            counterText.text = FormatMillis(0);
            PlayCounterBump();
        }

        ShowWinScreenAndHideOthers();

        _slowRoutine = null;
    }

    private void StartCloseScreen(float duration)
    {
        float halfOverlap = closeOverlap * 0.5f;

        if (upImage != null) TweenEdgeToCenter(upImage.rectTransform, MoveDir.Down, duration, halfOverlap);
        if (downImage != null) TweenEdgeToCenter(downImage.rectTransform, MoveDir.Up, duration, halfOverlap);
        if (leftImage != null) TweenEdgeToCenter(leftImage.rectTransform, MoveDir.Right, duration, halfOverlap);
        if (rightImage != null) TweenEdgeToCenter(rightImage.rectTransform, MoveDir.Left, duration, halfOverlap);
    }

    private enum MoveDir { Up, Down, Left, Right }

    private void TweenEdgeToCenter(RectTransform rt, MoveDir dir, float duration, float halfOverlap)
    {
        if (rt == null) return;
        var parent = rt.transform.parent as RectTransform;
        if (parent == null) return;


        var world = new Vector3[4];
        rt.GetWorldCorners(world);

        for (int i = 0; i < 4; i++)
            world[i] = parent.InverseTransformPoint(world[i]);


        Vector2 center = parent.rect.center;

        float endX = rt.anchoredPosition.x;
        float endY = rt.anchoredPosition.y;

        switch (dir)
        {
            case MoveDir.Down:
                {
                    float bottomY = world[0].y;
                    float targetEdgeY = center.y - halfOverlap;
                    float deltaY = targetEdgeY - bottomY;
                    endY = rt.anchoredPosition.y + deltaY;
                    rt.DOKill();
                    rt.DOAnchorPosY(endY, duration).SetEase(Ease.Linear).SetUpdate(true);
                    break;
                }
            case MoveDir.Up:
                {
                    float topY = world[1].y;
                    float targetEdgeY = center.y + halfOverlap;
                    float deltaY = targetEdgeY - topY;
                    endY = rt.anchoredPosition.y + deltaY;
                    rt.DOKill();
                    rt.DOAnchorPosY(endY, duration).SetEase(Ease.Linear).SetUpdate(true);
                    break;
                }
            case MoveDir.Left:
                {
                    float leftX = world[1].x;
                    float targetEdgeX = center.x - halfOverlap;
                    float deltaX = targetEdgeX - leftX;
                    endX = rt.anchoredPosition.x + deltaX;
                    rt.DOKill();
                    rt.DOAnchorPosX(endX, duration).SetEase(Ease.Linear).SetUpdate(true);
                    break;
                }
            case MoveDir.Right:
                {
                    float rightX = world[2].x;
                    float targetEdgeX = center.x + halfOverlap;
                    float deltaX = targetEdgeX - rightX;
                    endX = rt.anchoredPosition.x + deltaX;
                    rt.DOKill();
                    rt.DOAnchorPosX(endX, duration).SetEase(Ease.Linear).SetUpdate(true);
                    break;
                }
        }
    }

    private string FormatMillis(int millis)
    {
        int seconds = millis / 1000;
        int ms = millis % 1000;
        return string.Format("{0}.{1:000}", seconds, ms);
    }

    private void PlayCounterBump()
    {
        if (counterText == null) return;

        var t = counterText.rectTransform;
        t.DOKill(true);
        t.localScale = _counterOriginalScale;

        DOTween.Sequence()
            .SetUpdate(true)
            .Append(t.DOScale(_counterOriginalScale * bumpScale, bumpInDuration).SetEase(Ease.OutQuad))
            .Append(t.DOScale(_counterOriginalScale, bumpOutDuration).SetEase(Ease.InQuad));
    }

    private void ShowWinScreenAndHideOthers()
    {
        if (upImage != null) upImage.rectTransform.DOKill();
        if (downImage != null) downImage.rectTransform.DOKill();
        if (leftImage != null) leftImage.rectTransform.DOKill();
        if (rightImage != null) rightImage.rectTransform.DOKill();
        if (counterText != null) counterText.rectTransform.DOKill();

        if (upImage != null) upImage.gameObject.SetActive(false);
        if (downImage != null) downImage.gameObject.SetActive(false);
        if (leftImage != null) leftImage.gameObject.SetActive(false);
        if (rightImage != null) rightImage.gameObject.SetActive(false);
        if (counterText != null) counterText.gameObject.SetActive(false);

        winScreenImage.gameObject.SetActive(true);

        statisticManager.GenerateStats();
        statisticManager.PlayEntryAnimation();
    }

    public void ReturnToMainMenu()
    {
        LoadingSystem.Instance.LoadLevel(sceneName);

        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
