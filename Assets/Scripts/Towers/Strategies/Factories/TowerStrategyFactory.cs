using Tools;

namespace Tower
{
    public static class TowerStrategyFactory
    {
        public static void Configure(BaseTower tower, TowerData towerData)
        {
            tower
                .SetTargetFindingStrategy(TargetFindingStrategyFactory.Create(towerData.targetingType))
                .SetAimingStrategy(AimingStrategyFactory.Create(towerData.aimingType, towerData))
                .SetRotationStrategy(RotationStrategyFactory.Create(towerData.rotationType, towerData))
                .SetShootingConditionStrategy(ShootingConditionStrategyFactory.Create(towerData.conditionType, towerData))
                .SetShootingStrategy(ShootingStrategyFactory.Create(towerData.shootingType, towerData));
        }
    }
}
