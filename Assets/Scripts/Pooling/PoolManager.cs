using Projectile;
using UnityEngine;

namespace Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance { get; private set; }
        [SerializeField] private GuidedProjectile prefab;
        private const string PoolContainerName = "[Pool]";

        private IObjectPool m_pool;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            if (prefab == null)
            {
                // <- как тут правильно обработать NRE?
                Debug.LogError($"Prefab is not initialized in ObjectPool", this);
                return;
            }

            GameObject poolContainer = new GameObject(PoolContainerName + "_" + prefab.GetType());
            poolContainer.transform.SetParent(this.transform);
            m_pool = new ObjectPool(prefab, poolContainer.transform, 8);
        }

        public Component Get(Vector3 position, Quaternion rotation)
        {
            var guidedProjectile = m_pool.Get();
            guidedProjectile.transform.SetPositionAndRotation(position, rotation);
            return guidedProjectile;
        }

        public void Release(GuidedProjectile guidedProjectile)
        {
            if (guidedProjectile == null)
            {
                return;
            }
            m_pool.Release(guidedProjectile);
        }
    }
}