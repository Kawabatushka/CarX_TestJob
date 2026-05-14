using UnityEngine;
using Enemy;

namespace Tower
{
    public interface ITargetFindingStrategy
    {
        SimpleEnemy GetTarget(Vector3 towerPos, float rangeToFindEnemy);
    }
}