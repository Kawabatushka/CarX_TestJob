using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
	protected Enemy m_currentTarget;
	protected float m_lastShotTime = -1f;

	protected virtual void Update()
	{
		if (GameConfig.instance.towerSettings.cannonProjectilePrefab == null)
		{
			Debug.LogError($"Projectile Prefab не задан\n" + this.name);
			return;
		}

		FindTarget();

		if (m_currentTarget != null && CanShoot())
		{
			Shoot();
		}
	}

	protected virtual void FindTarget()
	{
		m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, GameConfig.instance.towerSettings.rangeToFindEnemy);
	}

	protected virtual bool CanShoot()
	{
		return Time.time >= m_lastShotTime + GameConfig.instance.towerSettings.shootInterval;
	}

	protected abstract void Shoot();

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = UnityEngine.Color.green;
		Gizmos.DrawWireSphere(transform.position, GameConfig.instance.towerSettings.rangeToFindEnemy);
	}
}