using System.Collections;
using UnityEngine;

public class Object : MonoBehaviour, IEagleVision
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    [SerializeField] public Material highlightMaterial;
    private bool isDetected = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    public void InteractEagleVision()
    {
        if (isDetected || objectRenderer == null || highlightMaterial == null)
            return;

        Debug.Log(gameObject.name);

        isDetected = true;
   
        objectRenderer.material = highlightMaterial;
        StartCoroutine(RestoreMaterial());
    }

    private IEnumerator RestoreMaterial()
    {
        yield return new WaitForSeconds(4.0f);

        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }

        isDetected = false;
    }
}