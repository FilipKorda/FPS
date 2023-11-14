using UnityEngine;

public class MonorailTrackEffect : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Kolizja z: " + hit.gameObject.name);
        // SprawdŸ, czy obiekt, z którym koliduje gracz, ma tag "Gracz".
        if (hit.gameObject.CompareTag("Player"))
        {
            // Pobierz CharacterController gracza.
            
            // Jeœli CharacterController jest dostêpny, dodaj efekt poœlizgu w lewo.
            if (hit.gameObject.TryGetComponent<CharacterController>(out var playerController))
            {
                Debug.Log("Kolizja z graczem");
                // Ustaw wartoœæ efektu poœlizgu w lewo.
                float poœlizgSi³a = 15f;

                // Ustaw wektor ruchu poœlizgu w lewo (zmiana wspó³rzêdnej x).
                Vector3 poœlizgRuch = Vector3.left * poœlizgSi³a;

                // Dodaj efekt poœlizgu w lewo do ruchu CharacterController.
                playerController.Move(poœlizgRuch * Time.deltaTime);
            }
        }
    }



}
