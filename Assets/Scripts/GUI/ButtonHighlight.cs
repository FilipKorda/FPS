using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    private Image buttonImage;
    private TextMeshProUGUI buttonText;
  
    public Color normalTextColor = Color.white;
    public Color highlightedTextColor = Color.yellow;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage.enabled = false;
        buttonText.color = normalTextColor; 
    }

    void Update()
    {
        if (!pauseMenu.isSettingsOpen)
        {
            if (IsMouseOver())
            {
                buttonImage.enabled = true;
                buttonText.color = highlightedTextColor;
            }
            else
            {
                buttonImage.enabled = false;
                buttonText.color = normalTextColor;
            }
        }
    }

    bool IsMouseOver()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 localMousePosition = rt.InverseTransformPoint(Input.mousePosition);
        return rt.rect.Contains(localMousePosition);
    }
}
