using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainInventory : MonoBehaviour
{
    public static MainInventory Instance { get; private set; }

    [SerializeField] private GameObject ButtonTabPanel;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quest;

    [SerializeField] private GameObject buttonInventory;
    [SerializeField] private GameObject buttonQuest;

    [SerializeField] private Color highlightButton;

    public bool isPanelActive = false;
    [SerializeField] private GameObject usebleItems;
    [SerializeField] private GameObject items;

    [Header("======= Oxygen Container =======")]
    [Space(5)]
    public int currentOxygenContainer = 0;
    private int maxOxygenContainer = 3;
    [SerializeField] private GameObject oxygenContainerPrefab;
    [SerializeField] private Sprite oxygenIcon;
    [SerializeField] private string oxygenContainerName = "Oxygen";
    private TextMeshProUGUI oxygenItemAmountText;
    private TextMeshProUGUI oxygenItemNameText;
    private Image oxygenImage;
    public GameObject instantiatedOxygenContainerPrefab;
    public bool isOxygenCreateAnPrefab;

    [Header("======= Health Bandage =======")]
    [Space(5)]
    public int currentHealthBandage = 0;
    private int maxHealthBandage = 3;
    [SerializeField] private GameObject healthBandagePrefab;
    [SerializeField] private Sprite healthBandageIcon;
    [SerializeField] private string healthBandageName = "Bandage";
    private TextMeshProUGUI healthBandageItemAmountText;
    private TextMeshProUGUI healthBandageNameText;
    private Image healthBandageImage;
    public GameObject instantiatedHealthBandagePrefab;
    public bool isHealthBandageCreateAnPrefab;

    [Header("======= Cards =======")]
    [Space(5)]
    [Header("==== Red ====")]
    [Space(5)]
    [SerializeField] private Color redCardColor;
    [SerializeField] private GameObject redCardPrefab;
    [SerializeField] private Sprite redCardIcon;
    private TextMeshProUGUI redCardNameText;
    private Image redCardMainImage;
    private Image redCardImage;
    public GameObject instantiatedRedCardPrefab;
    public int redCard = 0;
    public string redCardName = "Red Card";
    [Header("==== Green ====")]
    [Space(5)]
    [SerializeField] private Color greenCardColor;
    [SerializeField] private GameObject greenCardPrefab;
    [SerializeField] private Sprite greenCardIcon;
    private TextMeshProUGUI greenCardNameText;
    private Image greenCardMainImage;
    private Image greenCardImage;
    public GameObject instantiatedGreenCardPrefab;
    public int greenCard = 0;
    public string greenCardName = "Green Card";
    [Header("==== Blue ====")]
    [Space(5)]
    [SerializeField] private Color blueCardColor;
    [SerializeField] private GameObject blueCardPrefab;
    [SerializeField] private Sprite blueCardIcon;
    private TextMeshProUGUI blueCardNameText;
    private Image blueCardMainImage;
    private Image blueCardImage;
    public GameObject instantiatedBlueCardPrefab;
    public int blueCard = 0;
    public string blueCardName = "Blue Card";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ButtonTabPanel.SetActive(false);
        inventory.SetActive(true);
      
        buttonInventory.GetComponentInChildren<TextMeshProUGUI>().color = highlightButton;
    }

    private void Update()
    {
        InputHandler();

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isPanelActive = !isPanelActive;
            ButtonTabPanel.SetActive(isPanelActive);

            if (isPanelActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

    }

    void InputHandler()
    {
        if (isPanelActive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                buttonInventory.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                buttonQuest.GetComponentInChildren<TextMeshProUGUI>().color = highlightButton;

                inventory.SetActive(false);
                quest.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                buttonInventory.GetComponentInChildren<TextMeshProUGUI>().color = highlightButton;
                buttonQuest.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

                inventory.SetActive(true);
                quest.SetActive(false);
            }
        }
    }

    private void OxygenUseblePrefabItemsToGUI()
    {
        isOxygenCreateAnPrefab = true;
        instantiatedOxygenContainerPrefab = Instantiate(oxygenContainerPrefab, usebleItems.transform);
        oxygenItemAmountText = instantiatedOxygenContainerPrefab.GetComponentInChildren<TextMeshProUGUI>();
        oxygenItemNameText = instantiatedOxygenContainerPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        oxygenImage = instantiatedOxygenContainerPrefab.transform.GetChild(2).GetComponent<Image>();
    }

    private void UpdateAmountOfOxygenNumber()
    {
        if (oxygenItemAmountText != null)
        {
            oxygenItemAmountText.text = currentOxygenContainer.ToString();
        }
    }

    private void UpdateOxygenName()
    {
        if (oxygenItemNameText != null)
        {
            oxygenItemNameText.text = oxygenContainerName;
        }
    }

    private void UpdateOxygenImage()
    {
        if (oxygenImage != null)
        {
            oxygenImage.sprite = oxygenIcon;
        }
    }

    public void AddOxygenContainer()
    {
        if (!isOxygenCreateAnPrefab)
        {
            OxygenUseblePrefabItemsToGUI();
            UpdateOxygenName();
            UpdateOxygenImage();
        }

        currentOxygenContainer++;
        UpdateAmountOfOxygenNumber();
    }

    public void RemoveOxygenContainer()
    {
        currentOxygenContainer--;
        UpdateAmountOfOxygenNumber();
    }

    private void HealthBandageUseblePrefabItemsToGUI()
    {
        isHealthBandageCreateAnPrefab = true;
        instantiatedHealthBandagePrefab = Instantiate(healthBandagePrefab, usebleItems.transform);
        healthBandageItemAmountText = instantiatedHealthBandagePrefab.GetComponentInChildren<TextMeshProUGUI>();
        healthBandageNameText = instantiatedHealthBandagePrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        healthBandageImage = instantiatedHealthBandagePrefab.transform.GetChild(2).GetComponent<Image>();
    }

    private void UpdateAmountOfHealthBandageNumber()
    {
        if (healthBandageItemAmountText != null)
        {
            healthBandageItemAmountText.text = currentHealthBandage.ToString();
        }
    }

    private void UpdateealthBandageName()
    {
        if (healthBandageNameText != null)
        {
            healthBandageNameText.text = healthBandageName;
        }
    }

    private void UpdateHealthBandageImage()
    {
        if (healthBandageImage != null)
        {
            healthBandageImage.sprite = healthBandageIcon;
        }
    }

    public void AddHealthBandage()
    {
        if (!isHealthBandageCreateAnPrefab)
        {
            HealthBandageUseblePrefabItemsToGUI();
            UpdateealthBandageName();
            UpdateHealthBandageImage();
        }

        currentHealthBandage++;
        UpdateAmountOfHealthBandageNumber();
    }

    public void RemoveHealthBandage()
    {
        currentHealthBandage--;
        UpdateAmountOfHealthBandageNumber();
    }

    public void AddCard(bool isRedCard, bool isGreenCard, bool isBlueCard)
    {      
        if (isRedCard)
        {
            RedCardQuestPrefabItemsGUI();
            redCard++;
        }
        if (isGreenCard)
        {
            GreenCardQuestPrefabItemsGUI();
            greenCard++;
        }
        if (isBlueCard)
        {
            BlueCardQuestPrefabItemsGUI();
            blueCard++;
        }
    }

    public void RemoveCard(bool isRedCard, bool isGreenCard, bool isBlueCard)
    {
        if (isRedCard)
        {
            redCard--;
            Destroy(instantiatedRedCardPrefab);
        }
        if (isGreenCard)
        {
            greenCard--;
            Destroy(instantiatedGreenCardPrefab);
        }
        if (isBlueCard)
        {
            blueCard--;
            Destroy(instantiatedBlueCardPrefab);
        }
    }

    private void RedCardQuestPrefabItemsGUI()
    {
        instantiatedRedCardPrefab = Instantiate(redCardPrefab, items.transform);
        redCardMainImage = instantiatedRedCardPrefab.GetComponent<Image>();
        redCardNameText = instantiatedRedCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        redCardImage = instantiatedRedCardPrefab.transform.GetChild(1).GetComponent<Image>();

        redCardMainImage.color = redCardColor;
        redCardImage.sprite = redCardIcon;
        redCardNameText.text = redCardName;
    }
    private void GreenCardQuestPrefabItemsGUI()
    {
        instantiatedGreenCardPrefab = Instantiate(greenCardPrefab, items.transform);
        greenCardMainImage = instantiatedGreenCardPrefab.GetComponent<Image>();
        greenCardNameText = instantiatedGreenCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        greenCardImage = instantiatedGreenCardPrefab.transform.GetChild(1).GetComponent<Image>();

        greenCardMainImage.color = greenCardColor;
        greenCardImage.sprite = greenCardIcon;
        greenCardNameText.text = greenCardName;
    }
    private void BlueCardQuestPrefabItemsGUI()
    {
        instantiatedBlueCardPrefab = Instantiate(blueCardPrefab, items.transform);
        blueCardMainImage = instantiatedBlueCardPrefab.GetComponent<Image>();
        blueCardNameText = instantiatedBlueCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        blueCardImage = instantiatedBlueCardPrefab.transform.GetChild(1).GetComponent<Image>();

        blueCardMainImage.color = blueCardColor;
        blueCardImage.sprite = blueCardIcon;
        blueCardNameText.text = blueCardName;
    }
}
