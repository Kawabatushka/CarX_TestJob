using System;
using System.Collections.Generic;
using UnityEngine;

class EnemyManager : MonoBehaviour
{
	private static EnemyManager m_instance;
	public static EnemyManager instance => m_instance;
	/*{
		get
		{
			if (m_instance == null)
			{
				// Автоматически создаем EnemyManager если его нет
				var managerObject = new GameObject("EnemyManager");
				m_instance = managerObject.AddComponent<EnemyManager>();
			}
			return m_instance;
		}
	}*/

	private HashSet<Enemy> m_activeEnemies = new HashSet<Enemy>();

	private void Awake()
	{
		if (m_instance != null && m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
		m_instance = this;
	}

	public void RegisterEnemy(Enemy enemy)
	{
		m_activeEnemies.Add(enemy);
		enemy.OnDied.AddListener(UnregisterEnemy);
	}

	public void UnregisterEnemy(Enemy enemy)
	{
		if (m_activeEnemies.Contains(enemy))
		{
			m_activeEnemies.Remove(enemy);
		}
	}

	public IEnumerable<Enemy> GetActiveEnemies()
	{
		return m_activeEnemies;
	}

	public List<Enemy> GetEnemiesInRange(Vector3 position, float range)
	{
		List<Enemy> enemiesInRange = new List<Enemy>();
		float sqrRange = range * range;

		foreach (var enemy in m_activeEnemies)
		{
			if (enemy.isAlive && (enemy.transform.position - position).sqrMagnitude <= sqrRange)
			{
				enemiesInRange.Add(enemy);
			}
		}

		return enemiesInRange;
	}

	public Enemy GetClosestEnemy(Vector3 position, float range)
	{
		Enemy closestEnemy = null;
		float closestDistance = float.MaxValue;
		float sqrRange = range * range;

		foreach (var enemy in m_activeEnemies)
		{
			if (!enemy.isAlive) continue;

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