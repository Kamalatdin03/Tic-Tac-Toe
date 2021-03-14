using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] List<Pool> pools;
    Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Instance = this;
    }

    private void Start()
    {
        foreach (var pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.amount; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.id, objectPool);
        }
    }

    public GameObject SpawnPool(string id, Vector3 pos, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(id)) return null;

        GameObject objectToSpawn = poolDictionary[id].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;

        if (objectToSpawn.TryGetComponent<IPoolableObject>(out var poolable))
        {
            poolable.OnSpawn();
        }

        poolDictionary[id].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnPool(string id, Transform parent)
    {
        GameObject obj = SpawnPool(id, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(parent);
        return obj;
    }

    public T SpawnPool<T>(string id, Transform parent)
    {
        GameObject obj = SpawnPool(id, parent);
        return obj.GetComponent<T>();
    }

    public void DeSpawnPool(string id, GameObject obj)
    {
        if (obj.TryGetComponent<IPoolableObject>(out var poolable))
        {
            poolable.OnDeSpawn();
        }

        obj.transform.SetParent(transform);
        obj.SetActive(false);
    }

    [System.Serializable]
    sealed class Pool
    {
        public string id;
        public GameObject prefab;
        public int amount;
    }
}
