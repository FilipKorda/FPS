using System;
using UnityEngine;

public class GrenadeInventory : MonoBehaviour
{
    private int maxRegularGranat = 3;
    private int maxSmokeGranat = 3;
    public int currentGranat = 0;
    public int currentSmokeGranat = 0;

    public event Action<int> GranatChanged;

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
        GranatChanged?.Invoke(currentGranat);
    }

    public void RemoveGrenade()
    {
        currentGranat--;
        GranatChanged?.Invoke(currentGranat);
    }

    public void AddSmokeGrenade()
    {
        currentSmokeGranat++;
        GranatChanged?.Invoke(currentSmokeGranat);
    }

    public void RemoveSmokeGrenade()
    {
        currentSmokeGranat--;
        GranatChanged?.Invoke(currentSmokeGranat);
    }
}
