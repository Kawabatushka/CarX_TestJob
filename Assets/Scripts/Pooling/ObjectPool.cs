using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool : IObjectPool
    {
        private int m_elementCount;
        private Component m_prefab;
        private Queue<Component> m_pool = new(DEFAULT_CAPACITY);
        private const int DEFAULT_CAPACITY = 16;

        public ObjectPool(Component prefab, int capacity = DEFAULT_CAPACITY)
        {
            m_prefab = prefab;
            m_elementCount = capacity;
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
            var newElement = UnityEngine.Object.Instantiate(m_prefab);
            newElement.gameObject.SetActive(false);
            m_pool.Enqueue(newElement);
        }

        public Component Get()
        {
            if(m_pool.Count==0)
            {
                CreateElement();
            }
            var newElement = m_pool.Dequeue();
            newElement.gameObject.SetActive(true);
            return newElement;
        }

        public void Release(Component element)
        {
            element.gameObject.SetActive(false);
            m_pool.Enqueue(element);
        }
    }
}