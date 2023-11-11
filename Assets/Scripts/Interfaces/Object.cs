using System.Collections;
using UnityEngine;

public class Object : MonoBehaviour, IEagleVision
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    public Material highlightMaterial;
    private bool isDetected = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    public void InteractEagleVision()
    {
        if (!isDetected)
        {
            string objectName = gameObject.name;
            Debug.Log(objectName);

            isDetected = true;
            gameObject.layer = LayerMask.NameToLayer("EagleVisionObject");
            StartCoroutine(RestoreMaterial());

            objectRenderer.material = highlightMaterial;
        }
    }

    private IEnumerator RestoreMaterial()
    {
        yield return new WaitForSeconds(4.0f);
        gameObject.layer = LayerMask.NameToLayer("Default");
        objectRenderer.material = originalMaterial;
        isDetected = false;
    }
}