using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
    [Header("-= Loading System =-")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [Header("-= Loading Ratating Image =-")]
    [SerializeField] private Image loadingCircleImage;
    private int fullFillCount = 0;
    private float rotationZ = 0f;
    private Coroutine rotateCoroutine;
    private Coroutine animateCoroutine;

    public static LoadingSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string sceneName)
    {
        MusicManager.Instance.PlayLoading();

        StartAnimations();
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        Time.timeScale = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        loadingCanvas.SetActive(true);
        SetFadeAlpha(1f);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && fullFillCount >= 2)
            {
                StopAnimations();
                break;
            }
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        yield return new WaitForSecondsRealtime(0.5f);
        SetFadeAlpha(0f);

        loadingCanvas.SetActive(false);

        Time.timeScale = 1f;
    }

    private void SetFadeAlpha(float alpha)
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
    }

    public void StartAnimations()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        loadingCircleImage.gameObject.SetActive(true);

        fullFillCount = 0;
        rotationZ = 0f;

        if (rotateCoroutine == null)
            rotateCoroutine = StartCoroutine(RotateWholeObject());

        if (animateCoroutine == null)
            animateCoroutine = StartCoroutine(AnimateLoading());
    }

    public void StopAnimations()
    {
        loadingCircleImage.gameObject.SetActive(false);

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }

        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
            animateCoroutine = null;
        }
    }

    public IEnumerator RotateWholeObject()
    {
        while (rotationZ < 10000f)
        {
            rotationZ += Time.unscaledDeltaTime * 100f;
            if (rotationZ > 10000f) rotationZ = 10000f;

            loadingCircleImage.transform.rotation = Quaternion.Euler(0, 0, -rotationZ);

            yield return null;
        }
    }

    public IEnumerator AnimateLoading()
    {
        loadingCircleImage.type = Image.Type.Filled;
        loadingCircleImage.fillMethod = Image.FillMethod.Radial360;
        loadingCircleImage.fillOrigin = (int)Image.Origin360.Bottom;
        loadingCircleImage.fillAmount = 0f;
        loadingCircleImage.fillClockwise = true;

        float duration = 2f;

        while (true)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                loadingCircleImage.fillAmount = Mathf.Lerp(0f, 1f, t / duration);
                yield return null;
            }

            fullFillCount++;

            loadingCircleImage.fillClockwise = false;

            t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                loadingCircleImage.fillAmount = Mathf.Lerp(1f, 0f, t / duration);
                yield return null;
            }

            loadingCircleImage.fillClockwise = true;
        }
    }
}
