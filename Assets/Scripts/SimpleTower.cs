using UnityEngine;
using System.Collections;

public class SimpleTower : BaseTower
{
	protected override void Shoot()
	{
		var projectile = Instantiate(GameConfig.instance.towerSettings.guidedProjectilePrefab,
			transform.position + GameConfig.instance.guidedProjectileSettings.spawnOffset,
			Quaternion.identity);

		var guidedProjectile = projectile.GetComponent<GuidedProjectile>();
		if (guidedProjectile != null && m_currentTarget != null)
		{
			guidedProjectile.Launch(m_currentTarget.gameObject, GameConfig.instance.baseProjectileSettings.speed, 0);
		}

		m_lastShotTime = Time.time;
	}
}