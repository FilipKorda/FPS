using UnityEngine;

public class ClipPrevention : MonoBehaviour
{
    public GameObject clipProjector;
    public float checkDistance;
    public Vector3 newDirection;
    public LayerMask layerToClipGun; //Default

    float lerpPos;
    RaycastHit hit;


    private void Update()
    {
        if (Physics.Raycast(clipProjector.transform.position, clipProjector.transform.forward, out hit, checkDistance, layerToClipGun))
        {
            lerpPos = 1 - (hit.distance / checkDistance);
        }
        else
        {
            lerpPos = 0;
        }

        Mathf.Clamp01(lerpPos);

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(newDirection), lerpPos);
    }
}
