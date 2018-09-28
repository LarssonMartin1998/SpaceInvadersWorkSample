using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement
{
    LocalPositionAdapter positionAdapter;

    private float movementSpeed;

    public RocketMovement(LocalPositionAdapter posAdapter, float moveSpeed)
    {
        positionAdapter = posAdapter;
        movementSpeed = moveSpeed;
    }

    public void Move()
    {
        positionAdapter.LocalPosition += Vector3.up * movementSpeed * Time.deltaTime;
    }
}