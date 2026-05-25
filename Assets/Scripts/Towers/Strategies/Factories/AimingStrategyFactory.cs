using System;
using Tools;

namespace Tower
{
    public static class AimingStrategyFactory
    {
        public static IAimingStrategy Create(AimType type, TowerData towerData)
        {
            return type switch
            {
                AimType.Null => null,
                AimType.Direct => new DirectAimingStrategy(),
                AimType.Predicted => new PredictedAimingStrategy(towerData),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
