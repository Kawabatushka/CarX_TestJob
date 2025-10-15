using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float interval = 2;
	public GameObject moveTarget;

	private float m_lastSpawn = -1;

	private void Update()
	{
		if (Time.time > m_lastSpawn + interval)
		{
			var newMonster = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			var r = newMonster.AddComponent<Rigidbody>();
			r.useGravity = false;
			newMonster.transform.position = transform.position;
			var monsterBeh = newMonster.AddComponent<Monster>();
			monsterBeh.m_moveTarget = moveTarget;

			m_lastSpawn = Time.time;
		}
	}
}