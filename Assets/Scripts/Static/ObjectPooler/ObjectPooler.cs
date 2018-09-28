using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface PrefabInstantiater
{
    GameObject InstantiatePrefab(GameObject obj);
}

public class ObjectPooler : MonoBehaviour, PrefabInstantiater
{
    public static ObjectPooler instance;

    public List<PoolType> pools;

    private EventManager cachedEventManager;
    private UnityAction gameSceneLoadedListener;
    private ObjectPoolLogic poolLogic;

    public GameObject InstantiatePrefab(GameObject obj)
    {
        return Instantiate(obj);
    }

    private void Awake()
    {
        cachedEventManager = EventManager.instance;
        gameSceneLoadedListener = new UnityAction(OnGameFinishLoading);
        InitializeThisSingleton();

        poolLogic = new ObjectPoolLogic(this);
    }

    private void OnEnable()
    {
        cachedEventManager.StartListening(AllEventTypes.EVENT_GAME_SCENE_LOADED, gameSceneLoadedListener);
    }

    private void OnDisable()
    {
        cachedEventManager.StopListening(AllEventTypes.EVENT_GAME_SCENE_LOADED, gameSceneLoadedListener);
    }

    private void OnGameFinishLoading()
    {
        poolLogic.Initialize("Object Pools", pools);
    }

    private void InitializeThisSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        // Means that there already is an instance of this singleton and were trying to create another.
        else if (instance != null)
        {
            Debug.Log("Tried to initialize an already created singleton: \"ObjectPooler\"");
            Destroy(gameObject);
        }
    }

    public GameObject SpawnFromPool(AllPoolTypes type, Vector3 position, Vector3 rotation)
    {
        GameObject objToSpawn = poolLogic.GetObjectInQueue(type);
        Quaternion qRot = Quaternion.Euler(rotation);
        objToSpawn.transform.SetPositionAndRotation(position, qRot);
        objToSpawn.SetActive(true);

        return objToSpawn;
    }

    public GameObject SpawnFromPool(AllPoolTypes type)
    {
        GameObject objToSpawn = poolLogic.GetObjectInQueue(type);
        objToSpawn.SetActive(true);

        return objToSpawn;
    }

    public void AddPool(PoolType pool)
    {
        poolLogic.LoadPool(pool);
    }
}