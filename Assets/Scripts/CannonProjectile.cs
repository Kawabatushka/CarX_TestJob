using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CannonProjectile : BaseProjectile
{
	[SerializeField] private bool m_useGravity = false;
	private Vector3 m_velocity;
	private float m_gravity = Physics.gravity.y;

	public void Launch(float speed, int damage = 10, bool useGravity = false)
	{
		base.Launch(speed, damage);
		m_useGravity = useGravity;
		m_velocity = transform.forward * m_speed;
	}

	protected override void Move()
	{
		if (m_useGravity)
		{
			// Параболическая траектория с гравитацией
			m_velocity += Vector3.down * (m_gravity * Time.deltaTime);
			transform.position += m_velocity * Time.deltaTime;

			// Поворот снаряда по направлению движения
			if (m_velocity != Vector3.zero)
			{
				transform.rotation = Quaternion.LookRotation(m_velocity.normalized);
			}
		}
		else
		{
			// Прямолинейное движение
			transform.position += transform.forward * (m_speed * Time.deltaTime);
		}
	}
}