using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
    public GrenadeSO granatPrefab;
    public GrenadeSO smokeGranatPrefab;
    public Transform releaseTransform;

    private GameObject heldGrenade;
    public GrenadeType currentGrenadeType;
    public enum GrenadeType
    {
        Regular,
        Smoke
    }

    public delegate void GrenadeTypeChangedHandler(GrenadeType newType);
    public event GrenadeTypeChangedHandler GrenadeTypeChanged;

    private void Awake()
    {
        currentGrenadeType = GrenadeType.Regular;
    }

    void Update()
    {
        SetupGrenade();
        SwitchListOfGrenade();
    }

    private void SwitchListOfGrenade()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentGrenadeType == GrenadeType.Regular)
            {
                currentGrenadeType = GrenadeType.Smoke;
            }
            else
            {
                currentGrenadeType = GrenadeType.Regular;
            }

            GrenadeTypeChanged?.Invoke(currentGrenadeType);
        }
    }

    private void SetupGrenade()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (heldGrenade != null)
            {
                ThrowGrenade();
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldGrenade == null)
            {
                HoldGrenade();
            }
        }
    }

    void HoldGrenade()
    {
        int availableGrenades = currentGrenadeType == GrenadeType.Regular ? Inventory.Instance.currentGranat : Inventory.Instance.currentSmokeGranat;
        if (availableGrenades > 0)
        {
            GrenadeSO selectedGrenade = currentGrenadeType == GrenadeType.Regular ? granatPrefab : smokeGranatPrefab;
            heldGrenade = Instantiate(selectedGrenade.ModelPrefab, releaseTransform.position, releaseTransform.rotation);
            heldGrenade.GetComponent<Rigidbody>().isKinematic = true;
            heldGrenade.transform.parent = releaseTransform;

            if (currentGrenadeType == GrenadeType.Regular)
            {
                StatisticsCollector.AddGrenadeUsed();
                Inventory.Instance.RemoveGrenade();
            }
            else
            {
                StatisticsCollector.AddSmokeGrenadeUsed();
                Inventory.Instance.RemoveSmokeGrenade();
            }
        }
    }

    void ThrowGrenade()
    {
        if (heldGrenade != null)
        {
            GrenadeTypeChanged?.Invoke(currentGrenadeType);

            Transform pinTransform = heldGrenade.transform.GetChild(1);
            Rigidbody pinRigidbody = pinTransform.GetComponent<Rigidbody>();
            pinRigidbody.isKinematic = true;
            pinRigidbody.transform.parent = null;

            BoxCollider[] boxColliders = pinTransform.GetComponents<BoxCollider>();

            float duration = 0.1f;
            Sequence pinRemovalSequence = DOTween.Sequence();
            pinRemovalSequence.Append(pinRigidbody.DOMoveY(0.1f, duration).SetRelative().SetEase(Ease.Linear));
            Vector3 randomRotation = new(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            pinRemovalSequence.Join(pinRigidbody.DORotate(randomRotation, duration, RotateMode.LocalAxisAdd));
            pinRemovalSequence.OnComplete(() =>
            {
                float initialSpeed = 7.5f;
                Vector3 throwDirection = releaseTransform.forward;
                Vector3 initialVelocity = throwDirection * initialSpeed;
                float angle = 30.0f;
                float verticalSpeed = initialSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
                Rigidbody rb = heldGrenade.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.linearVelocity = initialVelocity + Vector3.up * verticalSpeed;

                foreach (var collider in boxColliders)
                {
                    collider.isTrigger = true;
                }

                heldGrenade.GetComponent<Rigidbody>().isKinematic = false;
                heldGrenade.transform.parent = null;
                pinRigidbody.isKinematic = false;
                StartCoroutine(ExplodeAfterDelay(heldGrenade));
                heldGrenade = null;

                StartCoroutine(EnableBoxColliderAfterDelay(boxColliders, 0.1f));
            });

            pinRemovalSequence.Play();

            float destroyDuration = 3f;
            Destroy(pinTransform.gameObject, destroyDuration);
        }
    }

    IEnumerator EnableBoxColliderAfterDelay(BoxCollider[] colliders, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
    }

    private IEnumerator ExplodeAfterDelay(GameObject grenadeObject)
    {
        GrenadeSO selectedGrenade = currentGrenadeType == GrenadeType.Regular ? granatPrefab : smokeGranatPrefab;

        yield return new WaitForSeconds(selectedGrenade.ExplosionDelay);
        Vector3 explosionPosition = grenadeObject.transform.position;
        GameObject explosionEffect = Instantiate(selectedGrenade.ExplosionParticleSystem, explosionPosition, Quaternion.identity);
        explosionEffect.transform.SetPositionAndRotation(new Vector3(explosionPosition.x, explosionPosition.y, explosionPosition.z), Quaternion.Euler(-90f, 0f, 0f));
        selectedGrenade.Explode(explosionPosition);
        Destroy(grenadeObject);
        Destroy(explosionEffect, selectedGrenade.DestroyDelay);
    }
}
