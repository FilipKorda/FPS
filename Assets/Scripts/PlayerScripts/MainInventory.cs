using TMPro;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    public static MainInventory Instance { get; private set; }

    [SerializeField] private GameObject ButtonTabPanel;
    private bool isPanelActive = false;
    [SerializeField] private GameObject usebleItems;

    [Header("======= Oxygen Container =======")]
    [Space(5)]   
    public int currentOxygenContainer = 0;
    [SerializeField] private GameObject oxygenContainerPrefab;
    private TextMeshProUGUI oxygenItemText;
    public GameObject instantiatedOxygenContainerPrefab;
    public bool isOxygenCreateAnPrefab;
    private int maxOxygenContainer = 3;

    [Header("======= Health Bandage =======")]
    [Space(5)]  
    public int currentHealthBandage = 0;
    private int maxHealthBandage = 3;
    [SerializeField] private GameObject healthBandagePrefab;
    private TextMeshProUGUI healthBandageItemText;
    public GameObject instantiatedHealthBandagePrefab;
    public bool isHealthBandageCreateAnPrefab;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ButtonTabPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isPanelActive = !isPanelActive;
            ButtonTabPanel.SetActive(isPanelActive);
        }
    }

    private void OxygenUseblePrefabItemsToGUI()
    {
        isOxygenCreateAnPrefab = true;
        instantiatedOxygenContainerPrefab = Instantiate(oxygenContainerPrefab, usebleItems.transform);
        oxygenItemText = instantiatedOxygenContainerPrefab.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void UpdateAmountOfOxygenNumber()
    {
        if (oxygenItemText != null)
        {
            oxygenItemText.text = currentOxygenContainer.ToString();
        }
    }

    public void AddOxygenContainer()
    {
        if (!isOxygenCreateAnPrefab)
        {
            OxygenUseblePrefabItemsToGUI();
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
        healthBandageItemText = instantiatedHealthBandagePrefab.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void UpdateAmountOfHealthBandageNumber()
    {
        if (healthBandageItemText != null)
        {
            healthBandageItemText.text = currentHealthBandage.ToString();
        }
    }

    public void AddHealthBandage()
    {
        if (!isHealthBandageCreateAnPrefab)
        {
            HealthBandageUseblePrefabItemsToGUI();
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
