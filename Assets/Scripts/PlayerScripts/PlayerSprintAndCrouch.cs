using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float sprint_Value = 100f;
    public float sprint_Treshold = 7f;

    void Awake()
    {
        playerMovement = GetComponent<PlayerController>();
        look_Root = transform.GetChild(0);
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (sprint_Value > 0f)
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
            sprint_Value -= sprint_Treshold * Time.deltaTime;

            if (sprint_Value <= 0f)
            {
                sprint_Value = 0f;
                playerMovement.speed = move_Speed;
            }
        }
        else
        {
            if (sprint_Value != 100f)
            {
                sprint_Value += (sprint_Treshold / 2f) * Time.deltaTime;

                if (sprint_Value > 100f)
                {
                    sprint_Value = 100f;
                }
            }
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