using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IEnemyDamagable
{
    public static PlayerHealth Instance { get; private set; }

    [Header("========== Damage Screen ==========")]
    [SerializeField] private Sprite[] damageScreens;
    [SerializeField] private Image damageScreenImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("============ Health ============")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Animator bandageAnimator;
    [SerializeField] private GameObject bandageObject;

    [Header("============ Oxygen ============")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private float maxOxygen = 100f;
    public float currentOxygen;
    private readonly float oxygenDecreaseRate = 0.02f;
    private readonly float oxygenIncreaseRate = 0.1f;
    [SerializeField] private Animator oxygenAnimator;
    [SerializeField] private GameObject oxygenObject;

    [Header("=========== Mars Mask ===========")]
    [SerializeField] private GameObject filterMaks;
    private Vector3 filterMaksTransformWhenInside = new(0f, 1100f, 0f);
    private readonly float timeWhenInsde = 1f;
    private Vector3 filterMaksTransformWhenOutsisde = new(0f, 0f, 0f);
    private readonly float timeWhenOutInsde = 0.2f;
    public bool isInside = false;

    [Header("=========== Pause Menu ===========")]
    [SerializeField] private PauseMenu pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();

        currentOxygen = maxOxygen;
        UpdateOxygenSlider();

        healthSlider.gameObject.SetActive(false);
        oxygenSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Heal(10));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(UseOxygenContainer(100));
        }

        if (OxygenHugeContainer.Instance.isRefillingOxygen)
        {
            IncreaseOxygen();
        }
        else if (!OxygenHugeContainer.Instance.isRefillingOxygen && isInside)
        {
            IncreaseOxygen();

            if (PlayerSingleton.Instance.marsHurricaneController.isHurricaneActive)
            {
                PlayerSingleton.Instance.marsHurricaneController.DeactivePs_MarsHurricane();
            }

            if (filterMaks != null)
            {
                filterMaks.transform.DOLocalMove(filterMaksTransformWhenInside, timeWhenInsde);
            }

        }
        else
        {
            if (PlayerSingleton.Instance.marsHurricaneController.isHurricaneActive)
            {
                PlayerSingleton.Instance.marsHurricaneController.ActivePs_MarsHurricane();
            }

            if (filterMaks != null)
            {
                filterMaks.transform.DOLocalMove(filterMaksTransformWhenOutsisde, timeWhenOutInsde);
            }
            DecreaseOxygen();
        }
    }

    void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth / maxHealth;

        float healthPercentage = currentHealth / maxHealth;
        damageScreenImage.DOFade(1f - healthPercentage, fadeDuration);

        if (currentHealth == maxHealth)
        {
            healthSlider.gameObject.SetActive(false);
            damageScreenImage.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            healthSlider.gameObject.SetActive(true);
            if (currentHealth >= 75)
            {
                damageScreenImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else if (currentHealth <= 75 && currentHealth > 50)
            {
                damageScreenImage.color = new Color(1f, 1f, 1f, 1f);
                damageScreenImage.sprite = damageScreens[0];
            }
            else if (currentHealth <= 50 && currentHealth > 25)
            {
                damageScreenImage.sprite = damageScreens[1];
            }
            else if (currentHealth <= 25)
            {
                damageScreenImage.sprite = damageScreens[2];
            }
            else
            {
                damageScreenImage.sprite = null;
            }
        }

    }

    void UpdateOxygenSlider()
    {
        oxygenSlider.value = currentOxygen / maxOxygen;

        if (currentOxygen == maxOxygen)
        {
            oxygenSlider.gameObject.SetActive(false);
        }
    }

    void DecreaseOxygen()
    {
        if (!pauseMenu.isGamePaused)
        {
            if (currentOxygen <= 75)
            {
                oxygenSlider.gameObject.SetActive(true);
            }

            currentOxygen -= oxygenDecreaseRate;

            currentOxygen = Mathf.Max(0f, currentOxygen);

            UpdateOxygenSlider();

            if (currentOxygen == 0f)
            {
                TakeDamage(0.025f);
            }
        }

    }

    public void IncreaseOxygen()
    {
        if (!pauseMenu.isGamePaused)
        {
            oxygenSlider.gameObject.SetActive(true);

            currentOxygen += oxygenIncreaseRate;

            currentOxygen = Mathf.Min(maxOxygen, currentOxygen);

            UpdateOxygenSlider();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 75)
        {
            healthSlider.gameObject.SetActive(true);
        }

        currentHealth -= damageAmount;

        currentHealth = Mathf.Max(0f, currentHealth);

        UpdateHealthSlider();

        if (currentHealth == 0f)
        {
            Debug.Log("Gracz zgin¹³!");
        }
    }

    private IEnumerator Heal(float healAmount)
    {
        if (currentHealth < maxHealth)
        {
            if (MainInventory.Instance.currentHealthBandage > 0)
            {
                bandageObject.SetActive(true);
                bandageAnimator.SetTrigger("HEAL");
                yield return new WaitForSeconds(1.2f);
                currentHealth += healAmount;
                currentHealth = Mathf.Min(maxHealth, currentHealth);
                UpdateHealthSlider();

                MainInventory.Instance.RemoveHealthBandage();
                if (MainInventory.Instance.currentHealthBandage == 0)
                {
                    MainInventory.Instance.isHealthBandageCreateAnPrefab = false;
                    Destroy(MainInventory.Instance.instantiatedHealthBandagePrefab);
                }

                yield return new WaitForSeconds(1);
                bandageAnimator.ResetTrigger("HEAL");
                bandageObject.SetActive(false);
            }
            else
            {
                Debug.Log("Nie masz wiecej Health Bandage");
            }
        }
        else
        {
            Debug.Log("Masz pe³en ¿ycie");
        }
    }

    private IEnumerator UseOxygenContainer(float oxygenAmount)
    {
        if (currentOxygen < maxOxygen)
        {
            if (MainInventory.Instance.currentOxygenContainer > 0)
            {
                oxygenObject.SetActive(true);
                oxygenAnimator.SetTrigger("REFIL");
                yield return new WaitForSeconds(1);

                currentOxygen += oxygenAmount;
                currentOxygen = Mathf.Min(maxOxygen, currentOxygen);
                UpdateOxygenSlider();

                MainInventory.Instance.RemoveOxygenContainer();
                if (MainInventory.Instance.currentOxygenContainer == 0)
                {
                    MainInventory.Instance.isOxygenCreateAnPrefab = false;
                    Destroy(MainInventory.Instance.instantiatedOxygenContainerPrefab);
                }

                yield return new WaitForSeconds(1);
                oxygenAnimator.ResetTrigger("REFIL");
                oxygenObject.SetActive(false);
            }
            else
            {
                Debug.Log("Nie masz wiecej Oxyden Container");
            }
        }
        else
        {
            Debug.Log("Masz pe³en tlen");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Inside"))
        {
            isInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inside"))
        {
            isInside = false;
        }
    }
}
