using Enemy;
using UnityEngine;

namespace Tower
{
    // пришлось разделить IRotationStrategy и IAimingStrategy, т к не все башни будут поворачиваться
    public interface IAimingStrategy
    {
        void CalculateAim(SimpleEnemy target, Transform shootStartPoint, out Vector3 predictedPosition, out Vector3 aimDirection);
    }
}