using System;
using Tools;

namespace Tower
{
    public static class ShootingConditionStrategyFactory
    {
        public static IShootingConditionStrategy Create(ConditionType type, TowerData towerData)
        {
            return type switch
            {
                ConditionType.Null => null,
                ConditionType.RotationToTargetReached => new RotationToTargetReachedShootingConditionStrategy(towerData),
                ConditionType.OnlyCooldownManaged => new OnlyCooldownManagedShootingConditionStrategy(towerData),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
