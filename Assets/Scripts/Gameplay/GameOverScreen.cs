using UnityEngine;
using DG.Tweening;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject textMeshProPanel;
    [SerializeField] private GameObject buttonHolder;
    private readonly float tweenTime = 0.1f;
    public string sceneName = "MainMenu";

    private void Start()
    {
        StartAnimYouAreDeadText();
    }

    private void StartAnimYouAreDeadText()
    {
        buttonHolder.SetActive(false);
        textMeshProPanel.SetActive(true);
        textMeshProPanel.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(textMeshProPanel.transform.DOScale(Vector3.one, tweenTime).SetEase(Ease.OutBack));
        seq.AppendInterval(0.25f);
        seq.Append(textMeshProPanel.transform.DOLocalMoveY(
            textMeshProPanel.transform.localPosition.y + 150f,
            0.3f
        ).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => buttonHolder.SetActive(true));
    }

    public void ReturnToMainMenu()
    {
        LoadingSystem.Instance.LoadLevel(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("wychodzisz z gry");
        Application.Quit();
    }
}
