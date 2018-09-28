using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, LocalPositionAdapter
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float bulletCooldown = 0.5f;

    public Vector3 LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerMovement = new PlayerMovement(this, playerInput, movementSpeed);
        playerShooting = new PlayerShooting(this, playerInput, bulletCooldown);
    }

    private void Update()
    {
        playerInput.GetInputData();
        playerInput.BindInputDataToActions();
        playerMovement.Move();
        playerShooting.AttemptToShoot();
        playerShooting.HandleCooldown();
    }

}