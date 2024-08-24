using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Header("Generators Second Segment")]
    [Space(5)]
    public LoadFuelCan[] loadFuelCans;
    public ActiveHangarWhenFuelFull activeHangarWhenFuelFull;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
