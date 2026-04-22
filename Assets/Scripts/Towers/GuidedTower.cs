using UnityEngine;
using Projectile;
using Enemy;
using Pooling;

namespace Tower
{
	public class GuidedTower : BaseTower
	{
		protected override void FindTarget()
		{
			m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).rangeToFindEnemy);
		}

		protected override bool CanShoot()
		{
			if (GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
			{
				Debug.LogError($"Guided Projectile Prefab не задан\n" + this.name);
				return false;
			}
			return Time.time >= m_lastShootTime + GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).shootInterval;
		}

		protected override void Shoot()
		{
			var poolManager = GuidedProjectilePoolManager.instance;
			if (poolManager == null)
			{
				Debug.LogError("PoolManager = null");
				return;
			}
			GuidedProjectile guidedProjectile = (GuidedProjectile)poolManager.Get(transform.position, transform.rotation);

			if (guidedProjectile != null && m_currentTarget != null)
			{
				guidedProjectile.Launch(m_currentTarget.gameObject, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).speed, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).damage);
			}

			m_lastShootTime = Time.time;
		}

		protected override float GetRangeToFindEnemy()
		{
			return GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).rangeToFindEnemy;
		}
	}
}