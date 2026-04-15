using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
	class EnemyManager : MonoBehaviour
	{
		private static EnemyManager m_instance;
		public static EnemyManager instance => m_instance;

		private HashSet<SimpleEnemy> m_activeEnemies = new HashSet<SimpleEnemy>();

		private void Awake()
		{
			if (m_instance != null && m_instance != this)
			{
				Destroy(gameObject);
				return;
			}
			m_instance = this;
		}

		public void RegisterEnemy(SimpleEnemy enemy)
		{
			m_activeEnemies.Add(enemy);
			enemy.OnDied.AddListener(UnregisterEnemy);
		}

		public void UnregisterEnemy(SimpleEnemy enemy)
		{
			if (m_activeEnemies.Contains(enemy))
			{
				m_activeEnemies.Remove(enemy);
			}
		}

		public SimpleEnemy GetClosestEnemy(Vector3 position, float range)
		{
			SimpleEnemy closestEnemy = null;
			float closestDistance = float.MaxValue;
			float sqrRange = range * range;

			foreach (var enemy in m_activeEnemies)
			{
				if (!enemy.isAlive)
				{
					continue;
				}

				float sqrDistance = (enemy.transform.position - position).sqrMagnitude;
				if (sqrDistance <= sqrRange && sqrDistance < closestDistance)
				{
					closestDistance = sqrDistance;
					closestEnemy = enemy;
				}
			}

			return closestEnemy;
		}
	}
}