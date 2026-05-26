using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Tools
{
    [CreateAssetMenu(fileName = "TowersConfiguratorConfig", menuName = "Configs/TowersConfiguratorConfig")]
    //public class TowersDataConfig : ScriptableSingleton<TowersDataConfig>
    public class TowersDataConfig : ScriptableObject
    {
        [SerializeField] private List<TowerData> m_towerPresets = new List<TowerData>();
        public List<TowerData> towerPresets => m_towerPresets;

		private static TowersDataConfig m_instance;
		public static TowersDataConfig instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = Resources.Load<TowersDataConfig>("TowersDataConfig");
					if (m_instance == null)
					{
						Debug.LogError($"{nameof(TowersDataConfig)}.asset не найден в папке Resources");
					}
				}
				return m_instance;
			}
		}
    }

    [System.Serializable]
    public class TowerData
    {
        [SerializeField] private PooledObjectType m_towerType;

        [SerializeField] private TargetFindingType m_targetingType = TargetFindingType.Null;
        [SerializeField] private AimType m_aimingType = AimType.Null;
        [SerializeField] private RotationType m_rotationType = RotationType.Null;
        [SerializeField] private ConditionType m_conditionType = ConditionType.Null;
        [SerializeField] private ShootingType m_shootingType = ShootingType.Null;

        [SerializeField] private float m_rangeToFindEnemy = 20f;
        [SerializeField] private float m_rotationSpeed = 20f;
        [Tooltip("Max difference between the angles of the tower and the one facing the target (in fractions)")]
        [SerializeField] private float m_maxCannonAngleDifferenceForShooting = 0.1f;
        [SerializeField] private float m_shootInterval = 0.5f;
        [SerializeField] private PooledObjectType m_projectilePrefabType;
        [SerializeField] private float m_projectileSpeed = 20f;
        [SerializeField] private int m_projectileDamage = 10;

        public PooledObjectType towerType => m_towerType;
        public TargetFindingType targetingType => m_targetingType;
        public AimType aimingType => m_aimingType;
        public RotationType rotationType => m_rotationType;
        public ConditionType conditionType => m_conditionType;
        public ShootingType shootingType => m_shootingType;
        public float rangeToFindEnemy => m_rangeToFindEnemy;
        public float rotationSpeed => m_rotationSpeed;
        public float maxCannonAngleDifferenceForShooting => m_maxCannonAngleDifferenceForShooting;
        public float shootInterval => m_shootInterval;
        public PooledObjectType projectilePrefabType => m_projectilePrefabType;
        public float projectileSpeed => m_projectileSpeed;
        public int projectileDamage => m_projectileDamage;
    }

    #region Strategy's types enums
    public enum TargetFindingType
    {
        Null,
        GetClosest
    }
    public enum AimType
    {
        Null,
        Predicted,
        Direct
    }
    public enum RotationType
    {
        Null,
        Smooth
    }
    public enum ConditionType
    {
        Null,
        RotationToTargetReached,
        OnlyCooldownManaged
    }
    public enum ShootingType
    {
        Null,
        Predicted,
        Guided
    }
    #endregion
}