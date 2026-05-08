using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool : IObjectPool
    {
        private int m_elementCount;
        private GameObject m_prefab;
        private readonly Queue<GameObject> m_pool = new(DEFAULT_CAPACITY);
        private Transform m_parentObject;
        private const int DEFAULT_CAPACITY = 16;

        public ObjectPool(GameObject prefab, Transform parentObj = null, int capacity = DEFAULT_CAPACITY)
        {
            m_prefab = prefab;
            m_elementCount = capacity;
            m_parentObject = parentObj;
            if (m_prefab == null)
            {
                Debug.LogError("ObjectPool prefab is null");
                return;
            }
            if (m_elementCount < 0)
            {
                m_elementCount = DEFAULT_CAPACITY;
                Debug.LogError("ObjectPool capacity is less than 0. Capacity value set 16 as default.");
            }

            for (int i = 0; i < m_elementCount; i++)
            {
                CreateElement();
            }
        }
        private void CreateElement()
        {
            var instance = UnityEngine.Object.Instantiate(m_prefab, m_parentObject);
            instance.SetActive(false);

            var pooledObject = instance.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                pooledObject = instance.AddComponent<PooledObject>();
            }
            pooledObject.Initialize(m_prefab);

            m_pool.Enqueue(instance);
        }

        public GameObject Get()
        {
            if (m_pool.Count == 0)
            {
                CreateElement();
            }
            var newElement = m_pool.Dequeue();
            newElement.SetActive(true);

            if (newElement.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnSpawned();
            }
            return newElement;
        }

        public void Release(GameObject element)
        {
            if (element.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnDespawned();
            }
            element.SetActive(false);
            m_pool.Enqueue(element);
        }
    }
}