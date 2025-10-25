using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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
    public LocalizedString localizeStringRedCardName;
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
    public LocalizedString localizeStringGreenCardName;
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
    public LocalizedString localizeStringBlueCardName;


    [Header("========= Fuel Can ==========")]
    [Space(5)]
    [SerializeField] private Color fuelCanColor;
    [SerializeField] private GameObject fuelCanPrefab;
    [SerializeField] private Sprite fuelCanIcon;
    private Image fuelCanMainImage;
    private TextMeshProUGUI fuelCanNameText;
    private Image fuelCanImage;
    private GameObject instantiatedFuelCanPrefab;
    public int currentfuelCans;
    public int fuelCanForQuest;
    public LocalizedString localizeStringFuelCanName;

    [Header("========= Barrel ==========")]
    [SerializeField] private Color barrelColor;
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private Sprite barrelIcon;
    private Image barrelMainImage;
    private TextMeshProUGUI barrelNameText;
    private Image barrelImage;
    private GameObject instantiatedBarrelPrefab;
    public int currentBarrels;
    public int barrelForQuest;
    public LocalizedString localizeStringBarrelName;


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

        if (Input.GetKeyDown(KeyCode.I))
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

        StatisticsCollector.AddItemPickedUp();
    }

    public void RemoveOxygenContainer()
    {
        currentOxygenContainer--;
        UpdateAmountOfOxygenNumber();
        StatisticsCollector.AddItemUsed();
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

        StatisticsCollector.AddItemPickedUp();
    }

    public void RemoveHealthBandage()
    {
        currentHealthBandage--;
        UpdateAmountOfHealthBandageNumber();
        StatisticsCollector.AddItemUsed();
    }


    public void AddFuelCan()
    {
        FuelCanQuestPrefabItemsGUI();
        currentfuelCans++;
        fuelCanForQuest++;
        StatisticsCollector.AddItemPickedUp();
    }
    public void RemoveFuelCan()
    {
        Destroy(instantiatedFuelCanPrefab);
        currentfuelCans--;
        StatisticsCollector.AddItemUsed();
    }

    public void AddBarrel()
    {
        BarrelQuestPrefabItemsGUI();
        currentBarrels++;
        barrelForQuest++;
        StatisticsCollector.AddItemPickedUp();
    }

    public void RemoveBarrel()
    {
        Destroy(instantiatedBarrelPrefab);
        currentBarrels--;
        StatisticsCollector.AddItemUsed();
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

        StatisticsCollector.AddItemPickedUp();
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

        StatisticsCollector.AddItemUsed();
    }

    private void RedCardQuestPrefabItemsGUI()
    {
        instantiatedRedCardPrefab = Instantiate(redCardPrefab, items.transform);
        redCardMainImage = instantiatedRedCardPrefab.GetComponent<Image>();
        redCardNameText = instantiatedRedCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        redCardImage = instantiatedRedCardPrefab.transform.GetChild(1).GetComponent<Image>();

        redCardMainImage.color = redCardColor;
        redCardImage.sprite = redCardIcon;
        redCardNameText.text = localizeStringRedCardName.GetLocalizedString();
    }
    private void GreenCardQuestPrefabItemsGUI()
    {
        instantiatedGreenCardPrefab = Instantiate(greenCardPrefab, items.transform);
        greenCardMainImage = instantiatedGreenCardPrefab.GetComponent<Image>();
        greenCardNameText = instantiatedGreenCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        greenCardImage = instantiatedGreenCardPrefab.transform.GetChild(1).GetComponent<Image>();

        greenCardMainImage.color = greenCardColor;
        greenCardImage.sprite = greenCardIcon;
        greenCardNameText.text = localizeStringGreenCardName.GetLocalizedString();
    }
    private void BlueCardQuestPrefabItemsGUI()
    {
        instantiatedBlueCardPrefab = Instantiate(blueCardPrefab, items.transform);
        blueCardMainImage = instantiatedBlueCardPrefab.GetComponent<Image>();
        blueCardNameText = instantiatedBlueCardPrefab.GetComponentInChildren<TextMeshProUGUI>();
        blueCardImage = instantiatedBlueCardPrefab.transform.GetChild(1).GetComponent<Image>();

        blueCardMainImage.color = blueCardColor;
        blueCardImage.sprite = blueCardIcon;
        blueCardNameText.text = localizeStringBlueCardName.GetLocalizedString();
    }

    private void FuelCanQuestPrefabItemsGUI()
    {
        instantiatedFuelCanPrefab = Instantiate(fuelCanPrefab, items.transform);
        fuelCanMainImage = instantiatedFuelCanPrefab.GetComponent<Image>();
        fuelCanNameText = instantiatedFuelCanPrefab.GetComponentInChildren<TextMeshProUGUI>();
        fuelCanImage = instantiatedFuelCanPrefab.transform.GetChild(1).GetComponent<Image>();

        fuelCanMainImage.color = fuelCanColor;
        fuelCanImage.sprite = fuelCanIcon;
        fuelCanNameText.text = localizeStringFuelCanName.GetLocalizedString();
    }

    private void BarrelQuestPrefabItemsGUI()
    {
        instantiatedBarrelPrefab = Instantiate(barrelPrefab, items.transform);
        barrelMainImage = instantiatedBarrelPrefab.GetComponent<Image>();
        barrelNameText = instantiatedBarrelPrefab.GetComponentInChildren<TextMeshProUGUI>();
        barrelImage = instantiatedBarrelPrefab.transform.GetChild(1).GetComponent<Image>();

        barrelMainImage.color = barrelColor;
        barrelImage.sprite = barrelIcon;
        barrelNameText.text = localizeStringBarrelName.GetLocalizedString();
    }
}
