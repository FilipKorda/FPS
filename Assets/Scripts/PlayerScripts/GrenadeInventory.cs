using UnityEngine;

public class GrenadeInventory : MonoBehaviour
{
    public int maxGranatCount = 3;
    public int maxSmokeGranatCount = 3;
    public int currentGranatCount = 0;
    public int currentSmokeGranatCount = 0;

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

}
