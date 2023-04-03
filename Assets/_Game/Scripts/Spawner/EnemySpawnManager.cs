using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : SpawnManager
{
    protected override void Spawn()
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

        go.GetComponent<EnemyFollow>(); //.SetSpawnIndex(index);
    }
}
