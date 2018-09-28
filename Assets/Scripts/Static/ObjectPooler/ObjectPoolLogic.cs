using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolLogic
{
    private PrefabInstantiater prefabInstantiater;
    private GameObject poolerTitleInHierarchy;
    private Dictionary<AllPoolTypes, Queue<GameObject>> poolDictionary;

    public ObjectPoolLogic(PrefabInstantiater instantiater)
    {
        prefabInstantiater = instantiater;
    }

    public void Initialize(string objectPoolerTitle, List<PoolType> pools)
    {
        poolerTitleInHierarchy = new GameObject(objectPoolerTitle);

        LoadDictionary(pools);
    }

    private void LoadDictionary(List<PoolType> pools)
    {
        poolDictionary = new Dictionary<AllPoolTypes, Queue<GameObject>>();

        foreach(PoolType pool in pools)
        {
            LoadPool(pool);
        }
    }

    public void LoadPool(PoolType pool)
    {
        Queue<GameObject> objQueue = new Queue<GameObject>();

        GameObject groupTitle = new GameObject(pool.groupName);
        groupTitle.transform.SetParent(poolerTitleInHierarchy.transform);

        for (uint objIndex = 0; objIndex < pool.amount; ++objIndex)
        {
            GameObject prefabObj = prefabInstantiater.InstantiatePrefab(pool.prefab);
            prefabObj.transform.SetParent(groupTitle.transform);
            prefabObj.SetActive(false);
            objQueue.Enqueue(prefabObj);
        }

        poolDictionary.Add(pool.type, objQueue);
    }

    public GameObject GetObjectInQueue(AllPoolTypes type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.Log("poolDictionary in ObjectPoolLogic doesn't contain any pool of key: " + type.ToString() + ", see AllPoolTypes to see what type that is.");
            return null;
        }

        // Gets a reference to the obj furthest in the queue, then put it back into the queue.
        GameObject objFromQueue = poolDictionary[type].Dequeue();
        poolDictionary[type].Enqueue(objFromQueue);

        return objFromQueue;
    }
}