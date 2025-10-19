using FPS.Guns.Demo;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;
    public bool invert;
    [SerializeField]
    private Vector2 default_Look_Limits = new(-70f, 80f);
    private Vector2 look_Angles;
    private Vector2 current_Mouse_Look;

    public bool canLookAround = true;

    [SerializeField] private SensivitySettings sensivitySettings;

    [SerializeField, Range(0.01f, 1f)]
    private float zoomSensitivityMultiplier = 0.2f;

    void Start()
    {
        invert = PlayerPrefs.GetInt("InvertMouse", 0) == 1;
    }

    void Update()
    {
        if (!MainInventory.Instance.isPanelActive && Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LookAround()
    {
        if (!canLookAround)
        {
            current_Mouse_Look = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

            float effectiveSensitivity = sensivitySettings != null ? sensivitySettings.ClampedSensivityValue : 1f;

            if (PlayerGunSelector.Instance != null && PlayerGunSelector.Instance.isZoomed)
            {
                effectiveSensitivity *= zoomSensitivityMultiplier;
            }

            look_Angles.x += (invert ? current_Mouse_Look.x : -current_Mouse_Look.x) * effectiveSensitivity;

            look_Angles.y += (invert ? -current_Mouse_Look.y : current_Mouse_Look.y) * effectiveSensitivity;

            look_Angles.x = Mathf.Clamp(look_Angles.x, default_Look_Limits.x, default_Look_Limits.y);

            lookRoot.localRotation = Quaternion.Euler(look_Angles.x, 0f, 0f);
            playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
        }
    }

    public void SyncAnglesToTransforms()
    {
        float pitch = lookRoot.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        float yaw = playerRoot.localEulerAngles.y;
        if (yaw > 180f) yaw -= 360f;

        look_Angles = new Vector2(pitch, yaw);
    }

    public void SetInvert(bool value)
    {
        invert = value;
        PlayerPrefs.SetInt("InvertMouse", value ? 1 : 0);
    }
}