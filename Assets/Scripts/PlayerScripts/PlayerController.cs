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
        // natychmiastowe wejœcie bez wyg³adzenia (koniec "p³yniêcia")
        Vector3 input = new Vector3(Input.GetAxisRaw(Axis.HORIZONTAL), 0f,
                                    Input.GetAxisRaw(Axis.VERTICAL));

        // zapobiegamy szybszemu poruszaniu po przek¹tnej
        if (input.sqrMagnitude > 1f) input.Normalize();

        // prêdkoœæ horyzontalna w m/s
        Vector3 horizontalVelocity = transform.TransformDirection(input) * (speed * ExternalSpeedMultiplier);

        // aktualizuj pionow¹ prêdkoœæ (m/s)
        ApplyGravity();

        // ca³kowity ruch jako velocity (m/s)
        Vector3 velocity = horizontalVelocity + Vector3.up * vertical_Velocity;

        // przesuniêcie na klatkê
        var flags = character_Controller.Move(velocity * Time.deltaTime);

        if ((flags & CollisionFlags.Above) != 0 && vertical_Velocity > 0f)
        {
            vertical_Velocity = 0f; 
        }

        // dla innych systemów (jeœli potrzebujesz)
        move_Direction = velocity;
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

        // ju¿ nie przypisujemy tu move_Direction.y jako przesuniêcia — vertical_Velocity jest prêdkoœci¹ (m/s)
    }

    void PlayerJump()
    {
        if (character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }
    }
}
