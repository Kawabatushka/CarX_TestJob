using UnityEngine;

namespace Pooling
{
	public interface IObjectPool
	{
		Component Get();
		void Release(Component element);
	}
}