using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensivitySettings : MonoBehaviour
{
    [SerializeField] private Slider sensivitySlider;
    private readonly float minSensivity = 0f;
    private readonly float maxSensivity = 10f;
    private float initialSensivity = 3f;
    private float resetInitialSensivity = 3f;
    [SerializeField] private TextMeshProUGUI amountText;
    public float ClampedSensivityValue { get; private set; }

    private void Awake()
    {
        ClampedSensivityValue = initialSensivity;
    }

    private void Start()
    {
        ReadingSensivitySavesValues();
    }

    public void OnSensivitySliderValueChanged(float value)
    {
        ClampedSensivityValue = Mathf.Clamp(value, minSensivity, maxSensivity);
        if (sensivitySlider != null)
        {
            initialSensivity = ClampedSensivityValue;
        }
        UpdateAmountText();

        PlayerPrefs.SetFloat("SensivityValue", value);
    }


    void UpdateAmountText()
    {
        if (amountText != null)
        {
            amountText.text = ClampedSensivityValue.ToString("F0");
        }
    }

    public void ResetSensivity()
    {
        sensivitySlider.value = resetInitialSensivity;

        UpdateAmountText();
    }

    void ReadingSensivitySavesValues()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("SensivityValue", ClampedSensivityValue);
        sensivitySlider.value = savedMasterVolume;

    }
}
