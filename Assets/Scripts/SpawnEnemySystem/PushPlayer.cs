using System.Collections;
using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    [SerializeField] private Collider thisCollider;

    [Header("Force settings")]
    [SerializeField] private float minUpForce = 8f;
    [SerializeField] private float maxUpForce = 12f;
    [SerializeField] private float minHorizontalForce = 2f;
    [SerializeField] private float maxHorizontalForce = 6f;
    [SerializeField] private float airTime = 0.6f;
    [SerializeField] private float horizontalDrag = 2f;
    [SerializeField] private float gravity = 25f;

    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var cc = other.GetComponent<CharacterController>();
            if (cc == null) cc = other.GetComponentInParent<CharacterController>();
            if (cc != null)
            {
                StartCoroutine(LaunchPlayer(cc));
            }
        }
    }

    private IEnumerator LaunchPlayer(CharacterController cc)
    {
        var playerController = cc.GetComponent<PlayerController>();
        bool restorePlayerController = false;
        if (playerController != null && playerController.enabled)
        {
            playerController.enabled = false;
            restorePlayerController = true;
        }

        Vector3 horizontalDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (horizontalDir.sqrMagnitude < 0.001f) horizontalDir = Vector3.forward;
        horizontalDir.Normalize();

        float hForce = Random.Range(minHorizontalForce, maxHorizontalForce);
        float yForce = Random.Range(minUpForce, maxUpForce);

        Vector3 velocity = horizontalDir * hForce + Vector3.up * yForce;

        float elapsed = 0f;
        while (elapsed < airTime)
        {
            cc.Move(velocity * Time.deltaTime);

            velocity.y -= gravity * Time.deltaTime;

            velocity.x = Mathf.Lerp(velocity.x, 0f, horizontalDrag * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, 0f, horizontalDrag * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (restorePlayerController && playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
