using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : BaseProjectile
{
	public void Launch(Vector3 shootDirection, float speed, int damage = 10)
	{
		base.Launch(speed, damage);
	}

	protected override void Move()
	{
		transform.position += transform.forward * (m_speed * Time.deltaTime);
	}
}