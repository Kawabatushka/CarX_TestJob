using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
	[Space(10)]
	[SerializeField] private EnemyData m_enemySettings;

	[Space(10)]
	[SerializeField] private SpawnerData m_enemySpawnSettings;

	private BaseTowerData m_towerSettings;
	[Space(10)]
	[SerializeField] private List<CannonTowerData> m_cannonTowerSettings;
	[Space(5)]
	[SerializeField] private List<ProjectileTowerData> m_guidedTowerSettings;

	[Space(10)]
	[SerializeField] private List<CannonProjectileData> m_cannonProjectileSettings;
	[Space(5)]
	[SerializeField] private List<GuidedProjectileData> m_guidedProjectileSettings;

	[Space(20)]
	[SerializeField] private CannonTowerData m_defaultCannonTowerSettings;
	[SerializeField] private ProjectileTowerData m_defaultGuidedTowerSettings;
	[Space(5)]
	[SerializeField] private CannonProjectileData m_defaultCannonProjectileSettings;
	[SerializeField] private GuidedProjectileData m_defaultGuidedProjectileSettings;


	public EnemyData enemyData => m_enemySettings;

	public SpawnerData enemySpawnSettings => m_enemySpawnSettings;

	public BaseTowerData towerSettings => m_towerSettings;

	public CannonTowerData GetCannonTowerSettings(int id)
	{
		if (m_cannonTowerSettings.Count > id)
		{
			if (m_cannonTowerSettings[id].projectilePrefab == null)
			{
				m_cannonTowerSettings[id].projectilePrefab = m_defaultCannonTowerSettings.projectilePrefab;
			}
			return m_cannonTowerSettings[id];
		}
		return m_defaultCannonTowerSettings;
	}

	public ProjectileTowerData GetGuidedTowerSettings(int id)
	{
		if (m_guidedTowerSettings.Count > id)
		{
			if (m_guidedTowerSettings[id].projectilePrefab == null)
			{
				m_guidedTowerSettings[id].projectilePrefab = m_defaultGuidedTowerSettings.projectilePrefab;
			}
			return m_guidedTowerSettings[id];
		}
		return m_defaultGuidedTowerSettings;
	}

	public CannonProjectileData GetCannonProjectileSettings(int id)
	{
		if (m_cannonProjectileSettings.Count > id)
		{
			return m_cannonProjectileSettings[id];
		}
		return m_defaultCannonProjectileSettings;
	}

	public GuidedProjectileData GetGuidedProjectileSettings(int id)
	{
		if (m_guidedProjectileSettings.Count > id)
		{
			return m_guidedProjectileSettings[id];
		}
		return m_defaultGuidedProjectileSettings;
	}


	private static GameConfig m_instance;

	public static GameConfig instance
	{
		get
		{
			if (m_instance == null)
			{
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
	[SerializeField] private float m_speed = 10f;
	[SerializeField] private int m_maxHP = 200;

	public int maxHP => m_maxHP;
	public float speed => m_speed;
}

[System.Serializable]
public class SpawnerData
{
	[SerializeField] private GameObject m_enemyPrefab;
	//[SerializeField] private Transform m_moveTarget;
	[SerializeField] private float m_spawnInterval = 1.5f;

	public GameObject enemyPrefab => m_enemyPrefab;
	//public Transform moveTarget => m_moveTarget;
	public float spawnInterval => m_spawnInterval;
}

[System.Serializable]
public class BaseTowerData
{
	[SerializeField] protected float m_shootInterval = 0.5f;
	[SerializeField] protected float m_rangeToFindEnemy = 20f;
	[SerializeField] private GameObject m_projectilePrefab;

	public float shootInterval => m_shootInterval;
	public float rangeToFindEnemy => m_rangeToFindEnemy;
	public GameObject projectilePrefab
	{
		get
		{
			return m_projectilePrefab;
		}
		set
		{
			m_projectilePrefab = value;
		}
	}
}

[System.Serializable]
public class CannonTowerData : BaseTowerData
{
	[SerializeField] protected float m_rotationSpeed = 20f;

	public float rotationSpeed => m_rotationSpeed;
}

[System.Serializable]
public class ProjectileTowerData : BaseTowerData
{

}

[System.Serializable]
public class BaseProjectileData
{
	[SerializeField] protected float m_speed = 20f;
	[SerializeField] protected int m_damage = 10;

	public float speed => m_speed;
	public int damage => m_damage;
}

[System.Serializable]
public class CannonProjectileData : BaseProjectileData
{

}

[System.Serializable]
public class GuidedProjectileData : BaseProjectileData
{
	[SerializeField] private Vector3 m_spawnOffset = new Vector3(0, 5.5f, 0);

	public Vector3 spawnOffset => m_spawnOffset;

}