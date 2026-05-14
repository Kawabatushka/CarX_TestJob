using UnityEngine;

namespace Tower
{
    public class CannonTowerRotationStrategy : IRotationStrategy
    {
        private readonly int m_towerSettingsId;

        public CannonTowerRotationStrategy(int towerSettingsId)
        {
            m_towerSettingsId = towerSettingsId;
        }

        public void RotateTower(
            Vector3 predictedPosition = default,
            Transform horizontalRotatingTowerPart = null,
            Transform verticalRotatingTowerPart = null
            )
        {
            if (horizontalRotatingTowerPart == null || verticalRotatingTowerPart == null)
            {
                return;
            }

            #region Горизонтальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 horizontalDirectionToTarget = predictedPosition - horizontalRotatingTowerPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (horizontalDirectionToTarget != Vector3.zero)
            {
                // Игнорируем разницу по Y для горизонтального вращения
                horizontalDirectionToTarget.y = 0;

                // Из направления получаем поворот
                Quaternion targetHorizontalRotation = Quaternion.LookRotation(horizontalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                horizontalRotatingTowerPart.rotation = Quaternion.RotateTowards(
                    horizontalRotatingTowerPart.rotation,
                    targetHorizontalRotation,
                    GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
                );
            }
            #endregion

            #region Вертикальный поворот
            // Направление, куда надо повернуть пушку
            Vector3 verticalDirectionToTarget = predictedPosition - verticalRotatingTowerPart.position;

            // Проверяем во избежание бесконечного приближения поворота пушки к directionToTarget
            if (verticalDirectionToTarget != Vector3.zero)
            {
                // Из направления получаем поворот
                Quaternion targetVerticalRotation = Quaternion.LookRotation(verticalDirectionToTarget);

                // Вращаем пушку к цели с постоянной скоростью (через Lerp() поворот будет не равномерным, а с отрицательным ускорением)
                verticalRotatingTowerPart.rotation = Quaternion.RotateTowards(
                    verticalRotatingTowerPart.rotation,
                    targetVerticalRotation,
                    GameConfig.instance.GetCannonTowerSettings(m_towerSettingsId).rotationSpeed * Time.deltaTime
                );
            }
            #endregion
        }
    }
}