using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DirectionRetriever
{
    float Direction { get; }
}

public interface WantsToShootRetriever
{
    bool WantsToShoot { get; }
}

public class PlayerInput : DirectionRetriever, WantsToShootRetriever
{
    private float directionX = 0f;

    private float moveLeft = 0f;
    private float moveRight = 0f;

    private bool wantsToShoot = false;

    public float Direction
    {
        get { return directionX; }
    }

    public bool WantsToShoot
    {
        get { return wantsToShoot; }
    }

    public void GetInputData()
    {
        if (Input.GetKeyDown("a"))
        {
            moveLeft = -1f;
        }
        if (Input.GetKeyDown("d"))
        {
            moveRight = 1f;
        }

        if (Input.GetKeyUp("a"))
        {
            moveLeft = 0f;
        }
        if (Input.GetKeyUp("d"))
        {
            moveRight = 0f;
        }

        if (Input.GetKeyDown("space"))
        {
            wantsToShoot = true;
        }
        if (Input.GetKeyUp("space"))
        {
            wantsToShoot = false;
        }
    }

    public void BindInputDataToActions()
    {
        directionX = moveLeft + moveRight;
    }
}