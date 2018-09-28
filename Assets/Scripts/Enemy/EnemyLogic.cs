using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLogic
{
    private DirectionVectorRetriever directionRetriever;

    private EventManager cachedEventManager;

    private LayerMask loseLayer;
    private LayerMask directionSwapLayerLeft;
    private LayerMask directionSwapLayerRight;

    public EnemyLogic(DirectionVectorRetriever dirRetriever,LayerMask lseLayer, LayerMask dirSwapLayerLeft, LayerMask dirSwapLayerRight)
    {
        cachedEventManager = EventManager.instance;

        directionRetriever = dirRetriever;

        loseLayer = lseLayer;
        directionSwapLayerLeft = dirSwapLayerLeft;
        directionSwapLayerRight = dirSwapLayerRight;
    }

    public void HandleTriggerOverlap(int otherLayer)
    {
        // Compare layers to void string comparison
        if (((1 << otherLayer) & loseLayer) != 0)
        {
            cachedEventManager.TriggerEvent(AllEventTypes.EVENT_USER_LOST);
        }

        if (((1 << otherLayer) & directionSwapLayerLeft) != 0)
        {
            if (directionRetriever.Direction == Vector3.left)
            {
                cachedEventManager.TriggerEvent(AllEventTypes.EVENT_ENEMY_CHANGE_DIRECTION);
            }
        }

        if (((1 << otherLayer) & directionSwapLayerRight) != 0)
        {
            if (directionRetriever.Direction == Vector3.right)
            {
                cachedEventManager.TriggerEvent(AllEventTypes.EVENT_ENEMY_CHANGE_DIRECTION);
            }
        }
    }
}