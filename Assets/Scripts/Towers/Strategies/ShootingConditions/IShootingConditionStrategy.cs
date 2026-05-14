using UnityEngine;

namespace Tower
{
    public interface IShootingConditionStrategy
    {
        bool CanShoot(
            float lastShootTime,
            float maxCannonAngleDifference,
            Vector3 shootStartPointPos,
            Vector3 predictedPos,
            Transform horizontalRotatingTowerPart = null,
            Transform verticalRotatingTowerPart = null
            );
    }
}