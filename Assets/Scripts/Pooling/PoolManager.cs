using System;
using System.Collections.Generic;
using Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Pooling
{
    // <- странно, но нету метода Add/Create. Не особо понятно, как работает Юнитевский  пул
    public class GuidedProjectilePoolManager : MonoBehaviour
    {
        public static GuidedProjectilePoolManager instance { get; private set; }

        private ObjectPool<GuidedProjectile> m_pool;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            m_pool = new ObjectPool<GuidedProjectile>(
                createFunc: CreateItem,
                collectionCheck: true,   // helps catch double-release mistakes
                defaultCapacity: 8,
                maxSize: 32
            );
        }

        public GuidedProjectile Get(Vector3 position, Quaternion rotation)
        {
            var guidedProjectile = m_pool.Get();
            guidedProjectile.transform.SetPositionAndRotation(position, rotation);
            guidedProjectile.gameObject.SetActive(true);
            return guidedProjectile;
        }

        public void Release(GuidedProjectile guidedProjectile)
        {
            if (guidedProjectile == null)
            {
                return;
            }
            guidedProjectile.gameObject.SetActive(false);
            m_pool.Release(guidedProjectile);
        }

        private GuidedProjectile CreateItem()
        {
            GuidedProjectile newItem = Instantiate(
                GameConfig.instance.GetGuidedTowerSettings(0).projectilePrefab
                ).GetComponent<GuidedProjectile>();
            newItem.name = "PooledCube";
            newItem.gameObject.SetActive(false);
            return newItem;
        }
    }
}