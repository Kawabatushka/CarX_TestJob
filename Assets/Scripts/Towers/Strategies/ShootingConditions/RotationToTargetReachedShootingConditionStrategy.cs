using UnityEngine;
using Tools;

namespace Tower
{
    public class RotationToTargetReachedShootingConditionStrategy : IShootingConditionStrategy
    {
        private readonly float m_shootInterval;

        public RotationToTargetReachedShootingConditionStrategy(TowerData towerData)
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
            Vector3 predictedVector = predictedPos - shootStartPointPos;

            bool isHorizontalRotReached = Mathf.Abs(horizontalRotatingTowerPart.forward.x - predictedVector.normalized.x) <= maxCannonAngleDifference;
            bool isVerticalRotReached = Mathf.Abs(verticalRotatingTowerPart.forward.y - predictedVector.normalized.y) <= maxCannonAngleDifference;

            if (isHorizontalRotReached && isVerticalRotReached)
            {
                return Time.time >= lastShootTime + m_shootInterval;
            }
            else
            {
                return false;
            }
        }
    }
}