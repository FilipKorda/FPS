using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsTabHighlight : MonoBehaviour
{
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    public Color normalTextColor = Color.white;
    public Color highlightedTextColor = Color.yellow;

    public bool isSelected = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!isSelected)
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

    public void HighlightButtonOnClick()
    {
        SettingsTabHighlight[] allTabs = transform.parent.GetComponentsInChildren<SettingsTabHighlight>();
        foreach (SettingsTabHighlight tab in allTabs)
        {
            if (tab != this)
            {
                tab.DisableHighlight();
            }
        }

        isSelected = true;
        buttonImage.enabled = true;
        buttonText.color = highlightedTextColor;
    }

    public void DisableHighlight()
    {
        isSelected = false;
        buttonImage.enabled = false;
        buttonText.color = normalTextColor;
    }

    public void UpdateHighlight()
    {
        if (isSelected)
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
