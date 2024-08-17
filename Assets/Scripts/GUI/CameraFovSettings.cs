using FPS.Guns.Demo;
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
        if (PlayerGunSelector.Instance != null)
        {
            PlayerGunSelector.Instance.Camera.fieldOfView = ClampedValue;
        }
        UpdateAmountText();


        PlayerPrefs.SetFloat("FOVValue", value);
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
        fovSlider.value = resetInitialFOV;

        UpdateAmountText();
    }

    void ReadingFOVSavesValues()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("FOVValue", ClampedValue);
        fovSlider.value = savedMasterVolume;

    }
}
