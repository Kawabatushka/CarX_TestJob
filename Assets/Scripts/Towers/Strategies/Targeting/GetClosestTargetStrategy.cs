using UnityEngine;
using Enemy;

namespace Tower
{
    public class GetClosestTargetStrategy : ITargetFindingStrategy
    {
        public SimpleEnemy GetTarget(Vector3 towerPos, float rangeToFindEnemy)
        {
            return EnemyManager.instance?.GetClosestEnemy(towerPos, rangeToFindEnemy);
        }
    }
}