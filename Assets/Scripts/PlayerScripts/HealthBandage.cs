using UnityEngine;

public class HealthBandage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainInventory.Instance.AddHealthBandage();
            Destroy(gameObject);
        }
    }
}
