using Enemy;
using UnityEngine;
using Tools;

namespace Tower
{
    public class PredictedAimingStrategy : IAimingStrategy
    {
        private readonly float m_projectileSpeed;

        public PredictedAimingStrategy(TowerData towerData)
        {
            m_projectileSpeed = towerData.projectileSpeed;
        }

        public void CalculateAim(SimpleEnemy target, Transform shootStartPoint, out Vector3 predictedPosition, out Vector3 shootDirection)
        {
            if (target == null || shootStartPoint == null)
            {
                predictedPosition = Vector3.zero;
                shootDirection = Vector3.zero;
                return;
            }

            Vector3 toTarget = target.transform.position - shootStartPoint.position;
            if (target.velocity.magnitude < 0.1f)
            {
                predictedPosition = target.transform.position;
                shootDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            #region Решение квадратного уравнения для точного расчета
            float a = Vector3.Dot(target.velocity, target.velocity) -
                Mathf.Pow(m_projectileSpeed, 2);
            float b = 2f * Vector3.Dot(target.velocity, toTarget);
            float c = Vector3.Dot(toTarget, toTarget);

            float discriminant = b * b - 4f * a * c;

            if (discriminant < 0)
            {
                // Нет решения - цель слишком быстрая, стреляем прямо
                predictedPosition = target.transform.position;
                shootDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            float time1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
            float time2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

            var timeToTarget = Mathf.Max(time1, time2);
            #endregion

            if (timeToTarget < 0)
            {
                predictedPosition = target.transform.position;
                shootDirection = toTarget.sqrMagnitude > 0 ? toTarget.normalized : Vector3.forward;
                return;
            }

            predictedPosition = target.transform.position + target.velocity * timeToTarget;
            shootDirection = (predictedPosition - shootStartPoint.position).normalized;
        }
    }
}