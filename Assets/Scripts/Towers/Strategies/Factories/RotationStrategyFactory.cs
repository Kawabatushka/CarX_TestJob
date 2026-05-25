using System;
using Tools;

namespace Tower
{
    public static class RotationStrategyFactory
    {
        public static IRotationStrategy Create(RotationType type, TowerData towerData)
        {
            return type switch
            {
                RotationType.Null => new NullRotationStrategy(),
                RotationType.Smooth => new SmoothRotationStrategy(towerData),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
