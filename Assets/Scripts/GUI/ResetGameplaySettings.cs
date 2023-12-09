using UnityEngine;

public class ResetGameplaySettings : MonoBehaviour
{
    [SerializeField] private CameraFovSettings cameraFovSettings;
    [SerializeField] private SensivitySettings sensivitySettings;
    [SerializeField] private GameObject hintToReset;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reset();
            }
        }

    }

    void Reset()
    {
        Debug.Log("siema!");
        sensivitySettings.ResetSensivity();
    }

    private void OnDisable()
    {
        canReset = false;
        hintToReset.SetActive(false);
    }
}
