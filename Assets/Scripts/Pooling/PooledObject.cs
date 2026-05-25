using UnityEngine;

namespace Pooling
{
	// Класс будет использоваться, как компонент-флаг для спавна и возврата объетов в пул
	public sealed class PooledObject : MonoBehaviour
	{
		[SerializeField] private PooledObjectType m_prefabType;

		public PooledObjectType prefabType => m_prefabType;

		public void Initialize(PooledObjectType prefabType)
		{
			m_prefabType = prefabType;
		}
	}
}