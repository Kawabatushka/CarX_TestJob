using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	/// <summary>
	/// UI-бар здоровья врага (fill image) и опциональный текст текущего HP
	/// </summary>
	[System.Serializable] public class EnemyEvent : UnityEvent<Enemy> { }
	public EnemyEvent OnDied = new EnemyEvent();

	private Transform m_moveTarget;
	private const float REACH_DISTANCE = 0.3f;

	private int m_currentHP;
	private bool m_isAlive = true;

	public Vector3 moveTargetPosition => m_moveTarget != null ? m_moveTarget.position : Vector3.zero;
	public Vector3 Velocity => moveTargetPosition != null ? (moveTargetPosition - transform.position).normalized * GameConfig.instance.enemyData.speed : Vector3.zero;
	public bool hasReachedTarget => Vector3.Distance(transform.position, moveTargetPosition) <= REACH_DISTANCE;
	public bool isAlive => m_isAlive;

	[Header("HP UI")]
	[SerializeField] private Image hpBarFill;
	[SerializeField] private TextMeshProUGUI hpText;

	private void Start()
	{
		m_currentHP = GameConfig.instance.enemyData.maxHP;
		UpdateHPUI();
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
		UpdateHPUI();
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
		UpdateHPUI();

		if (m_currentHP <= 0)
		{
			Die();
		}
	}

	private void UpdateHPUI()
	{
		int maxHP = Math.Max(1, GameConfig.instance.enemyData.maxHP);
		if (hpBarFill != null)
		{
			hpBarFill.fillAmount = Mathf.Clamp01((float)m_currentHP / maxHP);
		}

		if (hpText != null)
		{
			hpText.text = $"{Mathf.Max(0, m_currentHP)}/{maxHP}";
		}
	}
}