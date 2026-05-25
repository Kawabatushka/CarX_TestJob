using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool : IObjectPool
    {
        private int m_elementCount;
        private GameObject m_prefab;
        private PooledObjectType m_prefabType;
        private readonly Queue<GameObject> m_pool = new(DefaultCapacity);
        private Transform m_parentObject;
        private const int DefaultCapacity = 16;

        public ObjectPool(PooledObjectType prefabType, Transform parentObj = null, int capacity = DefaultCapacity)
        {
            m_prefab = PooledObjectTypeConfig.instance.GetPrefab(prefabType);
            m_prefabType = prefabType;
            m_elementCount = capacity;
            m_parentObject = parentObj;
            if (m_prefab == null)
            {
                Debug.LogError($"{typeof(ObjectPool)} prefab is null");
                return;
            }
            if (m_elementCount < 0)
            {
                m_elementCount = DefaultCapacity;
                Debug.LogError($"{typeof(ObjectPool)} capacity is less than 0. Capacity value set 16 as default.");
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
            pooledObject.Initialize(m_prefabType);

            m_pool.Enqueue(instance);
        }

        public GameObject Get(bool isActiveInstance = true)
        {
            if (m_pool.Count == 0)
            {
                CreateElement();
            }
            var newElement = m_pool.Dequeue();
            newElement.SetActive(isActiveInstance);

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