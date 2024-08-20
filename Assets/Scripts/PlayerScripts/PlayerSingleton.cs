using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    public bool canShoot = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
