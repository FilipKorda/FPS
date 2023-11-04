using System.Collections;
using UnityEngine;

public class Object : MonoBehaviour, IInteractable
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    public Material highlightMaterial; 

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    public void Interact()
    {
        string objectName = gameObject.name;
        Debug.Log(objectName);

        objectRenderer.material = highlightMaterial;

        StartCoroutine(RestoreMaterial());
    }

    private IEnumerator RestoreMaterial()
    {
        yield return new WaitForSeconds(2.0f);

        objectRenderer.material = originalMaterial;
    }
}
