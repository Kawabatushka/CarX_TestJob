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
            // implementation, like rotation, is empty
        }
    }
}