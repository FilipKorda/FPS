using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraFovSettings : MonoBehaviour
{
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI amountText;

    private readonly float minFOV = 50f;
    private readonly float maxFOV = 120f;
    public float initialFOV = 60f;

    public float ClampedValue { get; set; }

    private void Awake()
    {
        ClampedValue = initialFOV;
        UpdateAmountText();
    }

    void Start()
    {
        if (fovSlider != null)
        {
            fovSlider.minValue = minFOV;
            fovSlider.maxValue = maxFOV;
            fovSlider.value = initialFOV;
            fovSlider.onValueChanged.AddListener(OnFOVSliderValueChanged);
        }
        else
        {
            Debug.LogError("Slider nie jest przypisany. Przypisz Slider w inspektorze.");
        }
    }

    public void OnFOVSliderValueChanged(float value)
    {
        ClampedValue = Mathf.Clamp(value, minFOV, maxFOV);
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = ClampedValue;
        }
        UpdateAmountText();
    }

    void UpdateAmountText()
    {
        if (amountText != null)
        {
            amountText.text = ClampedValue.ToString("F0");
        }
    }
}
