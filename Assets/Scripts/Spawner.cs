using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Transform m_moveTarget;

	private float m_lastSpawn = -1f;

	void Start()
	{
		if (EnemyManager.instance == null)
		{
			var managerObject = new GameObject("EnemyManager");
			managerObject.AddComponent<EnemyManager>();
		}
	}

	void Update()
	{
		if (Time.time >= m_lastSpawn + GameConfig.instance.spawnSettings.spawnInterval)
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

		var newEnemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		newEnemy.transform.position = transform.position;
		// Отключение влияния гравитации на созданный объект врага
		var newEnemyRigidbody = newEnemy.AddComponent<Rigidbody>();
		newEnemyRigidbody.useGravity = false;
		// Смена цвета врага
		var newEnemyRenderer = newEnemy.GetComponent<Renderer>();
		newEnemyRenderer.material.color = Color.magenta;

		var enemyComponent = newEnemy.AddComponent<Enemy>();
		if (enemyComponent != null)
		{
			enemyComponent.SetMoveTarget(m_moveTarget);
			EnemyManager.instance.RegisterEnemy(enemyComponent);
		}
	}
}