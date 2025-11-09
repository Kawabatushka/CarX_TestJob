using UnityEngine;
using System.Collections;

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
		if (Time.time >= m_lastSpawn + GameConfig.instance.enemySpawnSettings.spawnInterval)
		{
			SpawnEnemy();
			m_lastSpawn = Time.time;
		}
	}

	private void SpawnEnemy()
	{
		if (m_moveTarget == null)
		{
			Debug.LogError("Move Target не задан");
			return;
		}
		if (GameConfig.instance?.enemySpawnSettings?.enemyPrefab == null)
		{
			Debug.LogError("Enemy Prefab не задан");
			return;
		}


		var newEnemy = Instantiate(GameConfig.instance.enemySpawnSettings.enemyPrefab);
		newEnemy.transform.position = this.transform.position;
		newEnemy.TryGetComponent(out Enemy enemyComponent);
		enemyComponent.SetMoveTarget(m_moveTarget);
		EnemyManager.instance.RegisterEnemy(enemyComponent);
	}
}