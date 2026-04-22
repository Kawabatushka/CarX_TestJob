using System;
using Projectile;
using UnityEngine;

namespace Pooling
{
public class GuidedProjectilePoolManager : MonoBehaviour
    {
        public static GuidedProjectilePoolManager instance { get; private set; }
        [SerializeField] private GuidedProjectile prefab;

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

            m_pool = new ObjectPool(prefab, 8);
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
            //guidedProjectile.gameObject.SetActive(false);
            m_pool.Release(guidedProjectile);
        }
    }
}