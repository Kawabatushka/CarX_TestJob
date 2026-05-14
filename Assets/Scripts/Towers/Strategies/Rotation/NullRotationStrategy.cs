using UnityEngine;

namespace Tower
{
    public class NullRotationStrategy : IRotationStrategy
    {
        public void RotateTower(
            Vector3 predictedPosition = default,
            Transform horizontalRotatingTowerPart = null,
            Transform verticalRotatingTowerPart = null
            )
        {
            // реализация, как и сам поворот, отсутствует
        }
    }
}