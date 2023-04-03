using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    protected ObjectPooler pooler;

    [SerializeField] protected string spawnablesTag = "Enemy";

    [SerializeField] protected SpawnPoint[] spawnPoints;
    [SerializeField] private int initialAmount = 5;

    protected override void Awake()
    {
        base.Awake();

        pooler = FindObjectOfType<ObjectPooler>();
    }

    private void Start()
    {
        StartCoroutine(StartSpawn());
    }

    public IEnumerator StartSpawn()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < initialAmount; i++)
        {
            Spawn();
            Debug.Log("Alo");
        }
    }

    [ContextMenu("TestSpawn")]
    protected virtual void Spawn()
    {
        var avaliableSpawners = new List<SpawnPoint>();

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (!spawnPoint.hasItem)
                avaliableSpawners.Add(spawnPoint);
        }

        if (avaliableSpawners.Count <= 0)
            return;

        int index = Random.Range(0, avaliableSpawners.Count);

        var targetPoint = avaliableSpawners[index];

        targetPoint.hasItem = true;

        var go = pooler.SpawnFromPool(spawnablesTag, targetPoint.transform.position, Quaternion.identity);

        go.GetComponent<EnemyFollow>().SetSpawnIndex(index);
    }

    public void AddToAvaliable(EnemyFollow enemyFollow, int index)
    {
        spawnPoints[index].hasItem = false;

        enemyFollow.transform.position = spawnPoints[index].transform.position;

        //Spawn();
    }
}
