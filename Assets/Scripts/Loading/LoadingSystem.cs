using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    public static LoadingSystem Instance;

    private void Awake()
    {
        // Upewnij siê, ¿e tylko jedna instancja skryptu istnieje (singleton).
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zabezpiecz przed zniszczeniem.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Upewnij siê, ¿e obraz zaczyna siê jako ca³kowicie nieprzezroczysty.
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        // Rozpocznij ³adowanie sceny asynchronicznie, ale nie pozwól, aby zakoñczy³a siê automatycznie.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Fade-out (ciemny ekran)
        yield return StartCoroutine(Fade(1f));

        // Czekaj, a¿ scena siê za³aduje
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Scena za³adowana w 90%, gotowa do aktywacji
                break;
            }
            yield return null;
        }

        // Aktywuj scenê
        asyncLoad.allowSceneActivation = true;

        // Fade-in (odciemniaj ekran)
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        loadingCanvas.SetActive(true);
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }

        // Upewnij siê, ¿e alfa osi¹gnê³a wartoœæ docelow¹.
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
        loadingCanvas.SetActive(false);
    }
}
