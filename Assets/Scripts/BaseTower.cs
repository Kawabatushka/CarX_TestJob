using System;
using System.Collections;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
	[Tooltip("Выбор пресета из GameConfig")]
	[SerializeField] protected int m_towerSettingsId = 0;
	[Tooltip("Выбор пресета из GameConfig")]
	[SerializeField] protected int m_projectileSettingsId = 0;
	protected Enemy m_currentTarget;
	protected float m_lastShotTime = -1f;

	private Coroutine m_targetSearchCoroutine;
	private const float TargetSearchInterval = 0.1f;



	protected void Start()
	{
		m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
	}

	protected void Update()
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
			GetTarget();
			yield return new WaitForSeconds(TargetSearchInterval);
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

	protected abstract void GetTarget();

	protected virtual void RotateTower() { }

	protected abstract bool CanShoot();

	protected abstract void Shoot();

	protected void OnDrawGizmosSelected()
	{
		Gizmos.color = UnityEngine.Color.green;
		Gizmos.DrawWireSphere(transform.position, GetRangeToFindEnemy());
	}

	protected abstract float GetRangeToFindEnemy();
}