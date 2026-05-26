using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(fileName = "PooledObjectTypeConfig", menuName = "Configs/PooledObjectTypeConfig")]
    //public class PooledObjectTypeConfig : ScriptableSingleton<PooledObjectTypeConfig>
    public class PooledObjectTypeConfig : ScriptableObject
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

		private static PooledObjectTypeConfig m_instance;
		public static PooledObjectTypeConfig instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = Resources.Load<PooledObjectTypeConfig>("PooledObjectTypeConfig");
					if (m_instance == null)
					{
						Debug.LogError($"{nameof(PooledObjectTypeConfig)}.asset не найден в папке Resources");
					}
				}
				return m_instance;
			}
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