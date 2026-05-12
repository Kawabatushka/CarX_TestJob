namespace Tower
{
    public class CannonTowerDependenciesFactory : BaseTowerDependencyFactory<CannonTower>
    {
        public override void ApplyDependencies(CannonTower tower)
        {
            tower.SetRotationStrategy(tower.CreateRotationStrategy());
        }
    }
}