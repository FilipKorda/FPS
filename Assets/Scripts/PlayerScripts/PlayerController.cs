using FPS.Guns.Demo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class PlayerController : MonoBehaviour
{
    private CharacterController character_Controller;
    private Vector3 move_Direction;
    public float speed = 5f;
    private readonly float gravity = 20f;
    public float jump_Force = 10f;
    private float vertical_Velocity;
    public List<GunPickup> gunPickupList;
    public bool canMove = true;


    public LocalizedString localizeStringEvent;

    public float ExternalSpeedMultiplier { get; private set; } = 1f;
    public void SetExternalSpeedMultiplier(float multiplier)
    {
        ExternalSpeedMultiplier = Mathf.Clamp(multiplier, 0f, 1f);
    }

    void Awake()
    {
        character_Controller = GetComponent<CharacterController>();      
    }

    void Update()
    {
        Move();
        HandleGunPickups();
    }

    void HandleGunPickups()
    {
        foreach (var gunPickup in gunPickupList)
        {
            if (gunPickup.isImageActivate && Input.GetKeyDown(KeyCode.E))
            {
                TryPickupGun();
            }
        }
    }

    void TryPickupGun()
    {
        if (PlayerGunSelector.Instance.Guns[PlayerGunSelector.Instance.activeGunIndex] == PlayerGunSelector.Instance.Guns[1])
        {
            foreach (var gunPickup in gunPickupList)
            {
                if (gunPickup.isImageActivate)
                {
                    gunPickup.PickupGun();

                    break;
                }
            }
        }
        else
        {
            NotificationSystem.Instance.ShowNotification(localizeStringEvent,"You can only change second gun", 1.0f);
        }
    }

    void Move()
    {
        if (!canMove)
        {
            move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                  Input.GetAxis(Axis.VERTICAL));
            move_Direction = transform.TransformDirection(move_Direction);
            // U¿yj mno¿nika prêdkoœci
            move_Direction *= (speed * ExternalSpeedMultiplier) * Time.deltaTime;

            ApplyGravity();

            var flags = character_Controller.Move(move_Direction);

            if ((flags & CollisionFlags.Above) != 0 && vertical_Velocity > 0f)
            {
                vertical_Velocity = 0f; 
            }
        }
    }

    void ApplyGravity()
    {
        if (character_Controller.isGrounded)
        {
            if (vertical_Velocity < 0f)
            {
                vertical_Velocity = -2f; 
            }
        }
        else
        {
            vertical_Velocity -= gravity * Time.deltaTime;
        }

        PlayerJump();

        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }

    void PlayerJump()
    {
        if (character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }
    }
}
