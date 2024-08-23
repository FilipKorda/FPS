using UnityEngine;

public class ActiveHangarWhenFuelFull : MonoBehaviour
{
    [SerializeField] private LoadFuelCan loadFuelCan;
    [SerializeField] private LoadFuelCan loadFuelCan1;
    [SerializeField] private LoadFuelCan loadFuelCan2;
    [SerializeField] private LoadFuelCan loadFuelCan3;


    private void ActiveHangar()
    {
        if(loadFuelCan.isFuelFull && loadFuelCan1.isFuelFull & loadFuelCan2.isFuelFull && loadFuelCan3.isFuelFull)
        {
            Debug.Log("Open hangar");
        }
    }
}
