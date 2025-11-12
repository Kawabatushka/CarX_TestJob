using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	[System.Serializable] public class EnemyEvent : UnityEvent<Enemy> { }
	public EnemyEvent OnDied = new EnemyEvent();

	private Transform m_moveTarget;
	private const float ReachDistance = 0.3f;

	private int m_currentHP;
	private bool m_isAlive = true;

	public Vector3 moveTargetPosition => m_moveTarget != null ? m_moveTarget.position : Vector3.zero;
	public Vector3 velocity => moveTargetPosition != null ? (moveTargetPosition - transform.position).normalized * GameConfig.instance.enemyData.speed : Vector3.zero;
	public bool hasReachedTarget => Vector3.Distance(transform.position, moveTargetPosition) <= ReachDistance;
	public bool isAlive => m_isAlive;

	private void Start()
	{
		m_currentHP = GameConfig.instance.enemyData.maxHP;
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
		Debug.Log($"Враг дошел до цели и был уничтожен", this);
		Die();
	}

	private void Die()
	{
		m_isAlive = false;
		m_currentHP = 0;
		OnDied?.Invoke(this);
		Destroy(gameObject);
	}

	private void MoveTowardsTarget()
	{
		Vector3 direction = (moveTargetPosition - transform.position).normalized;
		transform.position += direction * (GameConfig.instance.enemyData.speed * Time.deltaTime);
	}

	public void SetMoveTarget(Transform target)
	{
		m_moveTarget = target;
	}

	public void ApplyDamage(int damage)
	{
		if (!m_isAlive) return;

		m_currentHP -= damage;

		if (m_currentHP <= 0)
		{
			Die();
		}
	}
}