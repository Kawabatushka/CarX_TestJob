using System.Collections;
using UnityEngine;

public class CannonTower : BaseTower
{
	/// <summary>
	/// TODO - не забыть добавить поворот пушки в направлении стрельбы
	/// </summary>

	[Header("Cannon Tower Settings")]
	[Tooltip("Point of head of cannon")]
	[SerializeField] private Transform m_shootStartPoint;
	[SerializeField] private float m_projectileSpeed = 20f;
	private Vector3 m_shootDirection;
	private Vector3 predictedPosition;

	protected override void Shoot()
	{
		if (m_shootStartPoint == null)
		{
			Debug.LogError("Shoot Point не задан");
			return;
		}

		m_shootDirection = CalculateShootDirection();
		Debug.LogError($"m_shootDirection={m_shootDirection}");
		Quaternion shootRotation = Quaternion.LookRotation(m_shootDirection);

		var projectile = Instantiate(m_projectilePrefab, m_shootStartPoint.position, shootRotation);
		var cannonProjectile = projectile.GetComponent<CannonProjectile>();
		if (cannonProjectile != null)
		{
			cannonProjectile.Launch(m_shootDirection, m_projectileSpeed);
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
		if (m_currentTarget.Velocity.magnitude < 0.1f)
		{
			return toTarget.normalized;
		}

		// Решение квадратного уравнения для точного расчета
		float a = Vector3.Dot(m_currentTarget.Velocity, m_currentTarget.Velocity) - (m_projectileSpeed * m_projectileSpeed);
		float b = 2f * Vector3.Dot(m_currentTarget.Velocity, toTarget);
		float c = Vector3.Dot(toTarget, toTarget);

		float discriminant = b * b - 4f * a * c;

		if (discriminant < 0)
		{
			// Нет решения - цель слишком быстрая, стреляем прямо
			return toTarget.normalized;
		}

		float time1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
		float time2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

		float timeToTarget = Mathf.Max(time1, time2);
		if (timeToTarget < 0)
		{
			return toTarget.normalized;
		}

		predictedPosition = m_currentTarget.transform.position + m_currentTarget.Velocity * timeToTarget;
		Debug.LogError($"predictedPosition = {predictedPosition}");
		return (predictedPosition - m_shootStartPoint.position).normalized;
	}

	private void OnDrawGizmos()
	{
		if (m_currentTarget != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(m_shootStartPoint.position, m_currentTarget.transform.position);
		}
		if(m_shootStartPoint != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(predictedPosition, 0.2F);
		}
	}
}