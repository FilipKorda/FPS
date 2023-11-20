using UnityEngine;

public class OxygenContainer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            MainInventory.Instance.AddOxygenContainer();
            Destroy(gameObject);
        }
    }
}
