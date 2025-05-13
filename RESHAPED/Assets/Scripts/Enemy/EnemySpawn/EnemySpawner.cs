using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // singleton for easy access throughout the whole project
    private static EnemySpawner instance;
    public static EnemySpawner Instance { get { return instance; } }

    // private List<GameObject> asteroids = new List<GameObject>();

    public GameObject enemyPrefab;
    public float padding = 0.1f;

    public float minSpawnTime = 3;
    public float maxSpawnTime = 5;
    private float spawnTimer;

    [SerializeField] public float minForceMagnitudeTowardsCenter = 0.5f;

    [SerializeField] public float maxForceMagnitudeTowardsCenter = 1f;

    private void Awake()
    {
        // setup singleton
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        ResetTimer();
        spawnTimer = 1f;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            SpawnEnemyOffscreen();
            ResetTimer();
        }
    }

    private void SpawnEnemyOffscreen()
    {
        // instantiate new GO from prefab on position off screen
        GameObject enemy = Instantiate(enemyPrefab, GetRandomPositionOffScreen(), Quaternion.identity, transform);
        enemy.GetComponent<EnemyStats>().OnSpawn();
    }

    private void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private Vector3 GetRandomPositionOffScreen()
    {
        // get current screen size in world space
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // randomly choose which side to spawn
        int side = Random.Range(0, 4);

        // define padding as percentual screen w/h
        float paddingWidth = Screen.width * padding;
        float paddingHeight = Screen.height * padding;

        // define position vector in screen space
        Vector3 screenPosition = Vector3.zero;

        switch (side)
        {
            case 0: // top
                screenPosition = new Vector3(Random.Range(-paddingWidth, Screen.width + paddingWidth), Screen.height + paddingHeight);
                break;

            case 1: // right
                screenPosition = new Vector3(Screen.width + paddingWidth, Random.Range(-paddingHeight, Screen.height + paddingHeight));
                break;

            case 2: // bottom
                screenPosition = new Vector3(Random.Range(-paddingWidth, Screen.width + paddingWidth), -paddingHeight);
                break;

            case 3: // left
                screenPosition = new Vector3(-paddingWidth, Random.Range(-paddingHeight, Screen.height + paddingHeight));
                break;
        }

        // convert from view port space to world space
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        spawnPosition.z = 0;
        return spawnPosition;
    }
}
