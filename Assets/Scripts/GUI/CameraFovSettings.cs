using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraFovSettings : MonoBehaviour
{
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TextMeshProUGUI amountText;

    private readonly float minFOV = 50f;
    private readonly float maxFOV = 120f;
    public float initialFOV = 60f;
    private float resetInitialFOV = 60f;

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
            ReadingFOVSavesValues();
        }
        else
        {
            Debug.LogError("Slider nie jest przypisany. Przypisz Slider w inspektorze.");
        }
    }

    public void OnFOVSliderValueChanged(float value)
    {
        ClampedValue = Mathf.Clamp(value, minFOV, maxFOV);
        UpdateAmountText();
        PlayerPrefs.SetFloat("FOVValue", ClampedValue);
    }

    void UpdateAmountText()
    {
        if (amountText != null)
        {
            amountText.text = ClampedValue.ToString("F0");
        }
    }

    public void ResetCameraFov()
    {
        if (fovSlider != null)
        {
            fovSlider.value = resetInitialFOV;
        }
        ClampedValue = Mathf.Clamp(resetInitialFOV, minFOV, maxFOV);
        PlayerPrefs.SetFloat("FOVValue", ClampedValue);
        UpdateAmountText();
    }

    void ReadingFOVSavesValues()
    {
        float saved = PlayerPrefs.GetFloat("FOVValue", ClampedValue);
        if (fovSlider != null)
        {
            fovSlider.value = saved;
        }
        ClampedValue = Mathf.Clamp(saved, minFOV, maxFOV);
    }
}
