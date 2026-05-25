using UnityEngine;
using UnityEngine.Events;
using Pooling;
using Tools;

namespace Enemy
{
	public class SimpleEnemy : MonoBehaviour, IPoolable
	{
		[System.Serializable] public class EnemyEvent : UnityEvent<SimpleEnemy> { }
		public EnemyEvent OnDied = new EnemyEvent();

		private Transform m_moveTarget;
		private const float ReachDistance = 0.3f;

		private int m_currentHP;
		private bool m_isAlive = true;

		public Vector3 moveTargetPosition => m_moveTarget != null ? m_moveTarget.position : Vector3.zero;
		public Vector3 velocity => moveTargetPosition != null ? (moveTargetPosition - transform.position).normalized * EnemyConfig.instance.enemyData.speed : Vector3.zero;
		public bool hasReachedTarget => Vector3.Distance(transform.position, moveTargetPosition) <= ReachDistance;
		public bool isAlive => m_isAlive;

		public void OnSpawned()
		{
			m_isAlive = true;
			m_currentHP = EnemyConfig.instance.enemyData.maxHP;
		}

		public void OnDespawned()
		{
			m_moveTarget = null;
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
			Debug.Log($"Enemy reached the target and was destroyed", this);
			Die();
		}

		private void Die()
		{
			m_isAlive = false;
			m_currentHP = 0;
			OnDied?.Invoke(this);
			PoolManager.instance?.Release(gameObject);
		}

		private void MoveTowardsTarget()
		{
			Vector3 direction = (moveTargetPosition - transform.position).normalized;
			transform.position += direction * (EnemyConfig.instance.enemyData.speed * Time.deltaTime);
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
}