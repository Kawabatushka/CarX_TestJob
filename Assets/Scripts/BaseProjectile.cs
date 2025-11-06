using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
	protected int m_damage;
	protected float m_speed;

	protected bool m_isLaunched = false;

	public virtual void Launch(float speed, int damage)
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