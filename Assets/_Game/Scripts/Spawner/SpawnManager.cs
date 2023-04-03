using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private ObjectPooler pooler;

    [SerializeField] private string spawnablesTag = "Collectables";
    
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private int initialAmount = 5;

    protected override void Awake()
    {
        base.Awake();

        pooler = FindObjectOfType<ObjectPooler>();
    }

    private void Start()
    {
        StartSpawn();
    }

    public void StartSpawn()
    {
        var spawn = 0;

        while(spawn < initialAmount)
        {
            spawn++;
            Spawn();
        }
    }

    [ContextMenu("TestSpawn")]
    private void Spawn()
    {
        var avaliableSpawners = new List<SpawnPoint>();

        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if(!spawnPoint.hasItem)
                avaliableSpawners.Add(spawnPoint);
        }

        if (avaliableSpawners.Count <= 0)
            return;

        int index = Random.Range(0, avaliableSpawners.Count);

        var targetPoint = avaliableSpawners[index];

        targetPoint.hasItem = true;

        var go = pooler.SpawnFromPool(spawnablesTag, targetPoint.transform.position, Quaternion.identity);

        go.GetComponent<Item>().SetSpawnIndex(index);
    }

    public void AddToAvaliable(int index)
    {
        spawnPoints[index].hasItem = false;
    }
}