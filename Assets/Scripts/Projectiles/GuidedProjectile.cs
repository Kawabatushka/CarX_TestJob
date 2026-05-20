using UnityEngine;
using Pooling;
using Enemy;

namespace Projectile
{
	public class GuidedProjectile : BaseProjectile
	{
		private GameObject m_target;

		public void Launch(GameObject target, float speed, int damage)
		{
			base.Launch(speed, damage);
			m_target = target;
		}

		public override void OnDespawned()
		{
			base.OnDespawned();
			m_target = null;
		}

		protected override void Move()
		{
			if (!m_target.activeSelf)
			{
				PoolManager.instance?.Release(gameObject);
				return;
			}

			Vector3 translation = m_target.transform.position - transform.position;
			if (translation.magnitude > m_speed * Time.deltaTime)
			{
				translation = translation.normalized * (m_speed * Time.deltaTime);
			}
			transform.Translate(translation, Space.World);
		}

		protected override void OnTriggerEnter(Collider other)
		{
			var enemy = other.GetComponent<SimpleEnemy>();
			if (enemy != null && enemy.isAlive)
			{
				enemy.ApplyDamage(m_damage);
				PoolManager.instance?.Release(gameObject);
			}
		}
	}
}