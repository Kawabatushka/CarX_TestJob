namespace Tower
{
    public interface ITowerDependencyFactory<T> where T : BaseTower
    {
        void ApplyDependencies(T tower);
    }
}