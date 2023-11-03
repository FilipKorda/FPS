using System;
using UnityEngine;

public class GrenadeInventory : MonoBehaviour
{
    public Action<int> GrenadeAmmoChanged;
    public Action<int> SmokeGrenadeAmmoChanged;

    private int maxRegularGranat = 3;
    private int maxSmokeGranat = 3;
    public int currentGranat = 0;
    public int currentSmokeGranat = 0;


    private static GrenadeInventory instance;
    public static GrenadeInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GrenadeInventory>();

                if (instance == null)
                {
                    GameObject singletonObject = new("GrenadeInventory");
                    instance = singletonObject.AddComponent<GrenadeInventory>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    public void AddGrenade()
    {
        currentGranat++;
        GrenadeAmmoChanged?.Invoke(currentGranat);
    }

    public void RemoveGrenade()
    {
        currentGranat--;
        GrenadeAmmoChanged?.Invoke(currentGranat);
    }

    public void AddSmokeGrenade()
    {
        currentSmokeGranat++;
        SmokeGrenadeAmmoChanged?.Invoke(currentSmokeGranat);
    }

    public void RemoveSmokeGrenade()
    {
        currentSmokeGranat--;
        SmokeGrenadeAmmoChanged?.Invoke(currentSmokeGranat);
    }

}
