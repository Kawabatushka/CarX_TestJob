using UnityEngine;
using Pooling;
using Projectile;
using Enemy;

namespace Tower
{
    public class GuidedShootingStrategy : IShootingStrategy
    {
        private readonly int m_towerSettingsId;
        private readonly int m_projectileSettingsId;

        public GuidedShootingStrategy(int towerSettingsId, int projectileSettingsId)
        {
            m_towerSettingsId = towerSettingsId;
            m_projectileSettingsId = projectileSettingsId;
        }

        public void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget)
        {
            if (shootStartPoint == null)
            {
                Debug.LogError($"GuidedShootingStrategy.Shoot shootStartPoint = null\n{nameof(CannonShootingStrategy)}");
                return;
            }
            /* var poolManager = PoolManager.instance;
            if (poolManager == null)
            {
                Debug.LogError("GuidedShootingStrategy.Shoot poolManager is null");
                return;
            } */
            /* var prefab = GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).projectilePrefab;
            if (prefab == null)
            {
                Debug.LogError($"Cannon Projectile Prefab = null\n{nameof(CannonShootingStrategy)}");
                return;
            } */

            var projectileGameObject = PoolManager.instance.Get(
                GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).projectilePrefab,
                true,
                shootStartPoint.position,
                Quaternion.Euler(shootDirection));
            // TO-DO-R: удалить cannonProjectile, юзать projectileGameObject
            var guidedProjectile = projectileGameObject?.GetComponent<GuidedProjectile>() ?? null;

            if (guidedProjectile != null && currentTarget != null)
            {
                var projectileSettings = GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId);
                guidedProjectile.Launch(currentTarget.gameObject, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).speed, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).damage);
            }
        }
    }
}