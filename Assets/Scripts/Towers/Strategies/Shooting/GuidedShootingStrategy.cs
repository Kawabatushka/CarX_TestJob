using UnityEngine;
using Pooling;
using Projectile;
using Enemy;
using Tools;

namespace Tower
{
    public class GuidedShootingStrategy : IShootingStrategy
    {
        private readonly PooledObjectType m_projectilePrefabType;
        private readonly float m_projectileSpeed;
        private readonly int m_projectileDamage;

        public GuidedShootingStrategy(TowerData towerData)
        {
            m_projectilePrefabType = towerData.projectilePrefabType;
            m_projectileSpeed = towerData.projectileSpeed;
            m_projectileDamage = towerData.projectileDamage;
        }

        public void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget)
        {
            if (shootStartPoint == null)
            {
                Debug.LogError($"{nameof(GuidedShootingStrategy)}.{nameof(Shoot)} shootStartPoint = null\n{nameof(GuidedShootingStrategy)}");
                return;
            }

            var projectileGameObject = PoolManager.instance.Get(
                m_projectilePrefabType,
                true,
                shootStartPoint.position,
                Quaternion.Euler(shootDirection));

            if (projectileGameObject?.GetComponent<GuidedProjectile>() != null && currentTarget != null)
            {
                projectileGameObject.GetComponent<GuidedProjectile>().Launch(
                    currentTarget.gameObject,
                    m_projectileSpeed,
                    m_projectileDamage);
            }
            else
            {
                Debug.LogError($"{nameof(GuidedShootingStrategy)}.{nameof(Shoot)} cannonProjectile is null\n{nameof(GuidedShootingStrategy)}");
            }
        }
    }
}