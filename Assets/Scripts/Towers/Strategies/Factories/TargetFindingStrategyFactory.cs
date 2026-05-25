using System;
using Tools;

namespace Tower
{
    public static class TargetFindingStrategyFactory
    {
        public static ITargetFindingStrategy Create(TargetFindingType type)
        {
            return type switch
            {
                TargetFindingType.Null => null,
                TargetFindingType.GetClosest => new GetClosestTargetStrategy(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
