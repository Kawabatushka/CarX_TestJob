using UnityEngine;
using Pooling;
using Projectile;
using Enemy;
using Tools;

namespace Tower
{
    public class PredictedShootingStrategy : IShootingStrategy
    {
        private readonly PooledObjectType m_projectilePrefabType;
        private readonly float m_projectileSpeed;
        private readonly int m_projectileDamage;

        public PredictedShootingStrategy(TowerData towerData)
        {
            m_projectilePrefabType = towerData.projectilePrefabType;
            m_projectileSpeed = towerData.projectileSpeed;
            m_projectileDamage = towerData.projectileDamage;
        }

        public void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget)
        {
            if (shootStartPoint == null)
            {
                Debug.LogError($"{typeof(PredictedShootingStrategy)}.Shoot shootStartPoint is null\n{nameof(PredictedShootingStrategy)}");
                return;
            }

            Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

            var projectileGameObject = PoolManager.instance.Get(
                m_projectilePrefabType,
                true,
                shootStartPoint.position,
                shootRotation);

            if (projectileGameObject?.GetComponent<CannonProjectile>() != null)
            {
                projectileGameObject.GetComponent<CannonProjectile>().Launch(m_projectileSpeed, m_projectileDamage);
            }
            else
            {
                Debug.LogError($"{typeof(PredictedShootingStrategy)}.Shoot cannonProjectile is null\n{nameof(PredictedShootingStrategy)}");
            }
        }
    }
}