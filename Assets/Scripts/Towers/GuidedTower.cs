using UnityEngine;
using Enemy;

namespace Tower
{
	public class GuidedTower : BaseTower
	{
		protected override void ConfigureStrategies()
		{
			m_targetFindingStrategy = new GetClosestTargetStrategy();
			m_aimingStrategy = new DirectAimingStrategy();
			m_rotationStrategy = new NullRotationStrategy();
			m_shootingConditionStrategy = new GuidedTowerShootingConditionStrategy(m_towerSettingsId);
			m_shootingStrategy = new GuidedShootingStrategy(m_towerSettingsId, m_projectileSettingsId);
		}

		protected override float GetRangeToFindEnemy()
		{
			return GameConfig.instance.GetGuidedTowerSettings(m_towerSettingsId).rangeToFindEnemy;
		}
	}
}