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
            return Time.time >= lastShootTime + GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).shootInterval;
        }
    }
}