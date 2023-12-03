using FPS.Guns.Demo;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerController playerMovement;
    public float sprint_Speed = 5f;
    public float move_Speed = 3f;
    public float crouch_Speed = 2f;
    private Transform look_Root;
    private readonly float stand_Height = 0.5f;
    private readonly float crouch_Height = 0.1f;
    private bool is_Crouching = false;
    public float sprint_Treshold = 0.1f;

    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    private bool isRecovering = false;
    [SerializeField] private CameraFovSettings cameraFovSettings;

    void Awake()
    {
        playerMovement = GetComponent<PlayerController>();
        look_Root = transform.GetChild(0);
    }

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaSlider();
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    public void UpdateStaminaSlider()
    {
        staminaSlider.value = currentStamina / maxStamina;

        if (currentStamina == maxStamina)
        {
            staminaSlider.gameObject.SetActive(false);
        }
        else
        {
            staminaSlider.gameObject.SetActive(true);
        }
    }


    void IncreaseStamina()
    {
        staminaSlider.gameObject.SetActive(true);

        currentStamina += sprint_Treshold;

        currentStamina = Mathf.Min(maxStamina, currentStamina);

        if (currentStamina > 100f)
        {
            currentStamina = 100f;
        }

        isRecovering = true;
        UpdateStaminaSlider();
    }

    void DecreaseStamina()
    {
        staminaSlider.gameObject.SetActive(true);

        currentStamina -= sprint_Treshold;

        currentStamina = Mathf.Max(0f, currentStamina);

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            playerMovement.speed = move_Speed;

        }

        isRecovering = false;
        UpdateStaminaSlider();
    }

    void ChangeFOVSmoothly(float targetFOV, float duration)
    {
        PlayerGunSelector.Instance.Camera.DOFieldOfView(targetFOV, duration);
    }

    void Sprint()
    {
        if (currentStamina > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
            {

                playerMovement.speed = sprint_Speed;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {

            playerMovement.speed = move_Speed;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            ChangeFOVSmoothly(cameraFovSettings.ClampedValue + 4, 1f);
            DecreaseStamina();
        }
        else
        {
            ChangeFOVSmoothly(cameraFovSettings.ClampedValue, 1f);
            IncreaseStamina();
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
            playerMovement.speed = crouch_Speed;
            is_Crouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
            playerMovement.speed = move_Speed;
            is_Crouching = false;
        }
    }
}