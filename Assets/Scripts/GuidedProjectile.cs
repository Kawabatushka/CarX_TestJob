using UnityEngine;
using System.Collections;

public class GuidedProjectile : BaseProjectile
{
	private GameObject m_target;

	public void Launch(GameObject target, float speed, int damage = 0)
	{
		base.Launch(speed, damage);
		m_target = target;
	}

	protected override void Move()
	{
		if (m_target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 translation = m_target.transform.position - transform.position;
		if (translation.magnitude > m_speed * Time.deltaTime)
		{
			translation = translation.normalized * (m_speed * Time.deltaTime);
		}
		transform.Translate(translation, Space.World);
	}
}