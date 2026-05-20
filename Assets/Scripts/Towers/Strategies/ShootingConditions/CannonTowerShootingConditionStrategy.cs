using UnityEngine;

namespace Tower
{
    public class CannonTowerShootingConditionStrategy : IShootingConditionStrategy
    {
        private readonly int m_towerSettingsId;

        public CannonTowerShootingConditionStrategy(int towerSettingsId)
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
            Vector3 predictedVector = predictedPos - shootStartPointPos;

            bool isHorizontalRotReached = Mathf.Abs(horizontalRotatingTowerPart.forward.x - predictedVector.normalized.x) <= maxCannonAngleDifference;
            bool isVerticalRotReached = Mathf.Abs(verticalRotatingTowerPart.forward.y - predictedVector.normalized.y) <= maxCannonAngleDifference;

            if (isHorizontalRotReached && isVerticalRotReached)
            {
                return Time.time >= lastShootTime + GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).shootInterval;
            }
            else
            {
                return false;
            }
        }
    }
}