using UnityEngine;

namespace Tower
{
    public class CannonTowerRotationStrategy : IRotationStrategy
    {
        private readonly Transform m_horizontalRotatingTowerPart;
        private readonly Transform m_verticalRotatingTowerPart;
        private readonly int m_towerSettingsId;

        public CannonTowerRotationStrategy(
            Transform horizontalRotatingTowerPart,
            Transform verticalRotatingTowerPart,
            int towerSettingsId
        )
        {
            m_horizontalRotatingTowerPart = horizontalRotatingTowerPart;
            m_verticalRotatingTowerPart = verticalRotatingTowerPart;
            m_towerSettingsId = towerSettingsId;
        }

        public void RotateTower(Vector3 predictedPosition)
        {
            #region Горизонтальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 horizontalDirectionToTarget = predictedPosition - m_horizontalRotatingTowerPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (horizontalDirectionToTarget != Vector3.zero)
            {
                // Игнорируем разницу по Y для горизонтального вращения
                horizontalDirectionToTarget.y = 0;

                // Из направления получаем поворот
                Quaternion targetHorizontalRotation = Quaternion.LookRotation(horizontalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                m_horizontalRotatingTowerPart.rotation = Quaternion.RotateTowards(
                    m_horizontalRotatingTowerPart.rotation,
                    targetHorizontalRotation,
                    GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
                );
            }
            #endregion

            #region Вертикальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 verticalDirectionToTarget = predictedPosition - m_verticalRotatingTowerPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (verticalDirectionToTarget != Vector3.zero)
            {
                // Из направления получаем поворот
                Quaternion targetVerticalRotation = Quaternion.LookRotation(verticalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                m_verticalRotatingTowerPart.rotation = Quaternion.RotateTowards(
                    m_verticalRotatingTowerPart.rotation,
                    targetVerticalRotation,
                    GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
                );
            }
            #endregion
        }
    }
}