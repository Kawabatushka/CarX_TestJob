using UnityEngine;
using Enemy;

namespace Tower
{
	public class CannonTower : BaseTower
	{
		protected override void ConfigureStrategies()
		{
			m_targetFindingStrategy = new GetClosestTargetStrategy();
			m_aimingStrategy = new CannonTowerAimingStrategy(m_projectileSettingsId);
			m_rotationStrategy = new CannonTowerRotationStrategy(m_towerSettingsId);
			m_shootingConditionStrategy = new CannonTowerShootingConditionStrategy(m_towerSettingsId);
			m_shootingStrategy = new CannonShootingStrategy(m_towerSettingsId, m_projectileSettingsId);
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