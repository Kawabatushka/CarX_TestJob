using UnityEngine;

namespace Pooling
{
	public interface IObjectPool
	{
		GameObject Get(bool isActiveInstance = true);
		void Release(GameObject element);
	}
}