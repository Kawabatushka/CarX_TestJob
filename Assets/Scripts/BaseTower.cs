using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
	[SerializeField] protected int m_towerSettingsId = 0;
	[SerializeField] protected int m_projectileSettingsId = 0;
	protected Enemy m_currentTarget;
	protected float m_lastShotTime = -1f;

	// Добавим корутину с интервалом поиска, чтобы не вызывать EnemyManager.instance каждый кадр
	private Coroutine m_targetSearchCoroutine;
	private const float TARGET_SEARCH_INTERVAL = 0.1f;

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

	protected virtual void Start()
	{
		m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
	}

	protected virtual void Update()
	{
		if (m_currentTarget != null && m_currentTarget.isAlive)
		{
			RotateTower();

			if (CanShoot())
			{
				Shoot();
			}
		}
	}

	protected IEnumerator TargetSearchRoutine()
	{
		while (true)
		{
			FindTarget();
			yield return new WaitForSeconds(TARGET_SEARCH_INTERVAL);
		}
	}

	protected virtual void OnDisable()
	{
		// Останавливаем корутину при выключении объекта
		if (m_targetSearchCoroutine != null)
		{
			StopCoroutine(m_targetSearchCoroutine);
		}
	}

	protected virtual void FindTarget()
	{
		m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, gameConfigInstance.towerSettings.rangeToFindEnemy);
	}

	protected abstract void RotateTower();

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