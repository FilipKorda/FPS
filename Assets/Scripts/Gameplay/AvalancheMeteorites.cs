using UnityEngine;

public class AvalancheMeteorites : MonoBehaviour
{
    public GameObject meteorit;
    public Transform pointA;
    public Transform pointB;
    public float speed = 1.0f;

    public bool shouldMove = false;
    public bool playerColideWithCollider = false;
    private float lerpTime = 0;

/*    private void OnTriggerEnter(Collider other)
    {
        if (!playerColideWithCollider && other.CompareTag("Player"))
        {
            shouldMove = true;
            playerColideWithCollider = true;
            CameraShake.Instance.AlarmPlayer();
        }
    }*/

    void Update()
    {
        if (shouldMove)
        {
            lerpTime += Time.deltaTime * speed;
            meteorit.transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpTime);

            if (lerpTime >= 1)
            {
                shouldMove = false;
                lerpTime = 0;               
            }
        }
    }
}
