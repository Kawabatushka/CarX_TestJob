using Enemy;
using UnityEngine;
using Pooling;

namespace Projectile
{
	public abstract class BaseProjectile : MonoBehaviour, IPoolable
	{
		protected int m_damage;
		protected float m_speed;
		protected bool m_isLaunched = false;

		public virtual void Launch(float speed, int damage)
		{
			m_speed = speed;
			m_damage = damage;
			m_isLaunched = true;
		}

		public virtual void OnSpawned()
		{
			// false потому, что снаряд просто заспавнился, но не начал движение
			m_isLaunched = false;
		}

		public virtual void OnDespawned()
		{
			m_isLaunched = false;
			m_speed = 0f;
			m_damage = 0;
		}

		protected abstract void Move();

		protected virtual void Update()
		{
			if (m_isLaunched)
			{
				Move();
			}
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var enemy = other.GetComponent<SimpleEnemy>();
			if (enemy != null && enemy.isAlive)
			{
				enemy.ApplyDamage(m_damage);

				// If projectile came from pool - return it, otherwise destroy.
				if (GetComponent<PooledObject>() != null)
				{
					PoolManager.instance?.Release(gameObject);
				}
				else
				{
					Destroy(gameObject);
				}
			}
		}
	}
}