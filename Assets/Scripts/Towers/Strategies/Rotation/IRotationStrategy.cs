using UnityEngine;

namespace Tower
{
	// пришлось разделить IRotationStrategy и IAimingStrategy, т к не все башни будут поворачиваться
	public interface IRotationStrategy
	{
		void RotateTower(Vector3 predictedPosition);
	}
}