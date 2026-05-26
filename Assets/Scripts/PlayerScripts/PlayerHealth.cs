using DG.Tweening;
using FPS.Guns.Demo;
using System.Collections;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI pressXToHeal;

    [Header("============ Oxygen ============")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private float maxOxygen = 100f;
    public float currentOxygen;
    private readonly float oxygenDecreaseRate = 0.02f;
    private readonly float oxygenIncreaseRate = 0.1f;
    [SerializeField] private Animator oxygenAnimator;
    [SerializeField] private GameObject oxygenObject;
    [SerializeField] private TextMeshProUGUI pressZToRefilOxygen;

    [Header("=========== Mars Mask ===========")]
    [SerializeField] private GameObject filterMaks;
    private Vector3 filterMaksTransformWhenInside = new(0f, 1100f, 0f);
    private readonly float timeWhenInsde = 1f;
    private Vector3 filterMaksTransformWhenOutsisde = new(0f, 0f, 0f);
    private readonly float timeWhenOutInsde = 0.2f;
    public bool isInside = true;
    public bool haveMask = false;
    [SerializeField] private AudioSource maskOff;
    [SerializeField] private AudioClip maskOffClip;
    [SerializeField] private AudioSource wearMask;
    [SerializeField] private AudioClip wearMaskClip;

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

    [Header("=========== Hints Blink ===========")]
    [SerializeField] private float hintFadeDuration = 0.25f;
    [SerializeField] private float hintVisibleDuration = 1f;
    [SerializeField] private float hintHiddenDuration = 2f;

    private Sequence pressXBlinkSeq;
    private Sequence pressZBlinkSeq;

    [Header("=========== Direct Of Danger ===========")]
    [SerializeField] private DirectOfDanger directOfDanger;

    [Header("=========== Death Sounds ===========")]
    [Tooltip("Lista dŸwiêków œmierci — bêdzie wybierany losowo.")]
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioSource deathAudioSource;

    [Header("=========== Damage Sounds ===========")]
    [Tooltip("Lista dŸwiêków przy otrzymaniu obra¿eñ — bêdzie wybierany losowo.")]
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioSource damageAudioSource;

    [Header("=========== Choking Sounds ===========")]
    [Tooltip("Lista dŸwiêków duszenia (brak tlenu) — bêd¹ odtwarzane kolejno.")]
    [SerializeField] private AudioClip[] chokingClips;
    [SerializeField] private AudioSource chokingDamageAudioSource;

    private bool lastDamageWasOxygen = false;

    private int chokingIndex = 0;

    private bool wasInside; // NEW
    private Tween maskTween; // NEW
    private bool isHealing;
    private bool isUsingOxygenContainer;

    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip refilOxygenSound;

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

        if (deathAudioSource != null)
        {
            deathAudioSource.playOnAwake = false;
            deathAudioSource.volume = 0.05f;
        }

        if (damageAudioSource != null)
        {
            damageAudioSource.playOnAwake = false;
            damageAudioSource.volume = 0.05f;
        }

        if (chokingDamageAudioSource != null)
        {
            chokingDamageAudioSource.playOnAwake = false;
            chokingDamageAudioSource.volume = 0.05f;
        }
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

        wasInside = isInside;          // NEW
        ApplyMaskState(wasInside);     // NEW (ustaw stan pocz¹tkowy maski)
    }

    private void Update()
    {
        if (isDead) return;

        if (!isInside && !haveMask)
        {
            DeathCauseManager.MarkNoMask();
            TakeDamage(1000, transform.position);
            return;
        }

        // cheat
        if (Input.GetKeyDown(KeyCode.O)) TakeDamage(10, transform.position);
        if (Input.GetKeyDown(KeyCode.X)) StartCoroutine(Heal(20));
        if (Input.GetKeyDown(KeyCode.Z)) StartCoroutine(UseOxygenContainer(60));

        if (isInside != wasInside)     
        {
            ApplyMaskState(isInside);
            wasInside = isInside;     
        }

        if (isInside)
        {
            IncreaseOxygen();

            if (PlayerSingleton.Instance.marsHurricaneController.isHurricaneActive)
            {
                PlayerSingleton.Instance.marsHurricaneController.DeactivePs_MarsHurricane();
            }
        }
        else
        {
            if (PlayerSingleton.Instance.marsHurricaneController.isHurricaneActive == false)
            {
                PlayerSingleton.Instance.marsHurricaneController.ActivePs_MarsHurricane();
            }

            DecreaseOxygen();
        }
    }

    private void ApplyMaskState(bool nowInside)
    {
        if (filterMaks == null) return;

        maskTween?.Kill(); // nie nak³adaj wielu tweenów

        if (nowInside)
        {
            // Zdejmujemy maskê (ruch w górê)
            if (maskOff != null && maskOffClip != null)
            {
                maskOff.PlayOneShot(maskOffClip);
            }

            maskTween = filterMaks.transform
                .DOLocalMove(filterMaksTransformWhenInside, timeWhenInsde);
        }
        else
        {
            if (wearMask != null && wearMaskClip != null)
            {
                wearMask.PlayOneShot(wearMaskClip);
            }

            maskTween = filterMaks.transform
                .DOLocalMove(filterMaksTransformWhenOutsisde, timeWhenOutInsde);
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
                StopBlink(pressXToHeal, ref pressXBlinkSeq, true);
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
            StopBlink(pressZToRefilOxygen, ref pressZBlinkSeq, true);
        }
    }

    void DecreaseOxygen()
    {
        if (!pauseMenu.isGamePaused)
        {
            if (currentOxygen <= 75)
            {
                oxygenSlider.gameObject.SetActive(true);
                StartBlink(pressZToRefilOxygen, ref pressZBlinkSeq);
            }

            currentOxygen -= oxygenDecreaseRate;

            currentOxygen = Mathf.Max(0f, currentOxygen);

            UpdateOxygenSlider();

            StatisticsCollector.AddOxygenLost(oxygenDecreaseRate);

            if (currentOxygen == 0f)
            {
                DeathCauseManager.MarkNoOxygen();

                lastDamageWasOxygen = true;
                TakeDamage(0.025f, transform.position);
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

            if (currentOxygen < maxOxygen)
            {
                StatisticsCollector.AddOxygenRecovery(oxygenIncreaseRate);
            }
        }
    }

    public void TakeDamage(float damageAmount, UnityEngine.Vector3 sourcePosition)
    {
        if (isDead) return;

        directOfDanger.NotifyDamageFrom(sourcePosition, PlayerSingleton.Instance.transform, playerCamera, true);

        if (currentHealth <= 74)
        {
            StartBlink(pressXToHeal, ref pressXBlinkSeq);
        }
        else if (currentHealth <= 75)
        {
            healthSlider.gameObject.SetActive(true);
        }

        StatisticsCollector.AddHealthLost(damageAmount);

        currentHealth -= damageAmount;

        currentHealth = Mathf.Max(0f, currentHealth);

        UpdateHealthSlider();

        if (currentHealth > 0f)
        {
            if (lastDamageWasOxygen)
            {
                PlayNextChokingSound();
            }
            else
            {
                PlayRandomDamageSound();
            }
        }

        lastDamageWasOxygen = false;

        if (currentHealth == 0f)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        if (isDead) return;

        DisablePlayer();
        DetacedGunFromPlayer();

        if (cameraFallCoroutine != null)
        {
            StopCoroutine(cameraFallCoroutine);
            cameraFallCoroutine = null;
        }

        PlayRandomDeathSound();

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayLose();

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
        if (!haveMask) return;
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

        yield return new WaitForSecondsRealtime(2f);

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
        if (isHealing) yield break;
        if (currentHealth >= maxHealth) yield break;
        if (MainInventory.Instance.currentHealthBandage <= 0) yield break;

        isHealing = true;

        if (currentHealth < maxHealth)
        {
            if (MainInventory.Instance.currentHealthBandage > 0)
            {

                bandageObject.SetActive(true);
                bandageAnimator.SetTrigger("HEAL");
                yield return new WaitForSeconds(1.2f);
                AudioManager.Instance.PlayClip(healSound, transform.position, 0.3f, false, 1, 500, 1, false, null);
                currentHealth += healAmount;
                currentHealth = Mathf.Min(maxHealth, currentHealth);
                UpdateHealthSlider();

                StatisticsCollector.AddHealthHealed(healAmount);

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

        }

        isHealing = false;
    }

    private IEnumerator UseOxygenContainer(float oxygenAmount)
    {
        if (isUsingOxygenContainer) yield break;
        if (currentOxygen >= maxOxygen) yield break;
        if (MainInventory.Instance.currentOxygenContainer <= 0) yield break;


        isUsingOxygenContainer = true;

        if (currentOxygen < maxOxygen)
        {
            if (MainInventory.Instance.currentOxygenContainer > 0)
            {           
                oxygenObject.SetActive(true);
                oxygenAnimator.SetTrigger("REFIL");
                yield return new WaitForSeconds(1);
                AudioManager.Instance.PlayClip(refilOxygenSound, transform.position, 0.3f, false, 1, 500, 1, false, null);
                currentOxygen += oxygenAmount;
                currentOxygen = Mathf.Min(maxOxygen, currentOxygen);
                UpdateOxygenSlider();

                StatisticsCollector.AddOxygenRecovery(oxygenAmount);

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
            
        }
        

        isUsingOxygenContainer = false;
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

    private void EnsureBlinkSequence(TextMeshProUGUI text, ref Sequence seq)
    {
        if (text == null) return;
        if (seq != null) return;

        var c = text.color;
        c.a = 0f;
        text.color = c;

        seq = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause()
            .Append(text.DOFade(1f, hintFadeDuration))
            .AppendInterval(hintVisibleDuration)
            .Append(text.DOFade(0f, hintFadeDuration))
            .AppendInterval(hintHiddenDuration)
            .SetLoops(-1, LoopType.Restart);
    }

    private void StartBlink(TextMeshProUGUI text, ref Sequence seq)
    {
        if (text == null) return;
        text.gameObject.SetActive(true);
        EnsureBlinkSequence(text, ref seq);
        seq.Restart();
    }

    private void StopBlink(TextMeshProUGUI text, ref Sequence seq, bool hideGO)
    {
        if (text == null) return;
        if (seq != null)
        {
            seq.Pause();
            seq.Rewind();
        }

        var c = text.color;
        c.a = 0f;
        text.color = c;

        if (hideGO) text.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        pressXBlinkSeq?.Kill();
        pressZBlinkSeq?.Kill();
    }

    private void PlayRandomDeathSound()
    {
        if (deathAudioSource == null) return;
        if (deathClips == null || deathClips.Length == 0) return;

        var clip = deathClips[Random.Range(0, deathClips.Length)];
        if (clip != null)
        {
            deathAudioSource.PlayOneShot(clip);
        }
    }

    private void PlayRandomDamageSound()
    {
        if (damageAudioSource == null) return;
        if (damageClips == null || damageClips.Length == 0) return;

        var clip = damageClips[Random.Range(0, damageClips.Length)];
        if (clip != null)
        {
            damageAudioSource.PlayOneShot(clip);
        }
    }

    private void PlayNextChokingSound()
    {
        if (chokingDamageAudioSource == null) return;
        if (chokingClips == null || chokingClips.Length == 0) return;

        var clip = chokingClips[chokingIndex];
        chokingIndex = (chokingIndex + 1) % chokingClips.Length;

        chokingDamageAudioSource.clip = clip;
        chokingDamageAudioSource.Play();
    }
}
