using UnityEngine;
using Enemy;

namespace Tower
{
    public interface IShootingStrategy
    {
        void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget/* , Quaternion towerRotation = default */);
    }
}