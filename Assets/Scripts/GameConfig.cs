using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
	[Space(10)]
	[SerializeField] private EnemyData m_enemySettings;

	[Space(10)]
	[SerializeField] private SpawnerData m_spawnSettings;

	[Space(10)]
	[SerializeField] private TowerData m_towerSettings;

	[Space(10)]
	[SerializeField] private BaseProjectileData m_baseProjectileSettings;
	[Space(5)]
	[SerializeField] private CannonProjectileData m_cannonProjectileSettings;
	[Space(5)]
	[SerializeField] private GuidedProjectileData m_guidedProjectileSettings;

	public EnemyData enemyData => m_enemySettings;
	public SpawnerData spawnSettings => m_spawnSettings;
	public TowerData towerSettings => m_towerSettings;
	public BaseProjectileData baseProjectileSettings => m_baseProjectileSettings;
	public CannonProjectileData cannonProjectileSettings => m_cannonProjectileSettings;
	public GuidedProjectileData guidedProjectileSettings => m_guidedProjectileSettings;


	private static GameConfig m_instance;
	public static GameConfig instance
	{
		get
		{
			if (m_instance == null)
			{
				// Автоматически ищем в ресурсах (Resource.Load)
				m_instance = Resources.Load<GameConfig>("GameConfig");
				if (m_instance == null)
				{
					Debug.LogError("GameConfig.asset не найден в папке Resources");
				}
			}
			return m_instance;
		}
	}
}

[System.Serializable]
public class EnemyData
{
	[SerializeField] private float m_speed = 7f;
	[SerializeField] private int m_maxHP = 30;

	public int maxHP => m_maxHP;
	public float speed => m_speed;
}

[System.Serializable]
public class SpawnerData
{
	[SerializeField] private float m_spawnInterval = 1.5f;

	public float spawnInterval => m_spawnInterval;
}

[System.Serializable]
public class TowerData
{
	[SerializeField] private float m_shootInterval = 0.5f;
	[SerializeField] private float m_rangeToFindEnemy = 20f;
	[SerializeField] private GameObject m_cannonProjectilePrefab;
	[SerializeField] private GameObject m_guidedProjectilePrefab;

	public float shootInterval => m_shootInterval;
	public float rangeToFindEnemy => m_rangeToFindEnemy;
	public GameObject cannonProjectilePrefab => m_cannonProjectilePrefab;
	public GameObject guidedProjectilePrefab => m_guidedProjectilePrefab;
}

[System.Serializable]
public class BaseProjectileData
{
	[SerializeField] private float m_speed = 20f;

	public float speed => m_speed;
}

[System.Serializable]
public class CannonProjectileData
{
	[SerializeField] private int m_damage = 10;

	public int damage => m_damage;
}

[System.Serializable]
public class GuidedProjectileData
{
	[SerializeField] private Vector3 m_spawnOffset = new Vector3(0, 5.5f, 0);

	public Vector3 spawnOffset => m_spawnOffset;
}