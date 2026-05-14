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

        // TO-DO-R: вместо towerRotation юзать shootDirection, просто перевести его в Quaternion
        // тогда будет -1 параметр и + гибкость для стратегий стрельбы
        public void Shoot(Transform shootStartPoint, Vector3 shootDirection, SimpleEnemy currentTarget/* , Quaternion towerRotation = default */)
        {
            if (shootStartPoint == null)
            {
                Debug.LogError($"Shoot Point = null\n{nameof(CannonShootingStrategy)}");
                return;
            }
            var poolManager = PoolManager.instance;
            if (poolManager == null)
            {
                Debug.LogError("PoolManager = null");
                return;
            }
            var prefab = GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).projectilePrefab;
            if (prefab == null)
            {
                Debug.LogError($"Cannon Projectile Prefab = null\n{nameof(CannonShootingStrategy)}");
                return;
            }

            var projectileGameObject = poolManager.Get(prefab, shootStartPoint.position, Quaternion.Euler(shootDirection));
            GuidedProjectile guidedProjectile = projectileGameObject != null ? projectileGameObject.GetComponent<GuidedProjectile>() : null;

            if (guidedProjectile != null && currentTarget != null)
            {
                var projectileSettings = GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId);
                guidedProjectile.Launch(currentTarget.gameObject, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).speed, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).damage);
            }

            //m_lastShootTime = Time.time;
        }
    }
}