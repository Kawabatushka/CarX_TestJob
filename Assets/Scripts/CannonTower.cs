using System.Collections;
using UnityEngine;

public class CannonTower : BaseTower
{
	[Tooltip("Point of head of cannon")]
	[SerializeField] private Transform m_shootStartPoint;
	[SerializeField] protected Transform m_horizontalRotatingTowerPart;
	[SerializeField] protected Transform m_verticalRotatingTowerPart;
	private Quaternion shootRotation;
	private Vector3 m_shootDirection;
	private Vector3 m_predictedPosition;
	private float timeToTarget;

	protected override bool CanShoot()
	{
		if (gameConfigInstance.GetCannonTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
		{
			Debug.LogError($"Cannon Projectile Prefab не задан\n" + this.name);
			return false;
		}
		return base.CanShoot();
	}
	protected override void RotateTower()
	{
		#region Горизонтальный поворот
		// Направление, куда надо повернуть пушку
		Vector3 directionToTarget = m_predictedPosition - m_horizontalRotatingTowerPart.position;

		// Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
		if (directionToTarget != Vector3.zero)
		{
			// Игнорируем разницу по Y для горизонтального вращения
			directionToTarget.y = 0;

			// Из направления получаем поворот
			Quaternion targetHorizontalRotation = Quaternion.LookRotation(directionToTarget);

			// Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
			m_horizontalRotatingTowerPart.rotation = Quaternion.RotateTowards(
				m_horizontalRotatingTowerPart.rotation,
				targetHorizontalRotation,
				GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
			);
		}
		#endregion

		#region Вертикальный поворот
		// Вращение будет происходить по локальной оси X
		Vector3 localTargetPos = m_horizontalRotatingTowerPart.InverseTransformPoint(m_predictedPosition);
		float targetVerticalAngle = Mathf.Atan2(localTargetPos.y, localTargetPos.z) * Mathf.Rad2Deg;
		targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, -30f, 60f);

		// Вращаем дуло пушки по оси X для вертикального вращения
		Quaternion targetVerticalRotation = Quaternion.Euler(-targetVerticalAngle, 0, 0);
		m_verticalRotatingTowerPart.localRotation = Quaternion.RotateTowards(
			m_verticalRotatingTowerPart.localRotation,
			targetVerticalRotation,
			GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
		);
		#endregion
	}

	protected override void Shoot()
	{
		if (m_shootStartPoint == null)
		{
			Debug.LogError("Shoot Point не задан");
			return;
		}

		m_shootDirection = CalculateShootDirection();
		shootRotation = Quaternion.LookRotation(m_shootDirection);

		var projectile = Instantiate(GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).projectilePrefab, m_shootStartPoint.position, shootRotation);
		var cannonProjectile = projectile.GetComponent<CannonProjectile>();
		if (cannonProjectile != null)
		{
			cannonProjectile.Launch(GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed,
				GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).damage);
		}
		else
		{
			Debug.LogError($"Cannon projectile component = null");
		}

		m_lastShotTime = Time.time;
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

	private void OnDrawGizmos()
	{
		if (m_currentTarget != null && m_shootStartPoint != null)
		{
			// Линия к текущей позиции цели
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(m_shootStartPoint.position, m_currentTarget.transform.position);

			// Точка - предсказанная позиция для выстрела
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(m_predictedPosition, 0.2F);
		}
	}
}