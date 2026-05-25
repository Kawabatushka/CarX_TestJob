using UnityEditor;
using UnityEngine;

namespace Tools
{
	[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
	public class EnemyConfig : ScriptableSingleton<EnemyConfig>
	{
		[Space(10)]
		[SerializeField] private EnemyData m_enemySettings;

		[Space(10)]
		[SerializeField] private SpawnerData m_enemySpawnSettings;

		public EnemyData enemyData => m_enemySettings;

		public SpawnerData enemySpawnSettings => m_enemySpawnSettings;
	}

	[System.Serializable]
	public class EnemyData
	{
		[SerializeField] private float m_speed = 10f;
		[SerializeField] private int m_maxHP = 200;

		public int maxHP => m_maxHP;
		public float speed => m_speed;
	}

	[System.Serializable]
	public class SpawnerData
	{
		[SerializeField] private PooledObjectType m_enemyType;
		[SerializeField] private float m_spawnInterval = 1.5f;

		public PooledObjectType enemyType => m_enemyType;
		public float spawnInterval => m_spawnInterval;
	}
}