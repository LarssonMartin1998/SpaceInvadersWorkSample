using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField]
    private int numEnemiesPerRow = 7;
    [SerializeField]
    private int numEnemiesInStart = 7;
    [SerializeField]
    private float yEnemyOffset = 1f;
    [SerializeField]
    private float xEnemyOffset = 1.5f;
    [SerializeField]
    private float enemyMoveCooldown = 1f;

    private UnityAction gameOverListener;
    private UnityAction gameSceneLoadedListener;
    private UnityAction enemyDeathListener;

    private EventManager cachedEventManager;
    private ObjectPooler cachedObjectPooler;
    private Camera cachedMainCamera;

    private int numAliveEnemies = 0;
    private int numCompletedWaves = 0;
    private List<GameObject> allActiveEnemies;

    private float elapsedTime = 0f;

    private float yTopOfScreenWorld;

    bool isGameFinished = false;

    private void Awake()
    {
        gameOverListener = new UnityAction(OnGameOver);
        enemyDeathListener = new UnityAction(OnEnemyDeath);
        gameSceneLoadedListener = new UnityAction(StartNewWave);

        cachedEventManager = EventManager.instance;
        cachedObjectPooler = ObjectPooler.instance;
        cachedMainCamera = Camera.main;

        allActiveEnemies = new List<GameObject>();
    }

    private void OnEnable()
    {
        cachedEventManager.StartListening(AllEventTypes.EVENT_USER_LOST, gameOverListener);
        cachedEventManager.StartListening(AllEventTypes.EVENT_ENEMY_DIED, enemyDeathListener);
        cachedEventManager.StartListening(AllEventTypes.EVENT_GAME_SCENE_LOADED, gameSceneLoadedListener);
    }

    private void OnDisable()
    {
        cachedEventManager.StopListening(AllEventTypes.EVENT_USER_LOST, gameOverListener);
        cachedEventManager.StopListening(AllEventTypes.EVENT_ENEMY_DIED, enemyDeathListener);
        cachedEventManager.StopListening(AllEventTypes.EVENT_GAME_SCENE_LOADED, gameSceneLoadedListener);
    }

    private void Start()
    {
        Vector3 topOfScreen = new Vector3(0f, Screen.height, 0f);
        yTopOfScreenWorld = cachedMainCamera.ScreenToWorldPoint(topOfScreen).y;
    }

    private void Update()
    {
        bool canEnemyMove = (elapsedTime >= enemyMoveCooldown);

        if (canEnemyMove)
        {
            elapsedTime = 0f;
            canEnemyMove = true;
            cachedEventManager.TriggerEvent(AllEventTypes.EVENT_ENEMY_MOVE);
        }

        elapsedTime += Time.deltaTime;
    }

    private void OnGameOver()
    {
        isGameFinished = true;

        foreach (GameObject enemy in allActiveEnemies)
        {
            enemy.SetActive(false);
        }

        Debug.Log("You lost Lolz");
    }

    private void OnEnemyDeath()
    {
        numAliveEnemies--;

        if (numAliveEnemies == 0)
        {
            numCompletedWaves++;
            cachedEventManager.TriggerEvent(AllEventTypes.EVENT_COMPLETED_WAVE);
            StartNewWave();
        }
    }

    private void StartNewWave()
    {
        if (isGameFinished)
        {
            return;
        }

        allActiveEnemies.Clear();
        numAliveEnemies = numEnemiesInStart + numEnemiesPerRow * numCompletedWaves;

        uint enemiesOnRowCounter = 1;
        uint rowCount = 1;
        for (uint enemyIndex = 0; enemyIndex < numAliveEnemies; ++enemyIndex)
        {
            Vector3 enemyPosition = CalculateEnemyPositionOnSpawn(rowCount, ref enemiesOnRowCounter);
            GameObject newEnemy = cachedObjectPooler.SpawnFromPool(AllPoolTypes.POOL_ENEMIES, enemyPosition, Vector3.zero);
            allActiveEnemies.Add(newEnemy);
            enemiesOnRowCounter++;

            if (enemiesOnRowCounter > numEnemiesPerRow)
            {
                enemiesOnRowCounter = 1;
                rowCount++;
            }
        }
    }

    private Vector3 CalculateEnemyPositionOnSpawn(uint rowCount, ref uint enemiesOnRowCounter)
    {
        Vector3 enemyPosition = new Vector3();

        // First enemy should always be at 0
        if (enemiesOnRowCounter == 1)
        {
            enemyPosition.x = 0f;
        }
        else
        {
            bool numberIsEven = (enemiesOnRowCounter % 2 == 0);
            enemyPosition.x = enemiesOnRowCounter * xEnemyOffset;

            if (numberIsEven)
            {
                enemyPosition.x *= -1;
            }
            else
            {
                enemyPosition.x -= xEnemyOffset;
            }
        }

        enemyPosition.y = yTopOfScreenWorld - yEnemyOffset * rowCount;

        enemyPosition.z = 0f;

        return enemyPosition;
    }
}