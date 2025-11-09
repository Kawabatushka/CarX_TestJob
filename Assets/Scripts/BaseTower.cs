using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
	[SerializeField] protected int m_towerSettingsId = 0;
	[SerializeField] protected int m_projectileSettingsId = 0;
	protected Enemy m_currentTarget;
	protected float m_lastShotTime = -1f;
	private GameConfig m_gameConfigInstance;

	public GameConfig gameConfigInstance
	{
		get
		{
			if (m_gameConfigInstance == null)
			{
				m_gameConfigInstance = GameConfig.instance;
			}
			return m_gameConfigInstance;
		}
	}

	protected virtual void Update()
	{

		FindTarget();
		// RotateTower();


		/// <summary> 
		/// TODO - убрать из Апдейта всё нижнее
		/// </summary>
		if (m_currentTarget != null && CanShoot())
		{
			Shoot();
		}
	}

	protected virtual void FindTarget()
	{
		m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, gameConfigInstance.towerSettings.rangeToFindEnemy);
	}

	protected virtual bool CanShoot()
	{
		return Time.time >= m_lastShotTime + gameConfigInstance.towerSettings.shootInterval;
	}

	protected abstract void Shoot();

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = UnityEngine.Color.green;
		Gizmos.DrawWireSphere(transform.position, gameConfigInstance.towerSettings.rangeToFindEnemy);
	}
}