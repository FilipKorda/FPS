using UnityEngine;

public class MonorailTrackEffect : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Kolizja z: " + hit.gameObject.name);

        if (hit.gameObject.CompareTag("Player"))
        {
            
            if (hit.gameObject.TryGetComponent<CharacterController>(out var playerController))
            {
                Debug.Log("Kolizja z graczem");
        
                float x = 15f;

                Vector3 z = Vector3.left * x;

                playerController.Move(z * Time.deltaTime);
            }
        }
    }



}
