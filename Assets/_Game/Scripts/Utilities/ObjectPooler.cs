using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPooler : Singleton<ObjectPooler>
{
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];
        GameObject objToSpawn = null;

        if (objectPool.Count > 0)
        {
            objToSpawn = objectPool.Dequeue();
            while (objToSpawn.activeSelf == true)
            {
                objToSpawn = objectPool.Dequeue();
                if (objectPool.Count == 0)
                {
                    objToSpawn = Instantiate(pools.Find(x => x.tag == tag).prefab);
                    objToSpawn.SetActive(false);
                    objectPool.Enqueue(objToSpawn);
                }
                else
                {
                    Debug.Log("Break");
                    break;
                }
            }
        }
        else
        {
            objToSpawn = Instantiate(poolDictionary[tag].Peek().gameObject);
            objToSpawn.SetActive(false);
            objectPool.Enqueue(objToSpawn);
        }

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.SetActive(true);

        objectPool.Enqueue(objToSpawn);
        
        return objToSpawn;
    }
}

