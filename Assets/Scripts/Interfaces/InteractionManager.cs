using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Camera mainCamera;
    public float maxRaycastDistance = 10f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRaycastDistance))
            {
                if (hit.collider.TryGetComponent<IPickupable>(out var pickupableObject))
                {
                    pickupableObject.Pickup();
                }
            }
        }
    }
}
