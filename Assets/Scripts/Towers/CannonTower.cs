using UnityEngine;
using Projectile;
using Enemy;
using Pooling;

namespace Tower
{
	public class CannonTower : BaseTower
	{
		[SerializeField] protected Transform m_horizontalRotatingTowerPart;
		[SerializeField] protected Transform m_verticalRotatingTowerPart;

		[Tooltip("Max angles difference for a shot (in fractions)")]
		[SerializeField] private float m_maxCannonAngleDifference = 0.1f;

		private Quaternion shootRotation;
		private Vector3 m_shootDirection;
		private Vector3 m_predictedPosition;
		private float timeToTarget;

		protected override void FindTarget()
		{
			m_currentTarget = EnemyManager.instance.GetClosestEnemy(transform.position, GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rangeToFindEnemy);
		}

		protected override bool CanShoot()
		{
			if (GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
			{
				Debug.LogError($"Cannon Projectile Prefab не задан\n" + this.name);
				return false;
			}

			m_shootDirection = CalculateShootDirection();

			if (IsReachedRotation(m_maxCannonAngleDifference))
			{
				return Time.time >= m_lastShootTime + GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).shootInterval;
			}
			else
			{
				return false;
			}
		}
		private bool IsReachedRotation(float maxAngleDifference)
		{
			Vector3 predictedVector = m_predictedPosition - m_shootStartPoint.position;

			bool isHorizontalRotReached = Mathf.Abs(m_horizontalRotatingTowerPart.forward.x - predictedVector.normalized.x) <= maxAngleDifference;
			bool isVerticalRotReached = Mathf.Abs(m_verticalRotatingTowerPart.forward.y - predictedVector.normalized.y) <= maxAngleDifference;

			return isHorizontalRotReached && isVerticalRotReached;
		}

		protected override void RotateTower()
		{
			#region Горизонтальный поворот
			// Направление, куда надо повернуть пушку
			Vector3 horizontalDirectionToTarget = m_predictedPosition - m_horizontalRotatingTowerPart.position;

			// Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
			if (horizontalDirectionToTarget != Vector3.zero)
			{
				// Игнорируем разницу по Y для горизонтального вращения
				horizontalDirectionToTarget.y = 0;

				// Из направления получаем поворот
				Quaternion targetHorizontalRotation = Quaternion.LookRotation(horizontalDirectionToTarget);

				// Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
				m_horizontalRotatingTowerPart.rotation = Quaternion.RotateTowards(
					m_horizontalRotatingTowerPart.rotation,
					targetHorizontalRotation,
					GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
				);
			}
			#endregion

			#region Вертикальный поворот
			// Направление, куда надо повернуть пушку
			Vector3 verticalDirectionToTarget = m_predictedPosition - m_verticalRotatingTowerPart.position;

			// Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
			if (verticalDirectionToTarget != Vector3.zero)
			{
				// Из направления получаем поворот
				Quaternion targetVerticalRotation = Quaternion.LookRotation(verticalDirectionToTarget);

				// Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
				m_verticalRotatingTowerPart.rotation = Quaternion.RotateTowards(
					m_verticalRotatingTowerPart.rotation,
					targetVerticalRotation,
					GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
				);
			}
			#endregion
		}

		protected override void Shoot()
		{
			if (m_shootStartPoint == null)
			{
				Debug.LogError("Shoot Point не задан");
				return;
			}

			shootRotation = Quaternion.LookRotation(m_shootDirection);

			var poolManager = PoolManager.instance;
			if (poolManager == null)
			{
				Debug.LogError("PoolManager = null");
				return;
			}
			var prefab = GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).projectilePrefab;
			var projectileGameObject = poolManager.Get(prefab, m_shootStartPoint.position, shootRotation);
			var cannonProjectile = projectileGameObject != null ? projectileGameObject?.GetComponent<CannonProjectile>() : null;

			if (cannonProjectile != null)
			{
				cannonProjectile.Launch(GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed,
					GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).damage);
			}
			else
			{
				Debug.LogError($"Cannon projectile component = null");
			}

			m_lastShootTime = Time.time;
		}

		private Vector3 CalculateShootDirection()
		{
			Vector3 toTarget = m_currentTarget.transform.position - m_shootStartPoint.position;
			if (m_currentTarget.velocity.magnitude < 0.1f)
			{
				return toTarget.normalized;
			}

			#region Решение квадратного уравнения для точного расчета
			float a = Vector3.Dot(m_currentTarget.velocity, m_currentTarget.velocity) -
				Mathf.Pow(GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed, 2);
			float b = 2f * Vector3.Dot(m_currentTarget.velocity, toTarget);
			float c = Vector3.Dot(toTarget, toTarget);

			float discriminant = b * b - 4f * a * c;

			if (discriminant < 0)
			{
				// Нет решения - цель слишком быстрая, стреляем прямо
				return toTarget.normalized;
			}

			float time1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
			float time2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

			timeToTarget = Mathf.Max(time1, time2);
			#endregion

			if (timeToTarget < 0)
			{
				return toTarget.normalized;
			}

			m_predictedPosition = m_currentTarget.transform.position + m_currentTarget.velocity * timeToTarget;
			return (m_predictedPosition - m_shootStartPoint.position).normalized;
		}

		protected override float GetRangeToFindEnemy()
		{
			return GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rangeToFindEnemy;
		}

		private void OnDrawGizmos()
		{
			// Направляющий вектор пушки
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(m_verticalRotatingTowerPart.position, m_verticalRotatingTowerPart.position + m_verticalRotatingTowerPart.forward * 10f);

			if (m_currentTarget != null && m_shootStartPoint != null)
			{
				// Линия к текущей позиции цели
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(m_shootStartPoint.position, m_currentTarget.transform.position);

				// Точка - предсказанная позиция для выстрела
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(m_predictedPosition, 0.2F);
				// Направляющий вектор пушки
				Gizmos.color = Color.red;
				Gizmos.DrawLine(m_shootStartPoint.position, m_predictedPosition);
			}
		}
	}
}