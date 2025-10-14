using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CardHolder : MonoBehaviour, ICardHolder
{
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;
    [SerializeField] private float timeUntilGateOpen = 1f;
    [SerializeField] private float heightToGateOpen = 2.75f;

    public LocalizedString localizeStringEventRedCard;
    public LocalizedString localizeStringEventGreenCard;
    public LocalizedString localizeStringEventBlueCard;

    public LocalizedString localizeStringEventPressERedCard;
    public LocalizedString localizeStringEventPressEGreenCard;
    public LocalizedString localizeStringEventPressEBlueCard;

    private Color originalColor;
    private Renderer originalColorRenderer;

    public bool needRedCard, needGreenCard, needBlueCard;

    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject cardObject;
    [SerializeField] private GameObject cardHolder;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
        cardObject.SetActive(false);
        cardHolder.SetActive(true);
    }

    private void OnLocaleChanged(Locale _)
    {
        if (hint_Panel != null && hint_Panel.activeSelf)
        {
            hint_Text.text = BuildHintLocalized();
        }
    }

    private string BuildHintLocalized()
    {
        LocalizedString ls;
        string cardName;

        if (needRedCard)
        {
            ls = localizeStringEventPressERedCard;
            cardName = MainInventory.Instance.redCardName;
        }
        else if (needGreenCard)
        {
            ls = localizeStringEventPressEGreenCard;
            cardName = MainInventory.Instance.greenCardName;
        }
        else
        {
            ls = localizeStringEventPressEBlueCard;
            cardName = MainInventory.Instance.blueCardName;
        }

        string localized = null;
        if (ls != null)
        {
            try
            {
                localized = ls.GetLocalizedString(cardName);
            }
            catch
            {
                
            }
        }

        if (string.IsNullOrEmpty(localized))
        {
            return $"Press [E] to place the {cardName}!";
        }

        return localized;
    }

    public void UseCard()
    {
        if (needRedCard)
        {
            if (MainInventory.Instance.redCard > 0)
            {
                MainInventory.Instance.RemoveCard(true, false, false);
                OpenGate();
            }
            else
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventRedCard, "You dont have Red Card!", 2f);
            }
        }
        else if (needGreenCard)
        {
            if (MainInventory.Instance.greenCard > 0)
            {
                MainInventory.Instance.RemoveCard(false, true, false);
                OpenGate();
            }
            else
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventGreenCard, "You dont have Green Card!", 2f);
            }
        }
        else if (needBlueCard)
        {
            if (MainInventory.Instance.blueCard > 0 && needBlueCard)
            {
                MainInventory.Instance.RemoveCard(false, false, true);
                OpenGate();
            }
            else
            {
                NotificationSystem.Instance.ShowNotification(localizeStringEventBlueCard, "You dont have Blue Card!", 2f);
            }
        }
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);
        hint_Text.text = BuildHintLocalized();
        originalColorRenderer.material.color = Color.yellow;
    }

    public void DeactiveHint()
    {
        hint_Panel.SetActive(false);
        hint_Text.text = "";
        originalColorRenderer.material.color = originalColor;
    }

    public void OpenGate()
    {
        cardObject.SetActive(true);
        cardHolder.SetActive(false);
        gate.transform.DOMoveY(gate.transform.position.y + heightToGateOpen, timeUntilGateOpen).SetEase(Ease.OutQuad);
    }
}
