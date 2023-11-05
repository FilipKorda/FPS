using System.Collections;
using UnityEngine;

public class Object : MonoBehaviour, IInteractable
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

    public void Interact()
    {
        if (!isDetected)
        {
            string objectName = gameObject.name;
            Debug.Log(objectName);

            isDetected = true;

            StartCoroutine(RestoreMaterial());

            objectRenderer.material = highlightMaterial;
        }
    }

    private IEnumerator RestoreMaterial()
    {
        yield return new WaitForSeconds(4.0f);
        objectRenderer.material = originalMaterial;
        isDetected = false;
    }
}