using System;
using UnityEngine;

namespace Tower
{
    public class CannonTowerRotationStrategy : IRotatable
    {
        private readonly Transform m_horizontalPart;
        private readonly Transform m_verticalPart;
        private readonly float m_rotationSpeed;
		private readonly Func<Vector3> m_getPredictedPosition; // подсказка от ИИ

        public CannonTowerRotationStrategy(
            Transform horizontalPart,
            Transform verticalPart,
            float rotationSpee,
            Func<Vector3> predictedPosition)
        {
            m_horizontalPart = horizontalPart;
            m_verticalPart = verticalPart;
            m_rotationSpeed = rotationSpee;
            m_getPredictedPosition = predictedPosition;
        }

        public void RotateTower()
        {
            Vector3 m_predictedPosition = m_getPredictedPosition();
            #region Горизонтальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 horizontalDirectionToTarget = m_predictedPosition - m_horizontalPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (horizontalDirectionToTarget != Vector3.zero)
            {
                // Игнорируем разницу по Y для горизонтального вращения
                horizontalDirectionToTarget.y = 0;

                // Из направления получаем поворот
                Quaternion targetHorizontalRotation = Quaternion.LookRotation(horizontalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                m_horizontalPart.rotation = Quaternion.RotateTowards(
                    m_horizontalPart.rotation,
                    targetHorizontalRotation,
                    m_rotationSpeed * Time.deltaTime
                );
            }
            #endregion

            #region Вертикальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 verticalDirectionToTarget = m_predictedPosition - m_verticalPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (verticalDirectionToTarget != Vector3.zero)
            {
                // Из направления получаем поворот
                Quaternion targetVerticalRotation = Quaternion.LookRotation(verticalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                m_verticalPart.rotation = Quaternion.RotateTowards(
                    m_verticalPart.rotation,
                    targetVerticalRotation,
                    m_rotationSpeed * Time.deltaTime
                );
            }
            #endregion
        }

    }
}