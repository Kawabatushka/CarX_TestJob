namespace Tower
{
    public abstract class BaseTowerDependencyFactory<T> : ITowerDependencyFactory<T> where T: BaseTower
    {
        public abstract void ApplyDependencies(T tower);
    }
}