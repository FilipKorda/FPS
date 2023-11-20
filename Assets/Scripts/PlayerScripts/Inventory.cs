using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int maxRegularGranat = 3;
    public int currentGranat = 0;

    private int maxSmokeGranat = 3;  
    public int currentSmokeGranat = 0;  

    public event Action<int> GranatChanged;

    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();

                if (instance == null)
                {
                    GameObject singletonObject = new("GrenadeInventory");
                    instance = singletonObject.AddComponent<Inventory>();
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
