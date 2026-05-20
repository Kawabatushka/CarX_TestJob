using UnityEngine;

namespace Tower
{
    public class GuidedTowerShootingConditionStrategy : IShootingConditionStrategy
    {
        private readonly int m_towerSettingsId;

        public GuidedTowerShootingConditionStrategy(int towerSettingsId)
        {
            m_towerSettingsId = towerSettingsId;
        }

        public bool CanShoot(
            float lastShootTime,
            float maxCannonAngleDifference,
            Vector3 shootStartPointPos,
            Vector3 predictedPos,
            Transform horizontalRotatingTowerPart = null,
            Transform verticalRotatingTowerPart = null
            )
        {
            /* if (GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
            {
                Debug.LogError($"Guided Projectile Prefab = null\n{nameof(GuidedTowerShootingConditionStrategy)}");
                return false;
            } */
            return Time.time >= lastShootTime + GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).shootInterval;
        }
    }
}