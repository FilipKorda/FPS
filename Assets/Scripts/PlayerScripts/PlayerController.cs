using FPS.Guns.Demo;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController character_Controller;
    private Vector3 move_Direction;
    public float speed = 5f;
    private readonly float gravity = 20f;
    public float jump_Force = 10f;
    private float vertical_Velocity;
    public List<GunPickup> gunPickupList;

    private readonly float slidingForce = 10f;
    private readonly float raycastDistance = 1.05f;
    private bool isSliding = false;
    private Vector3 raycastDir = Vector3.down;
    private Vector3 slidingDirection = Vector3.left;
    public BridgeButton bridgeButton;



    void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        HandleGunPickups();

        if (Physics.Raycast(transform.position, raycastDir, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("SlipperySurface"))
            {
                isSliding = true;

                Vector3 currentPosition = transform.position;
                Vector3 newPosition = currentPosition + slidingDirection * slidingForce * Time.deltaTime;
                transform.position = newPosition;
            }
            else
            {
                isSliding = false;
            }

            if (hit.collider.CompareTag("Platform"))
            {
                character_Controller.Move(bridgeButton.speed * Time.deltaTime * bridgeButton.direction);
            }
            else
            {
                move_Direction *= speed * Time.deltaTime;
            }
        }
        else
        {
            isSliding = false;
        }

        Debug.DrawRay(transform.position, raycastDir * raycastDistance, Color.red);

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
            NotificationSystem.Instance.ShowNotification("You can only change second gun", 1.0f);
        }
    }

    void Move()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                     Input.GetAxis(Axis.VERTICAL));
        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;
        ApplyGravity();
        character_Controller.Move(move_Direction);
    }

    void ApplyGravity()
    {
        vertical_Velocity -= gravity * Time.deltaTime;
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
