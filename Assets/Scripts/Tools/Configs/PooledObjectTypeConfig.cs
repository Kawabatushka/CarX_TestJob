using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(fileName = "PooledObjectTypeConfig", menuName = "Configs/PooledObjectTypeConfig")]
    public class PooledObjectTypeConfig : ScriptableSingleton<PooledObjectTypeConfig>
    {
        [SerializeField] private List<PooledObjectMapElement> m_prefabMap;

        public GameObject GetPrefab(PooledObjectType type)
        {
            foreach (var item in m_prefabMap)
            {
                if (item.type == type && item.prefab != null)
                {
                    return item.prefab;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class PooledObjectMapElement
    {
        [SerializeField] private PooledObjectType m_type;
        [SerializeField] private GameObject m_prefab;

        public PooledObjectType type => m_type;
        public GameObject prefab => m_prefab;
    }
}