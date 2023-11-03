using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Listy Of Granades")]
    public List<GameObject> grenadePrefabs;
    public List<GameObject> smokeGrenadePrefabs;

    public List<GameObject> activeGrenadeList;
    public int activeListIndex = 0;
    private readonly int currentGrenadeIndex = 0;
    private bool isHoldingGrenade = false;
    public GrenadeSO grenade;
    public GrenadeSO smokeGrenade;

    public Transform throwPoint;
    public float throwForce = 15f;

    //Events
    public delegate void GrenadeGUIChangedHandler(List<GameObject> activeGrenadeList);
    public event GrenadeGUIChangedHandler GrenadeChangedOnGUI;
    public delegate void GrenadeSelectionChanged(int activeListIndex);
    public event GrenadeSelectionChanged OnGrenadeSelectionChanged;

    private bool canThrowGrenade = true;
    private readonly float grenadeCooldown = 1f;

    private void Start()
    {
        SetActiveGrenadeList(grenadePrefabs);
        NotifyGrenadeGUIChanged();
    }

    private void Update()
    {
        SwitchListOfGrenade();
        SetupGrenade();
    }

    private void SwitchListOfGrenade()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeListIndex = (activeListIndex + 1) % 2;

            if (activeListIndex == 0)
            {
                Debug.Log("grenade");
                SetActiveGrenadeList(grenadePrefabs);
            }
            else if (activeListIndex == 1)
            {
                Debug.Log("smoke Granade");
                SetActiveGrenadeList(smokeGrenadePrefabs);
            }

            NotifyGrenadeGUIChanged();
            NotifyGrenadeGUISwamp();
        }
    }

    private void SetupGrenade()
    {
        if (isHoldingGrenade)
        {
            if (Input.GetKeyUp(KeyCode.G) && canThrowGrenade)
            {
                ThrowGrenade();
                canThrowGrenade = false; // Wy³¹cz mo¿liwoœæ rzucania granatem
                StartCoroutine(ResetGrenadeCooldown());
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            CreateGrenade();
        }
    }

    private void CreateGrenade()
    {
        if (canThrowGrenade)
        {
            GrenadeSO activeGrenadeType = activeListIndex == 0 ? grenade : smokeGrenade;

            if (activeGrenadeList.Count > 0)
            {
                isHoldingGrenade = true;
                GameObject newGrenade = Instantiate(activeGrenadeList[currentGrenadeIndex], throwPoint.position, throwPoint.rotation);
                activeGrenadeList.Add(newGrenade);
                activeGrenadeList.RemoveAt(0);
                Rigidbody currentGrenadeRigidbody = newGrenade.GetComponent<Rigidbody>();
                newGrenade.transform.parent = throwPoint;
                currentGrenadeRigidbody.isKinematic = true;
            }
            else
            {
                Debug.Log($"You don't have a {activeGrenadeType.Name}");
            }
        }

    }

    private void ThrowGrenade()
    {
        if (canThrowGrenade)
        {
            if (activeGrenadeList.Count > 0)
            {
                GameObject currentGrenade = activeGrenadeList[^1];
                activeGrenadeList.RemoveAt(activeGrenadeList.Count - 1);
                currentGrenade.transform.parent = null;

                Rigidbody currentGrenadeRigidbody = currentGrenade.GetComponent<Rigidbody>();
                currentGrenadeRigidbody.isKinematic = false;
                currentGrenadeRigidbody.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange);
                isHoldingGrenade = false;


                StartCoroutine(ExplodeAfterDelay(currentGrenade));
                NotifyGrenadeGUIChanged();
            }
        }
    }

    private IEnumerator ExplodeAfterDelay(GameObject grenadeObject)
    {
        GrenadeSO activeGrenadeType = activeListIndex == 0 ? grenade : smokeGrenade;

        yield return new WaitForSeconds(activeGrenadeType.ExplosionDelay);
        Vector3 explosionPosition = grenadeObject.transform.position;
        GameObject explosionEffect = Instantiate(activeGrenadeType.ExplosionParticleSystem, explosionPosition, Quaternion.identity);
        explosionEffect.transform.SetPositionAndRotation(new Vector3(explosionPosition.x, explosionPosition.y, explosionPosition.z), Quaternion.Euler(-90f, 0f, 0f));
        activeGrenadeType.Explode(explosionPosition);
        Destroy(grenadeObject);
        Destroy(explosionEffect, activeGrenadeType.DestroyDelay);
    }

    private IEnumerator ResetGrenadeCooldown()
    {
        yield return new WaitForSeconds(grenadeCooldown);
        canThrowGrenade = true;
    }

    private void SetActiveGrenadeList(List<GameObject> list)
    {
        activeGrenadeList = list;
    }

    public void NotifyGrenadeGUIChanged()
    {
        GrenadeChangedOnGUI?.Invoke(activeGrenadeList);
    }

    public void NotifyGrenadeGUISwamp()
    {
        OnGrenadeSelectionChanged?.Invoke(activeListIndex);
    }

}
