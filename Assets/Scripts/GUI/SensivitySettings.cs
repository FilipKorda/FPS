using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensivitySettings : MonoBehaviour
{
    [SerializeField] private Slider sensivitySlider;
    private readonly float minSensivity = 0f;
    private readonly float maxSensivity = 10f;
    private float initialSensivity = 3f;
    [SerializeField] private TextMeshProUGUI amountText;
    public float ClampedSensivityValue { get; private set; }

    private void Awake()
    {
        ClampedSensivityValue = initialSensivity;
    }

    public void OnSensivitySliderValueChanged(float value)
    {
        ClampedSensivityValue = Mathf.Clamp(value, minSensivity, maxSensivity);
        if (sensivitySlider != null)
        {
            initialSensivity = ClampedSensivityValue;
        }

        UpdateAmountText();
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
        ClampedSensivityValue = initialSensivity;
        if (sensivitySlider != null)
        {
            ClampedSensivityValue = initialSensivity;
        }

        UpdateAmountText();
    }
}
