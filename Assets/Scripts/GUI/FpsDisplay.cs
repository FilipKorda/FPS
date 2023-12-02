using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private TMP_Dropdown fpsDropdown;
    private float deltaTime = 0f;
    private readonly float updateRate = 0.1f;
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        if (fpsToggle != null)
        {
            fpsToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        else
        {
            Debug.LogError("Toggle nie jest przypisany. Przypisz Toggle w inspektorze.");
        }

        if (fpsDropdown != null)
        {
            fpsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        else
        {
            Debug.LogError("Dropdown nie jest przypisany. Przypisz Dropdown w inspektorze.");
        }
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        string fpsString = Mathf.Ceil(fps).ToString();

        timeSinceLastUpdate += Time.unscaledDeltaTime;
        if (timeSinceLastUpdate > updateRate)
        {
            if (fpsToggle != null && fpsToggle.isOn)
            {
                if (fpsText != null)
                {
                    fpsText.text = fpsString;
                }
            }
            else
            {
                if (fpsText != null)
                {
                    fpsText.text = "";
                }
            }

            timeSinceLastUpdate = 0.0f;
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            if (fpsText != null)
            {
                fpsText.enabled = true;
            }
        }
        else
        {
            if (fpsText != null)
            {
                fpsText.enabled = false;
            }
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        int[] fpsOptions = { 30, 60, 120, 144 }; 
        if (index >= 0 && index < fpsOptions.Length)
        {
            SetTargetFPS(fpsOptions[index]);
        }
    }

    void SetTargetFPS(int targetFPS)
    {
        Application.targetFrameRate = targetFPS;
    }


}
