using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, GameObjectReciever, LocalPositionAdapter
{
    [SerializeField]
    private float movementSpeed = 20f;
    [SerializeField]
    private LayerMask hitTargets;

    private RocketMovement rocketMovement;
    private RocketLogic rocketLogic;

    public GameObject Object
    {
        get { return gameObject; }
    }

    public Vector3 LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    private void Awake()
    {
        rocketMovement = new RocketMovement(this, movementSpeed);
        rocketLogic = new RocketLogic(this, this, hitTargets);
    }

    private void Update()
    {
        rocketMovement.Move();
        rocketLogic.HandleOutOfBounds();
    }

    private void OnTriggerEnter(Collider other)
    {
        rocketLogic.HandleOverlappingTriggers(other.gameObject);
    }
}