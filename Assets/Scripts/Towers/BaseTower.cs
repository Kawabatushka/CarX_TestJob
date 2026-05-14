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
		[Tooltip("Точка спавна снаряда")]
		[SerializeField] protected Transform m_shootStartPoint;
		[SerializeField] protected Transform m_horizontalRotatingTowerPart;
		[SerializeField] protected Transform m_verticalRotatingTowerPart;
		[Tooltip("Макс разница углов башни и напрвляющей к цели (в долях)")]
		[SerializeField] protected float m_maxCannonAngleDifferenceForShooting = 0.1f;
		protected SimpleEnemy m_currentTarget;
		protected Vector3 m_predictedPosition;
		protected Vector3 m_shootDirection;
		protected float m_lastShootTime = -1f;

		private Coroutine m_targetSearchCoroutine;
		private const float TargetSearchInterval = 0.1f;

		protected IAimingStrategy m_aimingStrategy;
		protected IRotationStrategy m_rotationStrategy;
		protected IShootingConditionStrategy m_shootingConditionStrategy;
		protected IShootingStrategy m_shootingStrategy;

		protected abstract void ConfigureStrategies();

		protected virtual void Awake()
		{
			ConfigureStrategies();
		}

		protected void Start()
		{
			m_targetSearchCoroutine = StartCoroutine(TargetSearchRoutine());
		}

		protected void Update()
		{
			if (m_currentTarget != null && m_currentTarget.isAlive)
			{
				m_aimingStrategy?.CalculateAim(m_currentTarget, m_shootStartPoint, out m_predictedPosition, out m_shootDirection);

				m_rotationStrategy?.RotateTower(m_predictedPosition, m_horizontalRotatingTowerPart, m_verticalRotatingTowerPart);

				if (m_shootingConditionStrategy != null &&
					m_shootingConditionStrategy.CanShoot(
						lastShootTime: m_lastShootTime,
						maxCannonAngleDifference: m_maxCannonAngleDifferenceForShooting,
						shootStartPointPos: m_shootStartPoint.position,
						predictedPos: m_predictedPosition,
						horizontalRotatingTowerPart: m_horizontalRotatingTowerPart,
						verticalRotatingTowerPart: m_verticalRotatingTowerPart
					))
				{
					m_shootingStrategy?.Shoot(m_shootStartPoint, m_shootDirection, m_currentTarget/* , transform.rotation */);

					m_lastShootTime = Time.time;
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

		protected abstract void FindTarget();
		//protected abstract bool CanShoot();
		//protected abstract void Shoot();
		protected void OnDrawGizmosSelected()
		{
			Gizmos.color = UnityEngine.Color.green;
			Gizmos.DrawWireSphere(transform.position, GetRangeToFindEnemy());
		}

		protected abstract float GetRangeToFindEnemy();
	}
}