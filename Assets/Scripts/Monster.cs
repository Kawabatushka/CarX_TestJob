using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
	public GameObject m_moveTarget;
	public float speed = 0.1f;
	public int maxHP = 30;
	const float m_ReachDistance = 0.3f;

	public int hp;

	private void Start()
	{
		hp = maxHP;
	}

	private void Update()
	{
		if (m_moveTarget == null)
		{
			return;
		}

		if (Vector3.Distance(transform.position, m_moveTarget.transform.position) <= m_ReachDistance)
		{
			Destroy(gameObject);
			return;
		}

		var translation = m_moveTarget.transform.position - transform.position;
		if (translation.magnitude > speed)
		{
			translation = translation.normalized * speed;
		}
		transform.Translate(translation);
	}
}