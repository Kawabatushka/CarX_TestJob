using UnityEngine;
using Pooling;
using Projectile;
using Enemy;

namespace Tower
{
    public class CannonShootingStrategy : IShootingStrategy
    {
        private readonly int m_towerSettingsId;
        private readonly int m_projectileSettingsId;

        public CannonShootingStrategy(int towerSettingsId, int projectileSettingsId)
        {
            m_towerSettingsId = towerSettingsId;
            m_projectileSettingsId = projectileSettingsId;
        }

        public void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget)
        {
            if (shootStartPoint == null)
            {
                Debug.LogError($"Shoot Point = null\n{nameof(CannonShootingStrategy)}");
                return;
            }
            var poolManager = PoolManager.instance;
            if (poolManager == null)
            {
                Debug.LogError("Pool Manager = null");
                return;
            }
            var prefab = GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).projectilePrefab;
            if (prefab == null)
            {
                Debug.LogError($"Cannon Projectile Prefab = null\n{nameof(CannonShootingStrategy)}");
                return;
            }

            Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

            var projectileGameObject = poolManager.Get(prefab, shootStartPoint.position, shootRotation);
            var cannonProjectile = projectileGameObject != null ? projectileGameObject?.GetComponent<CannonProjectile>() : null;

            if (cannonProjectile != null)
            {
                var projectileSettings = GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId);
                cannonProjectile.Launch(projectileSettings.speed, projectileSettings.damage);
            }
            else
            {
                Debug.LogError($"Cannon Projectile Component = null\n{nameof(CannonShootingStrategy)}");
            }
        }
    }
}