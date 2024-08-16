using UnityEngine;

public class ResetGraphicSettings : MonoBehaviour
{
    [SerializeField] private GameObject hintToReset;
    [SerializeField] private Settings qualityLevel;
    [SerializeField] private Settings resolution;
    [SerializeField] private Settings fullscreen;
    [SerializeField] private Settings antiAliasing;  
    [SerializeField] private Settings shadowResolution;

    private bool canReset;

    private void OnEnable()
    {
        hintToReset.SetActive(true);
        canReset = true;
    }

    private void Update()
    {
        if (canReset)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();
            }
        }
    }


    void Reset()
    {
        Debug.Log("Reset Graphic Settings!");
        qualityLevel.ResetQuality();
        resolution.ResetResolutionDropdown();
        fullscreen.ResetFullscreenToggle();
        antiAliasing.ResetAntiAliasingDropdown();
        shadowResolution.ResetShadowResolutionDropdown();
    }

    private void OnDisable()
    {
        canReset = false;
        hintToReset.SetActive(false);
    }
}
