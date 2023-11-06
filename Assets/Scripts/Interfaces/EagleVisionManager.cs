using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EagleVisionManager : MonoBehaviour
{

    public float maxDetectionRadius = 15f;
    [SerializeField] private Image eagleVision;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float durationToMaintain = 4f;
    [SerializeField] private float fadeOutDuration = 0.2f;
    [SerializeField] private GameObject eagleVisionWave;
    private float detectionRadius = 0f;
    public bool isExpanding = false;
    public float radiusIncreaseSpeed = 10f;



    private void Update()
    {
        HandleEagleVision();
    }

    public void HandleEagleVision()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            ShakeCamera();
            eagleVisionWave.SetActive(true);
            isExpanding = true;
        }

        if (isExpanding)
        {
            detectionRadius += radiusIncreaseSpeed * Time.deltaTime;
            detectionRadius = Mathf.Clamp(detectionRadius, 0f, maxDetectionRadius);

            DetectObjectsAroundPlayer();

            if (detectionRadius >= maxDetectionRadius)
            {
                isExpanding = false;
            }
        }
        else
        {
            detectionRadius = 0f;
        }
    }

    private void DetectObjectsAroundPlayer()
    {
        Vector3 playerPosition = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, detectionRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var objectDetector))
            {
                objectDetector.Interact();
            }
        }

        eagleVision.DOFade(0.3f, fadeInDuration)
              .OnComplete(() => MaintainAlpha());
    }

    private void MaintainAlpha()
    {
        eagleVision.DOFade(0.3f, durationToMaintain)
            .OnComplete(() => StartFadeOut());
    }

    private void StartFadeOut()
    {
        eagleVision.DOFade(0f, fadeOutDuration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void ShakeCamera()
    {       
        float shakeDuration = 0.1f;
        float shakeIntensity = 0.2f;
        float zoomFOV = 55f;
        float originalFOV = 60f;
        float zoomDuration = 0.1f;

        Sequence cameraSequence = DOTween.Sequence();
        cameraSequence.Append(Camera.main.DOFieldOfView(zoomFOV, zoomDuration));
        cameraSequence.Append(Camera.main.transform.DOShakePosition(shakeDuration, shakeIntensity, 10, 90, false, false));
        cameraSequence.Append(Camera.main.DOFieldOfView(originalFOV, zoomDuration));
  
        cameraSequence.Play();
    }
}
