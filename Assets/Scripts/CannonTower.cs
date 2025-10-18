using UnityEngine;
using System.Collections;

public class CannonTower : BaseTower
{
	/// <summary>
	/// TODO - не забыть добавить поворот пушки в направлении стрельбы
	/// </summary>

	[Header("Cannon Tower Settings")]
	[SerializeField] private Transform m_shootPoint;
	[SerializeField] private float m_projectileSpeed = 15f;
	[SerializeField] private bool m_useLeadingShot = true;

	protected override void Shoot()
	{
		if (m_shootPoint == null)
		{
			Debug.LogError("Shoot Point не задан");
			return;
		}

		Vector3 shootDirection = GetShootDirection();
		Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

		var projectile = Instantiate(m_projectilePrefab, m_shootPoint.position, shootRotation);
		var cannonProjectile = projectile.GetComponent<CannonProjectile>();
		if (cannonProjectile != null)
		{
			cannonProjectile.Launch(m_projectileSpeed);
		}

		m_lastShotTime = Time.time;
	}

	private Vector3 GetShootDirection()
	{
		if (!m_useLeadingShot || m_currentTarget == null)
		{
			return (m_currentTarget.transform.position - m_shootPoint.position).normalized;
		}

		// Расчет стрельбы с упреждением
		return CalculateLeadingShot();
	}

	private Vector3 CalculateLeadingShot()
	{
		Vector3 targetPosition = m_currentTarget.transform.position;
		Vector3 targetVelocity = GetTargetVelocity();

		float distanceToTarget = Vector3.Distance(m_shootPoint.position, targetPosition);
		float timeToTarget = distanceToTarget / m_projectileSpeed;

		// Прогнозируемая позиция цели
		Vector3 predictedPosition = targetPosition + targetVelocity * timeToTarget;

		return (predictedPosition - m_shootPoint.position).normalized;
	}

	private Vector3 GetTargetVelocity()
	{
		// Простая оценка скорости цели на основе перемещения
		// В реальном проекте можно добавить компонент VelocityEstimator
		var enemy = m_currentTarget.GetComponent<Enemy>();
		if (enemy != null)
		{
			Vector3 direction = (enemy.moveTargetPosition - m_currentTarget.transform.position).normalized;
			// Предполагаем, что скорость врага постоянна
			return direction * 0.1f; // Примерное значение скорости
		}

		return Vector3.zero;
	}
}