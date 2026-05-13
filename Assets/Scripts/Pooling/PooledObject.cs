using UnityEngine;

namespace Pooling
{
	// Класс будет использоваться, как компонент для спавна и возврата объетов в пул
	// Мб если объект не содержит компонент PooledObject, то его просто удалять при достижении цели
	// TO-DO-R: надо еще возвращать объекты пула спустя время, если они не достигли цели
	public sealed class PooledObject : MonoBehaviour
	{
		[SerializeField] private GameObject m_prefabKey;

		public GameObject prefabKey => m_prefabKey;

		public void Initialize(GameObject prefabKey)
		{
			m_prefabKey = prefabKey;
		}
	}
}