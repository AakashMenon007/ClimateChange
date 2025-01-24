using UnityEditor;
using UnityEngine;
using static SceneTransitionFXPro.TransitionManager;

namespace SceneTransitionFXPro
{
    [CustomEditor(typeof(TransitionManager))]
    public class TransitionEditor : Editor
    {
        SerializedProperty selectedEffect;
        SerializedProperty transitionSpeed;
        SerializedProperty cubeDirection;
        SerializedProperty spinDuration;
        SerializedProperty slideDirection;
        SerializedProperty wipeDirection;
        SerializedProperty popType;
        SerializedProperty flowDirection;
        SerializedProperty fadeInAfter;
        SerializedProperty zoomInAfter;
        SerializedProperty zRotationAmount;
        SerializedProperty reverseZRotationAmount;
        SerializedProperty glitchDuration;  // For Glitch Transition

        SerializedProperty currentScenePanel;
        SerializedProperty otherScenePanel;
        SerializedProperty sceneLoader;
        SerializedProperty mainCamera;

        void OnEnable()
        {
            // Find and assign all the serialized properties, ensuring no property is missing.
            selectedEffect = serializedObject.FindProperty("selectedEffect");
            transitionSpeed = serializedObject.FindProperty("transitionSpeed");
            cubeDirection = serializedObject.FindProperty("cubeDirection");
            spinDuration = serializedObject.FindProperty("spinDuration");
            slideDirection = serializedObject.FindProperty("slideDirection");
            wipeDirection = serializedObject.FindProperty("wipeDirection");
            popType = serializedObject.FindProperty("popType");
            flowDirection = serializedObject.FindProperty("flowDirection");
            fadeInAfter = serializedObject.FindProperty("fadeInAfter");
            zoomInAfter = serializedObject.FindProperty("zoomInAfter");
            zRotationAmount = serializedObject.FindProperty("zRotationAmount");
            reverseZRotationAmount = serializedObject.FindProperty("reverseZRotationAmount");
            glitchDuration = serializedObject.FindProperty("glitchDuration");  // Init glitch duration

            currentScenePanel = serializedObject.FindProperty("currentScenePanel");
            otherScenePanel = serializedObject.FindProperty("otherScenePanel");
            sceneLoader = serializedObject.FindProperty("sceneLoader");
            mainCamera = serializedObject.FindProperty("mainCamera");
        }

        public override void OnInspectorGUI()
        {
            // Prevent null properties from causing issues.
            if (selectedEffect == null || transitionSpeed == null)
            {
                Debug.LogError("One or more SerializedProperties are null.");
                return;
            }

            serializedObject.Update();

            // Display the transition effect options
            EditorGUILayout.PropertyField(selectedEffect, new GUIContent("Selected Effect"));
            EditorGUILayout.PropertyField(transitionSpeed, new GUIContent("Transition Speed"));

            // Display slide direction options if Slide is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Slide)
            {
                EditorGUILayout.PropertyField(slideDirection, new GUIContent("Slide Direction"));
            }

            // Display wipe direction options if Wipe is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Wipe)
            {
                EditorGUILayout.PropertyField(wipeDirection, new GUIContent("Wipe Direction"));
            }

            // Display fade-specific settings if Fade is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Fade)
            {
                EditorGUILayout.PropertyField(fadeInAfter, new GUIContent("Fade In After (Seconds)"));
            }

            // Display zoom-specific settings if Zoom is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Zoom)
            {
                EditorGUILayout.PropertyField(zoomInAfter, new GUIContent("Zoom In After (Seconds)"));
            }

            // Display spin duration if Spin with Fade is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.SpinWithFade)
            {
                EditorGUILayout.PropertyField(spinDuration, new GUIContent("Spin Duration (Seconds)"));
            }

            // Display options for CardFlip Transition
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.CardFlip)
            {
                EditorGUILayout.HelpBox("CardFlip Transition has no additional options.", MessageType.Info);
            }

            // Display options for Cube transition
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Cube)
            {
                EditorGUILayout.PropertyField(cubeDirection, new GUIContent("Cube Direction"));
            }

            // Display options for Pop transition
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Pop)
            {
                EditorGUILayout.PropertyField(popType, new GUIContent("Pop Type"));
            }

            // Display options for Smooth Flow transition
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.SmoothFlow)
            {
                EditorGUILayout.PropertyField(flowDirection, new GUIContent("Flow Direction"));
                EditorGUILayout.PropertyField(zRotationAmount, new GUIContent("Z Rotation Leave Scene"));
                EditorGUILayout.PropertyField(reverseZRotationAmount, new GUIContent("Z Rotation Enter Scene"));
            }

            // Display glitch duration if Glitch is selected
            if (selectedEffect.enumValueIndex == (int)TransitionManager.TransitionEffect.Glitch)
            {
                EditorGUILayout.PropertyField(glitchDuration, new GUIContent("Glitch Duration (Seconds)"));
            }


            // Display scene panels and scene loader assignments
            EditorGUILayout.PropertyField(currentScenePanel, new GUIContent("Current Scene Panel"));
            EditorGUILayout.PropertyField(otherScenePanel, new GUIContent("Next Scene Panel"));
            EditorGUILayout.PropertyField(sceneLoader, new GUIContent("Scene Loader"));
            EditorGUILayout.PropertyField(mainCamera, new GUIContent("Main Camera"));

            // Validation Checks: Warn user if critical fields are missing
            if (currentScenePanel.objectReferenceValue == null || otherScenePanel.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Current Scene Panel and Next Scene Panel must be assigned.", MessageType.Warning);
            }
            if (sceneLoader.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Scene Loader must be assigned.", MessageType.Warning);
            }
            if (mainCamera.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Main Camera must be assigned for 3D transitions.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}