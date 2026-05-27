using System.Collections;
using UnityEngine;
using Enemy;
using Tools;
using Pooling;

namespace Tower
{
	public class BaseTower : MonoBehaviour, IPoolable
	{
		#region  Fields
		private const float TargetSearchInterval = 0.1f;
		[Tooltip("Point for projectile spawn")]
		[SerializeField] private Transform m_shootStartPoint;
		[SerializeField] private Transform m_horizontalRotatingTowerPart;
		[SerializeField] private Transform m_verticalRotatingTowerPart;
		private SimpleEnemy m_currentTarget;
		private Vector3 m_predictedPosition;
		private Vector3 m_shootDirection;
		private float m_lastShootTime = -1f;
		private TowerData m_settings;
		private Coroutine m_targetSearchCoroutine;

		private ITargetFindingStrategy m_targetFindingStrategy;
		private IAimingStrategy m_aimingStrategy;
		private IRotationStrategy m_rotationStrategy;
		private IShootingConditionStrategy m_shootingConditionStrategy;
		private IShootingStrategy m_shootingStrategy;
		#endregion

		#region Setting strategies
		public BaseTower SetTargetFindingStrategy(ITargetFindingStrategy targetFindingStrategy)
		{
			m_targetFindingStrategy = targetFindingStrategy;
			return this;
		}
		public BaseTower SetAimingStrategy(IAimingStrategy aimingStrategy)
		{
			m_aimingStrategy = aimingStrategy;
			return this;
		}
		public BaseTower SetRotationStrategy(IRotationStrategy rotationStrategy)
		{
			m_rotationStrategy = rotationStrategy;
			return this;
		}
		public BaseTower SetShootingConditionStrategy(IShootingConditionStrategy shootingConditionStrategy)
		{
			m_shootingConditionStrategy = shootingConditionStrategy;
			return this;
		}
		public BaseTower SetShootingStrategy(IShootingStrategy shootingStrategy)
		{
			m_shootingStrategy = shootingStrategy;
			return this;
		}
		#endregion

		public void Initialize(TowerData towerData)
		{
			m_settings = towerData;
			TowerStrategyFactory.Configure(this, towerData);
			m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
		}

		private void Update()
		{
			if (m_currentTarget != null && m_currentTarget.isAlive)
			{
				m_aimingStrategy?.CalculateAim(m_currentTarget, m_shootStartPoint, out m_predictedPosition, out m_shootDirection);

				m_rotationStrategy?.RotateTower(m_predictedPosition, m_horizontalRotatingTowerPart, m_verticalRotatingTowerPart);

				if (m_shootingConditionStrategy != null &&
					m_shootingConditionStrategy.CanShoot(
						lastShootTime: m_lastShootTime,
						maxCannonAngleDifference: m_settings.maxCannonAngleDifferenceForShooting,
						shootStartPointPos: m_shootStartPoint.position,
						predictedPos: m_predictedPosition,
						horizontalRotatingTowerPart: m_horizontalRotatingTowerPart,
						verticalRotatingTowerPart: m_verticalRotatingTowerPart
					))
				{
					m_shootingStrategy?.Shoot(m_shootStartPoint, m_shootDirection, m_currentTarget);

					m_lastShootTime = Time.time;
				}
			}
		}

		public void OnSpawned()
		{
			if (m_settings == null)
			{
				return;
			}
			m_lastShootTime = -1f;
			m_currentTarget = null;
			m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
		}
		public void OnDespawned()
		{
			StopTargetSearch();
			m_lastShootTime = -1f;
			m_currentTarget = null;
		}

		private IEnumerator TargetSearchRoutine()
		{
			var coolDown = new WaitForSeconds(TargetSearchInterval);
			while (true)
			{
				m_currentTarget = m_targetFindingStrategy?.GetTarget(transform.position, GetRangeToFindEnemy()) ?? null;
				yield return coolDown;
			}
		}

		private void OnDisable()
		{
			// stop the routine when the object is turned off
			StopTargetSearch();
		}

		private void StopTargetSearch()
		{
			if (m_targetSearchCoroutine == null)
				return;

			StopCoroutine(m_targetSearchCoroutine);
			m_targetSearchCoroutine = null;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = UnityEngine.Color.green;
			Gizmos.DrawWireSphere(transform.position, GetRangeToFindEnemy());
		}

		private float GetRangeToFindEnemy()
		{
			return m_settings?.rangeToFindEnemy ?? 0f;
		}
	}
}