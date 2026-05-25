using UnityEngine;
using Tools;

namespace Tower
{
    public class OnlyCooldownManagedShootingConditionStrategy : IShootingConditionStrategy
    {
        private readonly float m_shootInterval;

        public OnlyCooldownManagedShootingConditionStrategy(TowerData towerData)
        {
            m_shootInterval = towerData.shootInterval;
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
            return Time.time >= lastShootTime + m_shootInterval;
        }
    }
}