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
    


    private bool isPanelActive = false;
    [SerializeField] private GameObject usebleItems;

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
        }

      
    }

    void InputHandler()
    {
        if(isPanelActive)
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
        //amount
        oxygenItemAmountText = instantiatedOxygenContainerPrefab.GetComponentInChildren<TextMeshProUGUI>();
        //name
        oxygenItemNameText = instantiatedOxygenContainerPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //Sorite
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
        //amount
        healthBandageItemAmountText = instantiatedHealthBandagePrefab.GetComponentInChildren<TextMeshProUGUI>();
        //name
        healthBandageNameText = instantiatedHealthBandagePrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //Sorite
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
}
