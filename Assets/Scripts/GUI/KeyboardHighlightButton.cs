using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardHighlightButton : MonoBehaviour
{
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    [Header("Normal")]
    [SerializeField] private Color normalTextColor = Color.white;
    [SerializeField] private Color normalButtonColor = Color.white;

    [Header("Highligt")]
    [SerializeField] private Color highlightedTextColor = Color.yellow;
    [SerializeField] private Color highlightedButtonColor = Color.yellow;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = normalTextColor;
        buttonImage.color = normalButtonColor;
    }


    private void Update()
    {
        if (IsMouseOver())
        {
            buttonText.color = highlightedTextColor;
            buttonImage.color = highlightedButtonColor;
        }
        else
        {
            buttonText.color = normalTextColor;
            buttonImage.color = normalButtonColor;
        }
    }



    bool IsMouseOver()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 localMousePosition = rt.InverseTransformPoint(Input.mousePosition);
        return rt.rect.Contains(localMousePosition);
    }

}
