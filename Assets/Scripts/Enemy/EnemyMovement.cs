using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface DirectionVectorRetriever
{
    Vector3 Direction { get; }
}

public class EnemyMovement : DirectionVectorRetriever
{
    private LocalPositionAdapter positionAdapter;

    private UnityAction directionSwapListener;
    private UnityAction moveListener;
    private EventManager cachedEventManager;

    private float movementChangeFactor;
    private Vector3 direction;
    private bool nextMoveIsDown = false;

    public Vector3 Direction
    {
        get { return direction; }
    }

    public EnemyMovement(LocalPositionAdapter posAdapter, float movementChange)
    {
        directionSwapListener = new UnityAction(OnDirectionSwap);
        moveListener = new UnityAction(Move);
        cachedEventManager = EventManager.instance;
        direction = Vector3.left;
        movementChangeFactor = movementChange;

        positionAdapter = posAdapter;
    }

    public void StartListening()
    {
        cachedEventManager.StartListening(AllEventTypes.EVENT_ENEMY_MOVE, moveListener);
        cachedEventManager.StartListening(AllEventTypes.EVENT_ENEMY_CHANGE_DIRECTION, directionSwapListener);
    }

    public void StopListening()
    {
        cachedEventManager.StopListening(AllEventTypes.EVENT_ENEMY_MOVE, moveListener);
        cachedEventManager.StopListening(AllEventTypes.EVENT_ENEMY_CHANGE_DIRECTION, directionSwapListener);
    }

    private void Move()
    {
        if (nextMoveIsDown)
        {
            nextMoveIsDown = false;
            positionAdapter.LocalPosition += Vector3.down * movementChangeFactor;
            return;
        }

        positionAdapter.LocalPosition += direction * movementChangeFactor;
    }

    private void OnDirectionSwap()
    {
        nextMoveIsDown = true;
        direction = (direction == Vector3.left) ? Vector3.right : Vector3.left;
    }
}