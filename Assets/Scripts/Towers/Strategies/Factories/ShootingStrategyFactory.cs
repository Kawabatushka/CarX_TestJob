using System;
using Tools;

namespace Tower
{
    public static class ShootingStrategyFactory
    {
        public static IShootingStrategy Create(ShootingType type, TowerData towerData)
        {
            return type switch
            {
                ShootingType.Null => null,
                ShootingType.Predicted => new PredictedShootingStrategy(towerData),
                ShootingType.Guided => new GuidedShootingStrategy(towerData),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
