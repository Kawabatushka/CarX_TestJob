using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour
{
	public float shootInterval = 2f;
	public float range = 10f;
	public GameObject projectilePrefab;
	public Transform shootPoint;

	private float m_lastShotTime = -0.5f;

	private void Update()
	{
		if (projectilePrefab == null || shootPoint == null)
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
			Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

			m_lastShotTime = Time.time;
		}

	}
}