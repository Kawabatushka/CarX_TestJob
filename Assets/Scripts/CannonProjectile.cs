using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour
{
	public float speed = 2f;
	public int damage = 10;

	private void Update()
	{
		var translation = transform.forward * speed;
		transform.Translate(translation);
	}

	private void OnTriggerEnter(Collider other)
	{
		var monster = other.gameObject.GetComponent<Monster>();
		if (monster == null)
		{
			return;
		}

		monster.hp -= damage;
		if (monster.hp <= 0)
		{
			Destroy(monster.gameObject);
		}
		Destroy(gameObject);
	}
}