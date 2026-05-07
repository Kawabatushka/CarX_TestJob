using UnityEngine;

namespace Pooling
{
	public interface IObjectPool
	{
		GameObject Get();
		void Release(GameObject element);
	}
}