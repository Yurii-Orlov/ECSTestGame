using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections;


using Random = UnityEngine.Random;
using Unity.Transforms;


/// <summary>
/// spawns a swarm of enemy entities offscreen, encircling the player
/// </summary>
public class EnemySpawner : MonoBehaviour
{

    [Header("Spawner")]
    [SerializeField] private GameObject _enemyPrefab;

    // number of enemies generated per interval
    [SerializeField] private int spawnCount = 30;

    // time between spawns
    [SerializeField] private float spawnInterval = 3f;

    // enemies spawned on a circle of this radius
    [SerializeField] private float spawnRadius = 30f;

    // extra enemy increase each wave
    [SerializeField] private int difficultyBonus = 5;

    [Header("Enemy")]
    // random speed range
    [SerializeField] float minSpeed = 4f;
    [SerializeField] float maxSpeed = 12f;

    // counter
    private float spawnTimer;
    private EntityManager _entityManager;
    private Entity _enemyEntityPrefab;

    // flag from GameManager to enable spawning
    private bool canSpawn;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyPrefab, settings);

        SpawnWave();
    }

    // spawns enemies in a ring around the player
    private void SpawnWave()
    {
        NativeArray<Entity> enemyArray = new NativeArray<Entity>(spawnCount, Allocator.Temp);

        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i] = _entityManager.Instantiate(_enemyEntityPrefab);

            _entityManager.SetComponentData(enemyArray[i], new Translation { Value = RandomPointOnCircle(spawnRadius) });
            _entityManager.SetComponentData(enemyArray[i], new EnemyMoveForward { Speed = Random.Range(5f, 10f) });
        }
    }

    // get a random point on a circle with given radius
    private float3 RandomPointOnCircle(float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * radius;

        // return random point on circle, centered around the player position
        return new float3(randomPoint.x, 0.5f, randomPoint.y) + (float3)GameManager.GetPlayerPosition();
    }

    // signal from GameManager to begin spawning
    public void StartSpawn()
    {
        canSpawn = true;
    }

    private void Update()
    {
        // disable if the game has just started or if player is dead
        if (!canSpawn || GameManager.IsGameOver())
        {
            return;
        }

        // count up until next spawn
        spawnTimer += Time.deltaTime;

        // spawn and reset timer
        if (spawnTimer > spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0;
        }
    }
}
