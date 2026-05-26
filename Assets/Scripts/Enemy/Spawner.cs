using UnityEngine;
using Pooling;
using Tools;

namespace Enemy
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private Transform m_moveTarget;

		private float m_lastSpawn = -1f;

		private void Start()
		{
			if (EnemyManager.instance == null)
			{
				var managerObject = new GameObject("EnemyManager");
				managerObject.AddComponent<EnemyManager>();
			}
		}

		private void Update()
		{
			if (Time.time >= m_lastSpawn + EnemyConfig.instance.enemySpawnSettings.spawnInterval)
			{
				SpawnEnemy();
				m_lastSpawn = Time.time;
			}
		}

		private void SpawnEnemy()
		{
			if (m_moveTarget == null)
			{
				Debug.LogError($"{nameof(Spawner)}.{nameof(SpawnEnemy)} m_moveTarget is null");
				return;
			}

			if (PoolManager.instance == null)
			{
				Debug.LogError($"{nameof(Spawner)}.{nameof(SpawnEnemy)} {nameof(PoolManager)} is missing on the scene");
				return;
			}

			var newEnemy = PoolManager.instance.Get(EnemyConfig.instance.enemySpawnSettings.enemyType, true, transform.position);
			if (newEnemy == null)
			{
				return;
			}

			newEnemy.TryGetComponent(out SimpleEnemy enemyComponent);
			if (enemyComponent != null)
			{
				enemyComponent.SetMoveTarget(m_moveTarget);
				EnemyManager.instance.RegisterEnemy(enemyComponent);
			}
		}
	}
}