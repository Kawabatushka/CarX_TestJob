using System;
using System.Collections;
using Enemy;
using UnityEngine;

namespace Tower
{
	public abstract class BaseTower : MonoBehaviour
	{
		[Tooltip("Выбор пресета из GameConfig")]
		[SerializeField] protected int m_towerSettingsId = 0;
		[Tooltip("Выбор пресета из GameConfig")]
		[SerializeField] protected int m_projectileSettingsId = 0;
		[Tooltip("Point of head of cannon")]
		[SerializeField] protected Transform m_shootStartPoint;
		protected SimpleEnemy m_currentTarget;
		protected float m_lastShootTime = -1f;

		private IRotatable m_rotationStrategy;

		private Coroutine m_targetSearchCoroutine;
		private const float TargetSearchInterval = 0.1f;

		protected void Awake()
		{
			InitializeDependencies();
		}

		protected abstract void InitializeDependencies();

		protected void Start()
		{
			m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
		}

		public BaseTower SetRotationStrategy(IRotatable rotationStrategy) // под каждую стратегию свой сеттер
		{
			m_rotationStrategy = rotationStrategy;
			return this;
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
				FindTarget();
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

		protected void RotateTower() => m_rotationStrategy?.RotateTower();
		protected abstract void FindTarget();
		protected abstract bool CanShoot();
		protected abstract void Shoot();
		protected abstract float GetRangeToFindEnemy();

		protected void OnDrawGizmosSelected()
		{
			Gizmos.color = UnityEngine.Color.green;
			Gizmos.DrawWireSphere(transform.position, GetRangeToFindEnemy());
		}
	}
}