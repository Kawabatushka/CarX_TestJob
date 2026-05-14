using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance { get; private set; }
        private const string PoolContainerName = "[Pool]";

        [SerializeField] private int m_poolsCapacity = 8;

        private readonly Dictionary<GameObject, IObjectPool> m_pools = new();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public GameObject Get(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            if (prefab == null)
            {
                Debug.LogError("PoolManager.Get prefab is null", this);
                return null;
            }

            if (!m_pools.TryGetValue(prefab, out var pool))
            {
                var container = new GameObject($"{PoolContainerName}_{prefab.name}");
                container.transform.SetParent(this.transform);
                pool = new ObjectPool(prefab, container.transform, m_poolsCapacity);
                m_pools.Add(prefab, pool);
            }

            var instance = pool.Get();
            if (position != default && rotation != default)
            {
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            return instance;
        }

        public void Release(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            var pooledObject = instance.GetComponent<PooledObject>();
            if (pooledObject == null || pooledObject.prefabKey == null)
            {
                Destroy(instance);
                return;
            }

            if (!m_pools.TryGetValue(pooledObject.prefabKey, out var pool))
            {
                var container = new GameObject($"{PoolContainerName}_{pooledObject.prefabKey.name}");
                container.transform.SetParent(this.transform);
                pool = new ObjectPool(pooledObject.prefabKey, container.transform, m_poolsCapacity);
                m_pools.Add(pooledObject.prefabKey, pool);
            }

            pool.Release(instance);
        }
    }
}