using UnityEngine;
using UnityEditor;

namespace Tools
{
    [CustomEditor(typeof(TowersDataConfig))]
    public class TowerManagerEditor : Editor
    {
        private bool[] _towerFoldouts;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty towersProp = serializedObject.FindProperty("m_towerPresets");

            if (_towerFoldouts == null || _towerFoldouts.Length != towersProp.arraySize)
            {
                _towerFoldouts = new bool[towersProp.arraySize];
            }

            EditorGUILayout.LabelField("Tower presets config", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            for (int i = 0; i < towersProp.arraySize; i++)
            {
                SerializedProperty towerProp = towersProp.GetArrayElementAtIndex(i);
                SerializedProperty towerTypeProp = towerProp.FindPropertyRelative("m_towerType");

                EditorGUILayout.BeginVertical(GUI.skin.box);

                string displayName = towerTypeProp.enumDisplayNames.Length > 0
                    && towerTypeProp.enumValueIndex >= 0
                    && towerTypeProp.enumValueIndex < towerTypeProp.enumDisplayNames.Length
                    ? towerTypeProp.enumDisplayNames[towerTypeProp.enumValueIndex]
                    : $"Preset {i}";
                _towerFoldouts[i] = EditorGUILayout.Foldout(_towerFoldouts[i], displayName, true, EditorStyles.foldoutHeader);

                if (_towerFoldouts[i])
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(towerTypeProp, new GUIContent("Tower Type"));
                    EditorGUILayout.Space(5);

                    DrawStrategyHeader("TARGET FINDING");
                    SerializedProperty targetingTypeProp = towerProp.FindPropertyRelative("m_targetingType");
                    EditorGUILayout.PropertyField(targetingTypeProp, new GUIContent("Targeting Type"));
                    DrawFieldIf(towerProp, "m_rangeToFindEnemy", "Range To Find Enemy",
                        IsTargetingGetClosest(targetingTypeProp));

                    DrawStrategyHeader("AIMING");
                    SerializedProperty aimingTypeProp = towerProp.FindPropertyRelative("m_aimingType");
                    SerializedProperty shootingTypeProp = towerProp.FindPropertyRelative("m_shootingType");
                    EditorGUILayout.PropertyField(aimingTypeProp, new GUIContent("Aiming Type"));
                    DrawFieldIf(towerProp, "m_projectileSpeed", "Projectile Speed",
                        IsProjectileSpeedInAimingSection(aimingTypeProp, shootingTypeProp));

                    DrawStrategyHeader("ROTATION");
                    SerializedProperty rotationTypeProp = towerProp.FindPropertyRelative("m_rotationType");
                    EditorGUILayout.PropertyField(rotationTypeProp, new GUIContent("Rotation Type"));
                    DrawFieldIf(towerProp, "m_rotationSpeed", "Rotation Speed",
                        IsRotationSmooth(rotationTypeProp));

                    DrawStrategyHeader("SHOOTING CONDITION");
                    SerializedProperty conditionTypeProp = towerProp.FindPropertyRelative("m_conditionType");
                    EditorGUILayout.PropertyField(conditionTypeProp, new GUIContent("Condition Type"));
                    DrawFieldIf(towerProp, "m_maxCannonAngleDifferenceForShooting", "Max Cannon Angle Difference For Shooting",
                        IsConditionRotationToTargetReached(conditionTypeProp));
                    DrawFieldIf(towerProp, "m_shootInterval", "Shoot Interval",
                        IsConditionShootIntervalVisible(conditionTypeProp));

                    DrawStrategyHeader("SHOOTING");
                    EditorGUILayout.PropertyField(shootingTypeProp, new GUIContent("Shooting Type"));
                    DrawFieldIf(towerProp, "m_projectilePrefabType", "Projectile Prefab Type",
                        IsShootingPredictedOrGuided(shootingTypeProp));
                    DrawFieldIf(towerProp, "m_projectileSpeed", "Projectile Speed",
                        IsProjectileSpeedInShootingSection(shootingTypeProp));
                    DrawFieldIf(towerProp, "m_projectileDamage", "Projectile Damage",
                        IsShootingPredictedOrGuided(shootingTypeProp));

                    EditorGUILayout.Space(10);

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Delete Preset"))
                    {
                        towersProp.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.Space(5);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Add New Preset", GUILayout.Height(30)))
            {
                towersProp.arraySize++;
            }
            GUI.backgroundColor = Color.white;

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawFieldIf(SerializedProperty parent, string propertyName, string label, bool condition)
        {
            if (!condition)
                return;

            SerializedProperty prop = parent.FindPropertyRelative(propertyName);
            if (prop != null)
                EditorGUILayout.PropertyField(prop, new GUIContent(label));
        }

        private static bool IsTargetingGetClosest(SerializedProperty targetingTypeProp) =>
            (TargetFindingType)targetingTypeProp.enumValueIndex == TargetFindingType.GetClosest;

        private static bool IsRotationSmooth(SerializedProperty rotationTypeProp) =>
            (RotationType)rotationTypeProp.enumValueIndex == RotationType.Smooth;

        private static bool IsConditionRotationToTargetReached(SerializedProperty conditionTypeProp) =>
            (ConditionType)conditionTypeProp.enumValueIndex == ConditionType.RotationToTargetReached;

        private static bool IsConditionShootIntervalVisible(SerializedProperty conditionTypeProp)
        {
            var conditionType = (ConditionType)conditionTypeProp.enumValueIndex;
            return conditionType == ConditionType.RotationToTargetReached
                || conditionType == ConditionType.OnlyCooldownManaged;
        }

        private static bool IsShootingPredictedOrGuided(SerializedProperty shootingTypeProp)
        {
            var shootingType = (ShootingType)shootingTypeProp.enumValueIndex;
            return shootingType == ShootingType.Predicted || shootingType == ShootingType.Guided;
        }

        private static bool IsProjectileSpeedInAimingSection(SerializedProperty aimingTypeProp, SerializedProperty shootingTypeProp) =>
            (AimType)aimingTypeProp.enumValueIndex == AimType.Predicted/* 
            && !IsShootingPredictedOrGuided(shootingTypeProp) */;

        private static bool IsProjectileSpeedInShootingSection(SerializedProperty shootingTypeProp) =>
            IsShootingPredictedOrGuided(shootingTypeProp);

        private static void DrawStrategyHeader(string title)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField(title, EditorStyles.miniBoldLabel);
        }
    }
}