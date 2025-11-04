using UnityEngine;

public static class TargetingSystem
{
	public static Vector3 CalculateParabolicLeadPosition(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float gravity)
	{
		float timeToTarget = CalculateParabolicTimeToTarget(shooterPos, targetPos, gravity);
		Vector3 leadPosition = targetPos + targetVelocity * timeToTarget;
		return leadPosition;
	}

	public static float CalculateParabolicTimeToTarget(Vector3 shooterPos, Vector3 targetPos, float gravity)
	{
		Vector3 distanceToTarget = targetPos - shooterPos;
		var timeToTarget = Mathf.Sqrt(Mathf.Abs(2 * distanceToTarget.y / gravity));
		return timeToTarget;
	}

	public static Quaternion CalculateParabolicAimRotationWithLead(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float gravity)
	{
		Vector3 leadPosition = CalculateParabolicLeadPosition(shooterPos, targetPos, targetVelocity, gravity);
		return CalculateAimRotation(shooterPos, leadPosition);
	}

	// для прямолинейной траектории снаряда
	public static Vector3 CalculateLeadPosition(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float projectileSpeed)
	{
		Vector3 toTarget = targetPos - shooterPos;
		var targetSpeedSquared = Vector3.Dot(targetVelocity, targetVelocity);
		var projectileSpeedSquared = projectileSpeed * projectileSpeed;

		var a = targetSpeedSquared - projectileSpeedSquared;
		var b = 2 * Vector3.Dot(toTarget, targetVelocity);
		var c = Vector3.Dot(toTarget, toTarget);

		var t = 0f;

		if (Mathf.Abs(a) < 0.001f)
		{
			if (Mathf.Abs(b) < 0.001f)
			{
				return targetPos;
			}

			t = -c / b;
			if (t < 0)
			{
				t = 0;
			}
			return targetPos + targetVelocity * t;
		}

		var discriminant = b * b - 4 * a * c;
		if (discriminant < 0)
		{
			return targetPos; // Нет решения TODO
		}

		var t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
		var t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

		t = t1 > 0 ? t1 : (t2 > 0 ? t2 : 0);

		return targetPos + targetVelocity * t;
	}

	public static bool CanHitTarget(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float projectileSpeed, float range)
	{
		float distance = Vector3.Distance(shooterPos, targetPos);
		if (distance > range) return false;

		Vector3 leadPosition = CalculateLeadPosition(shooterPos, targetPos, targetVelocity, projectileSpeed);
		float leadDistance = Vector3.Distance(shooterPos, leadPosition);

		return leadDistance <= range;
	}

	public static Quaternion CalculateAimRotation(Vector3 shooterPos, Vector3 targetPos)
	{
		Vector3 direction = (targetPos - shooterPos).normalized;
		return Quaternion.LookRotation(direction);
	}

	public static Quaternion CalculateAimRotationWithLead(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVelocity, float projectileSpeed)
	{
		Vector3 leadPosition = CalculateLeadPosition(shooterPos, targetPos, targetVelocity, projectileSpeed);
		return CalculateAimRotation(shooterPos, leadPosition);
	}

	public static float CalculateParabolicProjectileSpeed(Vector3 shooterPos, Vector3 targetPos, float timeToTarget)
	{
		Vector3 distanceToTarget = targetPos - shooterPos;
		Vector2 distanceToTarget2d = new Vector2(distanceToTarget.x, distanceToTarget.z);
		return (distanceToTarget2d / timeToTarget).magnitude;
	}
}