using UnityEngine;

public class CannonProjectile : BaseProjectile
{
	protected override void Move()
	{
		transform.position += transform.forward * (m_speed * Time.deltaTime);
	}
}