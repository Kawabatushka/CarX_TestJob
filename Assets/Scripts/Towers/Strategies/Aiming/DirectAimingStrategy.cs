using Enemy;
using UnityEngine;

namespace Tower
{
    public class DirectAimingStrategy : IAimingStrategy
    {
        public void CalculateAim(SimpleEnemy target, Transform shootStartPoint, out Vector3 predictedPosition, out Vector3 aimDirection)
        {
            if (target == null || shootStartPoint == null)
            {
                predictedPosition = Vector3.zero;
                aimDirection = Vector3.zero;
                return;
            }

            predictedPosition = target.transform.position;
            aimDirection = (predictedPosition - shootStartPoint.position).normalized;
        }
    }
}