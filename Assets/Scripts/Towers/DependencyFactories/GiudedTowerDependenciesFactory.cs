namespace Tower
{
    public class GuidedTowerDependenciesFactory : BaseTowerDependencyFactory<GuidedTower>
    {
        public override void ApplyDependencies(GuidedTower tower)
        {
            // реализации вращения нет, т к GuidedTower не вращается
        }
    }
}