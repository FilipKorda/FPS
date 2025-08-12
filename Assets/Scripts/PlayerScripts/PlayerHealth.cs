using DG.Tweening;
using FPS.Guns.Demo;
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

    [Header("=========== Player Death ===========")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fallDuration = 1.5f;
    [SerializeField] private GameObject gunSelectorPanel;
    [SerializeField] private GameObject statisticGunPanel;
    [SerializeField] private GameObject compasBarPanel;
    [SerializeField] private GameObject grenadeSelectorPanel;
    [SerializeField] private GameObject playerHealthGUI;
    [SerializeField] private GameObject gunParent;
    [SerializeField] private GameObject gameOverScreenPanel;
    [SerializeField] private GameObject damageScreenPanel;
    [SerializeField] private GameObject marsMaskPanel;
    [SerializeField] private GameObject croshair;
    [SerializeField] private GameObject ammoPackPanel;
    private MeshRenderer playerMesh;
    private CapsuleCollider playerCapsule;
    private CharacterController playerCharacterController;
    private PlayerSingleton playerSingleton;
    private PlayerController playerController;
    private MouseLook mouseLook;
    private PlayerSprintAndCrouch playerSprintAndCrouch;
    private PlayerGunSelector playerGunSelector;
    private PlayerAction playerAction;
    private GrenadeHandler grenadeHandler;
    private EagleVisionManager eagleVisionManager;
    private InteractionManager interactionManager;
    private MainInventory mainInventory;
    private MarsHurricaneController marsHurricaneController;

    private Coroutine cameraFallCoroutine;
    private bool isCameraFallActive;
    public bool isDead = false;

    private void Awake()
    {
        Instance = this;

        playerMesh = GetComponent<MeshRenderer>();
        playerCapsule = GetComponent<CapsuleCollider>();
        playerCharacterController = GetComponent<CharacterController>();
        playerSingleton = GetComponent<PlayerSingleton>();
        playerController = GetComponent<PlayerController>();
        mouseLook = GetComponent<MouseLook>();
        playerSprintAndCrouch = GetComponent<PlayerSprintAndCrouch>();
        playerGunSelector = GetComponent<PlayerGunSelector>();
        playerAction = GetComponent<PlayerAction>();
        grenadeHandler = GetComponent<GrenadeHandler>();
        eagleVisionManager = GetComponent<EagleVisionManager>();
        interactionManager = GetComponent<InteractionManager>();
        mainInventory = GetComponent<MainInventory>();
        marsHurricaneController = GetComponent<MarsHurricaneController>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();

        currentOxygen = maxOxygen;
        UpdateOxygenSlider();

        healthSlider.gameObject.SetActive(false);
        oxygenSlider.gameObject.SetActive(false);

        gameOverScreenPanel.SetActive(false);
    }

    private void Update()
    {
        if (isDead) return;

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
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        DisablePlayer();
        DetacedGunFromPlayer();

        if (cameraFallCoroutine != null)
        {
            StopCoroutine(cameraFallCoroutine);
            cameraFallCoroutine = null;
        }

        isDead = true;
        isCameraFallActive = true;
        cameraFallCoroutine = StartCoroutine(CameraFallEffect());
    }

    private void DisablePlayer()
    {
        currentOxygen = 0;

        gunSelectorPanel.SetActive(false);
        statisticGunPanel.SetActive(false);
        compasBarPanel.SetActive(false);
        grenadeSelectorPanel.SetActive(false);
        playerHealthGUI.SetActive(false);
        croshair.SetActive(false);
        ammoPackPanel.SetActive(false);

        playerMesh.enabled = false;
        playerCapsule.enabled = false;
        playerCharacterController.enabled = false;

        playerSingleton.enabled = false;
        playerController.enabled = false;
        mouseLook.enabled = false;
        playerSprintAndCrouch.enabled = false;
        playerGunSelector.enabled = false;
        playerAction.enabled = false;
        grenadeHandler.enabled = false;
        eagleVisionManager.enabled = false;
        interactionManager.enabled = false;
        mainInventory.enabled = false;
        marsHurricaneController.enabled = false;
    }

    private void DetacedGunFromPlayer()
    {
        Transform child = gunParent.transform.GetChild(0);
        child.gameObject.AddComponent<BoxCollider>();
        child.gameObject.AddComponent<Rigidbody>();
    }

    private IEnumerator CameraFallEffect()
    {
        Quaternion startRotation = playerCamera.transform.localRotation;
        Vector3 startPosition = playerCamera.transform.localPosition;

        Quaternion targetRotation = startRotation * Quaternion.Euler(85f, Random.Range(-20f, 20f), Random.Range(-30f, 30f));
        Vector3 targetPosition = startPosition + new Vector3(0f, -0.7f, 0.2f);

        float elapsed = 0f;
        float duration = fallDuration;

        Quaternion finalRotation = startRotation;
        Vector3 finalPosition = startPosition;

        while (elapsed < duration)
        {
            if (!isCameraFallActive) yield break;

            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float curveT = Mathf.SmoothStep(0f, 1f, t);
            float shake = Mathf.Sin(t * 25f) * (1f - curveT) * 0.02f;

            Quaternion currentRotation = Quaternion.Slerp(startRotation, targetRotation, curveT);
            currentRotation *= Quaternion.Euler(shake * 200f, shake * 100f, 0);

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, curveT);

            playerCamera.transform.localRotation = currentRotation;
            playerCamera.transform.localPosition = currentPosition;

            finalRotation = currentRotation;
            finalPosition = currentPosition;

            yield return null;
        }

        playerCamera.transform.localRotation = finalRotation;
        playerCamera.transform.localPosition = finalPosition;

        isCameraFallActive = false;
        cameraFallCoroutine = null;

        yield return new WaitForSeconds(2);

        ActiveGameOverScreen();
    }

    private void ActiveGameOverScreen()
    {
        gameOverScreenPanel.SetActive(true);
        damageScreenPanel.SetActive(false);
        marsMaskPanel.SetActive(false);
    }

    public void StopCameraFall()
    {
        isCameraFallActive = false;
        if (cameraFallCoroutine != null)
        {
            StopCoroutine(cameraFallCoroutine);
            cameraFallCoroutine = null;
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

    public void SetIsInside(bool inside)
    {
        isInside = inside;
    }
}
