using System.Collections;
using UnityEngine;

public class CannonTower : BaseTower
{
	/// <summary>
	/// TODO - не забыть добавить поворот пушки в направлении стрельбы
	/// </summary>

	[Tooltip("Point of head of cannon")]
	[SerializeField] private Transform m_shootStartPoint;
	private Vector3 m_shootDirection;
	private Vector3 predictedPosition;

	protected override bool CanShoot()
	{
		if (gameConfigInstance.GetCannonTowerSettings(m_towerSettingsId)?.projectilePrefab == null)
		{
			Debug.LogError($"Cannon Projectile Prefab не задан\n" + this.name);
			return false;
		}
		return base.CanShoot();
	}

	protected override void Shoot()
	{
		if (m_shootStartPoint == null)
		{
			Debug.LogError("Shoot Point не задан");
			return;
		}

		m_shootDirection = CalculateShootDirection();
		Quaternion shootRotation = Quaternion.LookRotation(m_shootDirection);

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
		if (m_currentTarget.Velocity.magnitude < 0.1f)
		{
			return toTarget.normalized;
		}

		// Решение квадратного уравнения для точного расчета
		float a = Vector3.Dot(m_currentTarget.Velocity, m_currentTarget.Velocity) - (GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed * GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed);
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
		return (predictedPosition - m_shootStartPoint.position).normalized;
	}

	private void OnDrawGizmos()
	{
		if (m_currentTarget != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(m_shootStartPoint.position, m_currentTarget.transform.position);
		}
		if (m_shootStartPoint != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(predictedPosition, 0.2F);
		}
	}
}