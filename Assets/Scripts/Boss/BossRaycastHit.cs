using System.Collections;
using UnityEngine;

public class BossRaycastHit : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float resetTime = 0.5f;
    [SerializeField] private ParticleSystem hitEffectPrefab;
    private bool hasHit = false;
    private bool shouldUseRaycast = false;

    void Update()
    {
        if (shouldUseRaycast)
        {
            if (!hasHit && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, layerMask))
            {
                if (hitEffectPrefab != null)
                {
                    ParticleSystem ps = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    ps.Play();

                    Destroy(ps.gameObject, ps.main.duration);
                }

                hasHit = true;
                StartCoroutine(ResetHitFlag());
            }
        }      
    }

    public void SetUseRaycast(bool value) => shouldUseRaycast = value;

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(resetTime);
        hasHit = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = hasHit ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
    }
}
