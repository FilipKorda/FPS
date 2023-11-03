using System.Collections;
using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
    public GrenadeSO granatPrefab;
    public GrenadeSO smokeGranatPrefab;
    public Transform releaseTransform;

    private GameObject heldGrenade;
    private GrenadeType currentGrenadeType;
    private enum GrenadeType
    {
        Regular,
        Smoke
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
                Debug.Log("Smoke");
            }
            else
            {
                currentGrenadeType = GrenadeType.Regular;
                Debug.Log("Regular");
            }
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
        int availableGrenades = currentGrenadeType == GrenadeType.Regular ? GrenadeInventory.Instance.currentGranatCount : GrenadeInventory.Instance.currentSmokeGranatCount;
        if (availableGrenades > 0)
        {
            GrenadeSO selectedGrenade = currentGrenadeType == GrenadeType.Regular ? granatPrefab : smokeGranatPrefab;
            heldGrenade = Instantiate(selectedGrenade.ModelPrefab, releaseTransform.position, releaseTransform.rotation);
            heldGrenade.GetComponent<Rigidbody>().isKinematic = true;
            heldGrenade.transform.parent = releaseTransform;

            if (currentGrenadeType == GrenadeType.Regular)
            {
                GrenadeInventory.Instance.currentGranatCount--;
            }
            else
            {
                GrenadeInventory.Instance.currentSmokeGranatCount--;
            }
        }
    }

    void ThrowGrenade()
    {
        if (heldGrenade != null)
        {           
            heldGrenade.GetComponent<Rigidbody>().isKinematic = false;
            Rigidbody rb = heldGrenade.GetComponent<Rigidbody>();
            rb.AddForce(releaseTransform.forward * 10.0f, ForceMode.Impulse);
            heldGrenade.transform.parent = null;
           
            StartCoroutine(ExplodeAfterDelay(heldGrenade));
            heldGrenade = null;
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
