using UnityEngine;

namespace Tower
{
    public class NullRotationStrategy : IRotationStrategy
    {
        public void RotateTower(Vector3 predictedPosition)
        {
            // реализация, как и сам поворот, отсутствует
        }
    }
}