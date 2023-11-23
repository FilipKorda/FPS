using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;
    [SerializeField]
    private bool invert;
    [SerializeField]
    private float sensivity = 5f;
    [SerializeField]
    private Vector2 default_Look_Limits = new Vector2(-70f, 80f);
    private Vector2 look_Angles;
    private Vector2 current_Mouse_Look;

    public bool canLookAround = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LockAndUnlockCursor();

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LookAround()
    {
        if (!canLookAround)
        {
            current_Mouse_Look = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

            look_Angles.x += current_Mouse_Look.x * sensivity * (invert ? 1f : -1f);
            look_Angles.y += current_Mouse_Look.y * sensivity;

            look_Angles.x = Mathf.Clamp(look_Angles.x, default_Look_Limits.x, default_Look_Limits.y);

            lookRoot.localRotation = Quaternion.Euler(look_Angles.x, 0f, 0f);
            playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
        }

    }

}