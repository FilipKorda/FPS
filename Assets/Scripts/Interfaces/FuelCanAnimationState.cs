using UnityEngine;

public class FuelCanAnimationState : MonoBehaviour
{
    [SerializeField] private LoadFuelCan loadFuelCan;
    public void OnFuelCanAnimationEnd()
    {
        loadFuelCan.fuelCanAnimation.SetActive(false);
        loadFuelCan.fuelCanEndHolder.SetActive(true);
    }
}
