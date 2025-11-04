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
	[SerializeField] private bool m_useLeadingShot = true;
	[Tooltip("Will a ballistic trajectory be used?")]
	[SerializeField] private bool m_useGravity = true;
	private Vector3 m_shootDirection;


	private float m_gravity = Physics.gravity.y;///

	protected override void Shoot()
	{
		if (m_shootStartPoint == null)
		{
			Debug.LogError("Shoot Point не задан");
			return;
		}

		m_shootDirection = CalculateShootDirection2();
		Debug.LogError($"m_shootDirection={m_shootDirection}");
		Quaternion shootRotation = Quaternion.LookRotation(m_shootDirection);

		var projectile = Instantiate(m_projectilePrefab, m_shootStartPoint.position, shootRotation);
		var cannonProjectile = projectile.GetComponent<CannonProjectile>();
		if (cannonProjectile != null)
		{
			cannonProjectile.Launch(m_shootDirection, m_projectileSpeed);
		}

		m_lastShotTime = Time.time;
	}

	private Vector3 CalculateShootDirection2()
	{
		Vector3 direction;

		if (m_currentTarget == null)
		{
			return transform.forward;
		}

		Rigidbody targetRigidbody = m_currentTarget.GetComponent<Rigidbody>();
		if (targetRigidbody == null || !m_useLeadingShot)
		{
			return (m_currentTarget.transform.position - m_shootStartPoint.position);
		}

		Debug.LogError($"ENEMY m_currentTarget.transform.position={m_currentTarget.transform.position}\nm_currentTarget.Velocity={m_currentTarget.Velocity}");

		Vector3 leadPosition = TargetingSystem.CalculateParabolicLeadPosition(
				m_shootStartPoint.position,
				m_currentTarget.transform.position,
				m_currentTarget.Velocity,
				m_gravity);
		Debug.LogError($"leedPosition={leadPosition}");
		var timeToTarget = TargetingSystem.CalculateParabolicTimeToTarget(m_shootStartPoint.position, leadPosition, m_gravity); // до сюда работает
		var projectileSpeed = TargetingSystem.CalculateParabolicProjectileSpeed(m_shootStartPoint.position, leadPosition, timeToTarget);
		direction = leadPosition - m_shootStartPoint.position;
		return direction.normalized * projectileSpeed;


		/*if (true*//*m_useGravity*//*)
		{
			leadPosition = TargetingSystem.CalculateParabolicLeadPosition(
				m_shootPoint.position,
				currentTarget.Position,
				currentTarget.Velocity,
				GameManager.Instance.Config.Gravity);
			var timeToTarget = TargetingSystem.CalculateParabolicTimeToTarget(m_shootPoint.position, leadPosition, GameManager.Instance.Config.Gravity);
			var projectileSpeed = TargetingSystem.CalculateParabolicProjectileSpeed(m_shootPoint.position, leadPosition, timeToTarget);
			direction = leadPosition - m_shootPoint.position;
			direction.y = 0;
			((CannonProjectileParabolic)currentProjectile).Initialize(direction.normalized * projectileSpeed);
			return Vector3.back;
		}
*/


	}///

	private Vector3 CalculateShootDirection()
	{
		if (m_currentTarget == null) return transform.forward;

		Rigidbody targetRigidbody = m_currentTarget.GetComponent<Rigidbody>();
		if (targetRigidbody == null || !m_useLeadingShot)
		{
			return (m_currentTarget.transform.position - m_shootStartPoint.position).normalized;
		}

		Vector3 targetVelocity = targetRigidbody.velocity;
		Vector3 toTarget = m_currentTarget.transform.position - m_shootStartPoint.position;

		if (targetVelocity.magnitude < 0.1f)
		{
			return toTarget.normalized;
		}

		// Решение квадратного уравнения для точного расчета
		float a = Vector3.Dot(targetVelocity, targetVelocity) - (m_projectileSpeed * m_projectileSpeed);
		float b = 2f * Vector3.Dot(targetVelocity, toTarget);
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

		Vector3 predictedPosition = m_currentTarget.transform.position + targetVelocity * timeToTarget;
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
			Gizmos.DrawSphere(m_shootDirection, 0.2F);
		}
	}
}


// (moveTargetPosition - transform.position).normalized;


/*public class CannonTower222 : BaseTower
{
	private bool isTargetAcquired = false;
	Transform m_shootPoint;
	protected override TowerConfig GetConfig()
	{
		return ConfigManager.Instance.GetCannonTowerConfig();
	}

	protected override void Shoot()
	{
		if (!isTargetAcquired) return;

		projectilePoolName = Config.UseParabolicTrajectory ? Config.ParabolicProjectilePrefab.PoolName : Config.ProjectilePrefab.PoolName;

		base.Shoot();
	}

	public override void UpdateTargeting()
	{
		base.UpdateTargeting();

		if (HasTarget && turret != null)
		{
			RotateTurretTowardsTarget();
		}
	}

	protected override void InitializeProjectile()
	{
		Vector3 leadPosition;
		Vector3 direction;
		currentProjectile.transform.position = m_shootPoint.position;
		currentProjectile.transform.rotation = m_shootPoint.rotation;

		if (Config.UseParabolicTrajectory)
		{
			leadPosition = TargetingSystem.CalculateParabolicLeadPosition(
				m_shootPoint.position,
				currentTarget.Position,
				currentTarget.Velocity,
				GameManager.Instance.Config.Gravity);
			var timeToTarget = TargetingSystem.CalculateParabolicTimeToTarget(m_shootPoint.position, leadPosition, GameManager.Instance.Config.Gravity);
			var projectileSpeed = TargetingSystem.CalculateParabolicProjectileSpeed(m_shootPoint.position, leadPosition, timeToTarget);
			direction = leadPosition - m_shootPoint.position;
			direction.y = 0;
			((CannonProjectileParabolic)currentProjectile).Initialize(direction.normalized * projectileSpeed);
			return;
		}

		leadPosition = TargetingSystem.CalculateLeadPosition(
				m_shootPoint.position,
				currentTarget.Position,
				currentTarget.Velocity,
				Config.ProjectilePrefab.Speed);
		direction = (leadPosition - m_shootPoint.position).normalized;
		currentProjectile.Initialize(direction);
	}

	private void RotateTurretTowardsTarget()
	{
		var targetRotation = Config.UseParabolicTrajectory ?
			TargetingSystem.CalculateParabolicAimRotationWithLead(
				m_shootPoint.position,
				currentTarget.Position,
				currentTarget.Velocity,
				GameManager.Instance.Config.Gravity) :
			TargetingSystem.CalculateAimRotationWithLead(
				m_shootPoint.position,
				currentTarget.Position,
				currentTarget.Velocity,
				Config.ProjectilePrefab.Speed);
		targetRotation.x = 0;
		targetRotation.z = 0;
		turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, Config.RotationSpeed * Time.deltaTime);

		isTargetAcquired = Mathf.Abs(Quaternion.Dot(m_shootPoint.rotation.normalized, targetRotation.normalized)) >= 0.99f;
	}
}*/