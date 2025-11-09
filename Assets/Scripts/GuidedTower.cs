using UnityEngine;
using System.Collections;

/// <summary>
/// TODO - переименовать класс в ProjectileTower
/// </summary>
public class GuidedTower : BaseTower
{
	protected override bool CanShoot()
	{
		if (gameConfigInstance.GetGuidedTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
		{
			Debug.LogError($"Guided Projectile Prefab не задан\n" + this.name);
			return false;
		}
		return base.CanShoot();
	}

	protected override void Shoot()
	{
		var projectile = Instantiate(GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).projectilePrefab,
			transform.position + GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).spawnOffset,
			Quaternion.identity);

		var guidedProjectile = projectile.GetComponent<GuidedProjectile>();
		if (guidedProjectile != null && m_currentTarget != null)
		{
			guidedProjectile.Launch(m_currentTarget.gameObject, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).speed, GameConfig.instance.GetGuidedProjectileSettings(m_projectileSettingsId).damage);
		}

		m_lastShotTime = Time.time;
	}
}