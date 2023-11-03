using UnityEngine;

public class PickUpGrenade : MonoBehaviour
{
    [SerializeField] private bool isSmoke;
    [SerializeField] private GrenadeInventory grenadeInventory;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);


            if (isSmoke)
            {
                GrenadeInventory.Instance.AddSmokeGrenade();
            }
            else
            {
                GrenadeInventory.Instance.AddGrenade();
            }
        }

    }
}
