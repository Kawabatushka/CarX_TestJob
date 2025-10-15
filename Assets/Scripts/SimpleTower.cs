using UnityEngine;
using System.Collections;

public class SimpleTower : MonoBehaviour
{
	public float shootInterval = 1f;
	public float range = 10f;
	public GameObject projectilePrefab;

	private float m_lastShotTime = -0.5f;

	private void Update()
	{
		if (projectilePrefab == null)
		{
			return;
		}

		foreach (var monster in FindObjectsOfType<Monster>())
		{
			if (Vector3.Distance(transform.position, monster.transform.position) > range)
			{
				continue;
			}

			if (m_lastShotTime + shootInterval > Time.time)
			{
				continue;
			}

			// shot
			var projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;
			var projectileBeh = projectile.GetComponent<GuidedProjectile>();
			projectileBeh.target = monster.gameObject;

			m_lastShotTime = Time.time;
		}

	}
}