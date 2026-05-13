using Enemy;
using UnityEngine;

namespace Tower
{
    public class CannonTowerAimingStrategy : IAimingStrategy
    {
        private readonly int m_projectileSettingsId;

        public CannonTowerAimingStrategy(int projectileSettingsId)
        {
            m_projectileSettingsId = projectileSettingsId;
        }

        public void CalculateAim(SimpleEnemy target, Transform shootStartPoint, out Vector3 predictedPosition, out Vector3 aimDirection)
        {
            if (target == null || shootStartPoint == null)
            {
                predictedPosition = Vector3.zero;
                aimDirection = Vector3.zero;
                return;
            }

            Vector3 toTarget = target.transform.position - shootStartPoint.position;
            if (target.velocity.magnitude < 0.1f)
            {
                predictedPosition = target.transform.position;
                aimDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            #region Решение квадратного уравнения для точного расчета
            float a = Vector3.Dot(target.velocity, target.velocity) -
                Mathf.Pow(GameConfig.instance.GetCannonProjectileSettings(m_projectileSettingsId).speed, 2);
            float b = 2f * Vector3.Dot(target.velocity, toTarget);
            float c = Vector3.Dot(toTarget, toTarget);

            float discriminant = b * b - 4f * a * c;

            if (discriminant < 0)
            {
                // Нет решения - цель слишком быстрая, стреляем прямо
                predictedPosition = target.transform.position;
                aimDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            float time1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
            float time2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

            var timeToTarget = Mathf.Max(time1, time2);
            #endregion

            if (timeToTarget < 0)
            {
                predictedPosition = target.transform.position;
                aimDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            predictedPosition = target.transform.position + target.velocity * timeToTarget;
            aimDirection = (predictedPosition - shootStartPoint.position).normalized;
        }
    }
}