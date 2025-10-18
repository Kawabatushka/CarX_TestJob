using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
	[SerializeField] protected int m_damage = 10;
	[SerializeField] protected float m_speed = 0.2f;

	protected bool m_isLaunched = false;

	public virtual void Launch(float speed, int damage = 10)
	{
		m_speed = speed;
		m_damage = damage;
		m_isLaunched = true;
	}

	protected abstract void Move();

	protected virtual void Update()
	{
		if (m_isLaunched)
		{
			Move();
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		var enemy = other.GetComponent<Enemy>();
		if (enemy != null && enemy.isAlive)
		{
			enemy.ApplyDamage(m_damage);
			Destroy(gameObject);
		}
	}
}