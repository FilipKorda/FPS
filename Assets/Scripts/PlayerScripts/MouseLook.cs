using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;
    [SerializeField]
    private bool invert;
    [SerializeField]
    private Vector2 default_Look_Limits = new(-70f, 80f);
    private Vector2 look_Angles;
    private Vector2 current_Mouse_Look;

    public bool canLookAround = true;

    [SerializeField] private SensivitySettings sensivitySettings;

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
            current_Mouse_Look = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

            look_Angles.x += current_Mouse_Look.x * sensivitySettings.ClampedSensivityValue * (invert ? 1f : -1f);
            look_Angles.y += current_Mouse_Look.y * sensivitySettings.ClampedSensivityValue;

            look_Angles.x = Mathf.Clamp(look_Angles.x, default_Look_Limits.x, default_Look_Limits.y);

            lookRoot.localRotation = Quaternion.Euler(look_Angles.x, 0f, 0f);
            playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
        }

    }

}