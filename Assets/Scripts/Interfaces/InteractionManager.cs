using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float maxDetectionRadius = 5f;
    [SerializeField] private Image eagleVision;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float durationToMaintain = 3.95f;
    [SerializeField] private float fadeOutDuration = 0.2f;

    private float detectionRadius = 0f;
    private bool isExpanding = false;
    [SerializeField] private float radiusIncreaseSpeed = 2.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {           
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

}
