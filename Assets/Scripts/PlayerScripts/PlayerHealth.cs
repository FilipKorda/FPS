using DG.Tweening;
using FPS.Guns.Demo;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private Sprite[] damageScreens;
    [SerializeField] private Image damageScreenImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private float maxOxygen = 100f;
    private float currentOxygen;
    private readonly float oxygenDecreaseRate = 0.02f;
    private readonly float oxygenIncreaseRate = 0.1f;

    private bool isInside = false;

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            AddOxygen(100);
        }

        if (isInside)
        {
            IncreaseOxygen();
        }
        else
        {
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

    void IncreaseOxygen()
    {
        oxygenSlider.gameObject.SetActive(true);

        currentOxygen += oxygenIncreaseRate;

        currentOxygen = Mathf.Min(maxOxygen, currentOxygen);

        UpdateOxygenSlider();
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

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealthSlider();
    }

    public void AddOxygen(float oxygenAmount)
    {
        currentOxygen += oxygenAmount;
        currentOxygen = Mathf.Min(maxOxygen, currentOxygen);
        UpdateOxygenSlider();
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
