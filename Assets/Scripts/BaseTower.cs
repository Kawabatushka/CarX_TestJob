using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
	[Header("Tower Settings")]
	[SerializeField] protected float m_shootInterval = 1f;
	[SerializeField] protected float m_rangeToFindEnemy = 15f;
	[SerializeField] protected GameObject m_projectilePrefab;

	protected Enemy m_currentTarget;
	protected float m_lastShotTime = -1f;

	protected virtual void Update()
	{
		if (m_projectilePrefab == null)
		{
			Debug.LogError($"Projectile Prefab не задан");
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
		m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, m_rangeToFindEnemy);
	}

	protected virtual bool CanShoot()
	{
		return Time.time >= m_lastShotTime + m_shootInterval;
	}

	protected abstract void Shoot();

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = UnityEngine.Color.green;
		Gizmos.DrawWireSphere(transform.position, m_rangeToFindEnemy);
	}
}