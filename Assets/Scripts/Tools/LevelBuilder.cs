using System;
using System.Collections.Generic;
using UnityEngine;
using Pooling;
using Tower;

namespace Tools
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private List<TowerSpawnData> m_towers;
        public List<TowerSpawnData> towers => m_towers;

        private void Awake()
        {
            foreach (var item in m_towers)
            {
                var towerPreset = TowersDataConfig.instance.towerPresets[item.towerPresetId];
                var tower = PoolManager.instance.Get(
                    towerPreset.towerType,
                    true,
                    item.position,
                    Quaternion.Euler(item.rotation));

                if (tower.TryGetComponent<BaseTower>(out var baseTowerComponent))
                {
                    baseTowerComponent.Initialize(towerPreset);
                }
            }
        }
    }

    [Serializable]
    public class TowerSpawnData
    {
        [SerializeField] private int m_towerPresetId;
        [SerializeField] private Vector3 m_position = Vector3.zero;
        [SerializeField] private Vector3 m_rotation = Vector3.zero;

        public int towerPresetId => m_towerPresetId;
        public Vector3 position => m_position;
        public Vector3 rotation => m_rotation;
    }
}