using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	[System.Serializable] public class EnemyEvent : UnityEvent<Enemy> { }

	public EnemyEvent OnDied = new EnemyEvent();
	public EnemyEvent OnDamageTaken = new EnemyEvent();

	[SerializeField] private Transform m_moveTarget;
	[SerializeField] private float m_speed = 10f;
	[SerializeField] private int m_maxHP = 30;
	private const float REACH_DISTANCE = 0.3f;

	private int m_currentHP;
	private bool m_isAlive = true;

	public Vector3 moveTargetPosition => m_moveTarget != null ? m_moveTarget.position : Vector3.zero;
	public bool hasReachedTarget => Vector3.Distance(transform.position, moveTargetPosition) <= REACH_DISTANCE;
	public bool isAlive => m_isAlive;
	public float healthPercent => (float)m_currentHP / m_maxHP;
	public int currentHP => m_currentHP;
	public int maxHP => m_maxHP;


	private void Start()
	{
		m_currentHP = m_maxHP;

		if (m_moveTarget == null)
		{
			Debug.LogError($"Move target = null", this);
		}
	}

	private void Update()
	{
		if (m_moveTarget == null || !isAlive)
		{
			return;
		}

		if (hasReachedTarget)
		{
			HandleTargetReached();
			return;
		}

		MoveTowardsTarget();
	}

	private void HandleTargetReached()
	{
		Debug.Log($"Враг дошел до цели и был уничтожен");
		Die();
	}

	private void Die()
	{
		m_isAlive = false;
		OnDied?.Invoke(this);
		Destroy(gameObject);
	}

	private void MoveTowardsTarget()
	{
		Vector3 direction = (moveTargetPosition - transform.position).normalized;
		transform.position += direction * (m_speed * Time.deltaTime);
	}

	public void SetMoveTarget(Transform target)
	{
		m_moveTarget = target;
	}

	public void ApplyDamage(int damage)
	{
		if (!m_isAlive) return;

		m_currentHP -= damage;
		OnDamageTaken?.Invoke(this);

		if (m_currentHP <= 0)
		{
			Die();
		}
	}
}