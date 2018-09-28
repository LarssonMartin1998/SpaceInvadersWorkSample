using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private LocalPositionAdapter positionAdapter;
    private DirectionRetriever directionRetriever;

    private Vector3 movementChange;
    private float movementSpeed;

    public PlayerMovement(LocalPositionAdapter posAdapter, DirectionRetriever dirRetriever, float moveSpeed)
    {
        positionAdapter = posAdapter;
        directionRetriever = dirRetriever;
        movementSpeed = moveSpeed;
        movementChange = new Vector3();
    }

    public void Move()
    {
        if (directionRetriever.Direction != 0f)
        {
            movementChange.Set(directionRetriever.Direction, 0f, 0f);
            positionAdapter.LocalPosition += movementChange * movementSpeed * Time.deltaTime;
        }
    }
}