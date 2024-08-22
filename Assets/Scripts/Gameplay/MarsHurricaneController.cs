using System.Collections;
using UnityEngine;

public class MarsHurricaneController : MonoBehaviour
{
    [SerializeField] private float normalFogDensity = 0.002f;
    [SerializeField] private float hurricaneFogDesity = 0.1f;
    [SerializeField] private float transitionDuration = 4f;
    public float hurricaneDuration = 30f;
    [SerializeField] private ParticleSystem ps_MarsHurricane;
    public bool isHurricaneActive = false;
    private float transitionProgress = 0f;

    private void Start()
    {
        RenderSettings.fogDensity = normalFogDensity;
        ps_MarsHurricane.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isHurricaneActive && transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            RenderSettings.fogDensity = Mathf.Lerp(normalFogDensity, hurricaneFogDesity, transitionProgress);
        }
        else if (!isHurricaneActive && transitionProgress > 0f)
        {
            transitionProgress -= Time.deltaTime / transitionDuration;
            RenderSettings.fogDensity = Mathf.Lerp(normalFogDensity, hurricaneFogDesity, transitionProgress);
        }
    }

    public void ActiveHurricaneFog()
    {
        isHurricaneActive = true;
        ps_MarsHurricane.gameObject.SetActive(true);
    }

    public void DeactiveHurricaneFog()
    {
        isHurricaneActive = false;
        StartCoroutine(DelayToStop_MarsHurricane());
    }

    private IEnumerator DelayToStop_MarsHurricane()
    {
        yield return new WaitForSeconds(4);
        ps_MarsHurricane.gameObject.SetActive(false);
    }

    public void DeactivePs_MarsHurricane()
    {
        ps_MarsHurricane.gameObject.SetActive(false);
    }

    public void ActivePs_MarsHurricane()
    {
        ps_MarsHurricane.gameObject.SetActive(true);
    }
}
