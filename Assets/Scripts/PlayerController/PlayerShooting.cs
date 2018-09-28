using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting
{
    private LocalPositionAdapter positionAdapter;
    private WantsToShootRetriever wantsToShootRetriever;
    private ObjectPooler cachedObjectPooler;

    private float rocketCooldown;
    private bool canShoot = true;
    private float elapsedTime = 0f;

    private float yRocketOffset = 1f;

    public PlayerShooting(LocalPositionAdapter posAdapter, WantsToShootRetriever shootRetriever, float rocketCd)
    {
        positionAdapter = posAdapter;
        wantsToShootRetriever = shootRetriever;
        cachedObjectPooler = ObjectPooler.instance;

        rocketCooldown = rocketCd;
    }

    public void AttemptToShoot()
    {
        if (canShoot && wantsToShootRetriever.WantsToShoot)
        {
            Shoot();
        }
    }

    public void HandleCooldown()
    {
        if (canShoot)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= rocketCooldown)
        {
            elapsedTime = 0f;
            canShoot = true;
        }
    }

    private void Shoot()
    {
        Vector3 rocketPosition = positionAdapter.LocalPosition + Vector3.up * yRocketOffset;

        cachedObjectPooler.SpawnFromPool(AllPoolTypes.POOL_ROCKETS, rocketPosition, Vector3.zero);
        canShoot = false;
    }

}