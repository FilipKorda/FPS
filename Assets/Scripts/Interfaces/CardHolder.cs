using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class CardHolder : MonoBehaviour, ICardHolder
{
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;
    private string HintString => "Press [E] to place the card";

    private Color originalColor;
    private Renderer originalColorRenderer;

    public bool needRedCard, needGreenCard, needBlueCard;

    void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void UseCard()
    {
        if (needRedCard)
        {
            if (MainInventory.Instance.redCard > 0)
            {
                MainInventory.Instance.RemoveCard(true, false, false);
            }
            else
            {
                NotificationSystem.Instance.ShowNotification("You dont have Red Card!", 2f);
            }
        }
        else if (needGreenCard)
        {
            if (MainInventory.Instance.greenCard > 0)
            {
                MainInventory.Instance.RemoveCard(false, true, false);
            }
            else
            {
                NotificationSystem.Instance.ShowNotification("You dont have Green Card!", 2f);
            }
        }
        else if (needBlueCard)
        {
            if (MainInventory.Instance.blueCard > 0 && needBlueCard)
            {
                MainInventory.Instance.RemoveCard(false, false, true);
            }
            else
            {
                NotificationSystem.Instance.ShowNotification("You dont have Blue Card!", 2f);
            }
        }
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);
        hint_Text.text = HintString;
        originalColorRenderer.material.color = Color.yellow;
    }

    public void DeactiveHint()
    {
        hint_Panel.SetActive(false);
        hint_Text.text = "";
        originalColorRenderer.material.color = originalColor;
    }
}
