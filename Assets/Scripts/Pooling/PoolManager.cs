using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance { get; private set; }
        private const string PoolContainerName = "[Pool]";

        [SerializeField] private int m_poolsCapacity = 8;

        private readonly Dictionary<PooledObjectType, IObjectPool> m_pools = new();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public GameObject Get(PooledObjectType type, bool isActiveInstance = true, Vector3 position = default, Quaternion rotation = default)
        {
            if (!m_pools.TryGetValue(type, out var pool))
            {
                var container = new GameObject($"{PoolContainerName}_{type.ToString()}");
                container.transform.SetParent(this.transform);
                pool = new ObjectPool(type, container.transform, m_poolsCapacity);
                m_pools.Add(type, pool);
            }

            var instance = pool.Get(false);
            if (position != default && rotation != default)
            {
                instance.transform.SetPositionAndRotation(position, rotation);
            }
            instance.SetActive(isActiveInstance);
            return instance;
        }

        public void Release(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            var pooledObject = instance.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Destroy(instance);
                return;
            }

            if (m_pools.TryGetValue(pooledObject.prefabType, out var pool))
            {
                pool.Release(instance);
            }

        }
    }
}