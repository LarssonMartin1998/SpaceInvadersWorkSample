using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, LocalPositionAdapter
{
    [SerializeField]
    private LayerMask directionSwapLayerLeft;
    [SerializeField]
    private LayerMask directionSwapLayerRight;
    [SerializeField]
    private LayerMask loseLayer;
    [SerializeField]
    private float movementChangeFactor = 1.5f;

    private EnemyMovement enemyMovement;
    private EnemyLogic enemyLogic;

    private void Awake()
    {
        enemyMovement = new EnemyMovement(this, movementChangeFactor);
        enemyLogic = new EnemyLogic(enemyMovement, loseLayer, directionSwapLayerLeft, directionSwapLayerRight);
    }

    private void OnEnable()
    {
        enemyMovement.StartListening();
    }

    private void OnDisable()
    {
        enemyMovement.StopListening();
    }

    public Vector3 LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyLogic.HandleTriggerOverlap(other.gameObject.layer);
    }
}