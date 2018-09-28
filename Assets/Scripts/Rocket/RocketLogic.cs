using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLogic
{
    private LocalPositionAdapter positionAdapter;
    private GameObjectReciever objectReciever;

    private EventManager cachedEventManager;

    private LayerMask hitTargets;
    private float yTopOfScreenWorld;

    public RocketLogic(LocalPositionAdapter posAdapter, GameObjectReciever objReciever, LayerMask htTargets)
    {
        cachedEventManager = EventManager.instance;

        positionAdapter = posAdapter;
        objectReciever = objReciever;

        hitTargets = htTargets;

        Vector3 topOfScreen = new Vector3(0f, Screen.height, 0f);
        yTopOfScreenWorld = Camera.main.ScreenToWorldPoint(topOfScreen).y;
    }

    public void HandleOutOfBounds()
    {
        if (positionAdapter.LocalPosition.y > yTopOfScreenWorld)
        {
            DisableSelf();
        }
    }

    public void DisableSelf()
    {
        objectReciever.Object.SetActive(false);
    }

    public void HandleOverlappingTriggers(GameObject other)
    {
        // Compare using layers to avoid string comparison.
        if (((1 << other.layer) & hitTargets) != 0)
        {
            cachedEventManager.TriggerEvent(AllEventTypes.EVENT_ENEMY_DIED);
            DisableSelf();
            other.SetActive(false);
        }
    }
}