using UnityEngine;
using System.Collections;

public class SimpleTower : BaseTower
{
	[Header("Simple Tower Settings")]
	[SerializeField] private Vector3 m_projectileSpawnOffset = new Vector3(0, 1.7f, 0);

	protected override void Shoot()
	{
		var projectile = Instantiate(m_projectilePrefab,
			transform.position + m_projectileSpawnOffset,
			Quaternion.identity);

		var guidedProjectile = projectile.GetComponent<GuidedProjectile>();
		if (guidedProjectile != null && m_currentTarget != null)
		{
			guidedProjectile.Launch(m_currentTarget.gameObject);
		}

		m_lastShotTime = Time.time;
	}
}