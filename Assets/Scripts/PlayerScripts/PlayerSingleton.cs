using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    public MarsHurricaneController marsHurricaneController;

    public Transform oxygenPipeLink;

    public bool canShoot = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        marsHurricaneController.GetComponent<MarsHurricaneController>();
    }
}
