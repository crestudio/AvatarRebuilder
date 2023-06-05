using UnityEngine;
using UnityEditor;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

    [CustomEditor(typeof(AvatarRebuilder))]
    public class AvatarRebuilderEditor : Editor {

        SerializedProperty SerializedNewAvatarGameObject;
        SerializedProperty SerializedOldAvatarGameObject;
        SerializedProperty SerializedNewAvatarSkinnedMeshRenderers;

        SerializedProperty SerializedAvatarRootBone;
        SerializedProperty SerializedToggleRestoreArmatureTransform;
        SerializedProperty SerializedToggleResetRestPose;
        SerializedProperty SerializedToggleReorderGameObject;


        public static string[] AvatarType = new string[0];
        public static int AvatarTypeIndex = 0;
		SerializedProperty SerializedStatusString;

		public static int LanguageIndex = 0;
        public static readonly string[] LanguageType = new[] { "English", "한국어", "日本語" };

        void OnEnable() {
            SerializedNewAvatarGameObject = serializedObject.FindProperty("NewAvatarGameObjectEditor");
            SerializedOldAvatarGameObject = serializedObject.FindProperty("OldAvatarGameObjectEditor");
            SerializedNewAvatarSkinnedMeshRenderers = serializedObject.FindProperty("NewAvatarSkinnedMeshRenderersEditor");

            SerializedAvatarRootBone = serializedObject.FindProperty("AvatarRootBoneEditor");
            SerializedToggleRestoreArmatureTransform = serializedObject.FindProperty("ToggleRestoreArmatureTransformEditor");
            SerializedToggleResetRestPose = serializedObject.FindProperty("ToggleResetRestPoseEditor");
            SerializedToggleReorderGameObject = serializedObject.FindProperty("ToggleReorderGameObjectEditor");

			SerializedStatusString = serializedObject.FindProperty("StatusStringEditor");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            AvatarType = LanguageHelper.GetAvatarString();
			LanguageIndex = EditorGUILayout.Popup(LanguageHelper.GetContextString("String_Language"), LanguageIndex, LanguageType);
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(SerializedOldAvatarGameObject, new GUIContent(LanguageHelper.GetContextString("String_OriginalAvatar")));
            GUI.enabled = true;
            EditorGUILayout.PropertyField(SerializedNewAvatarGameObject, new GUIContent(LanguageHelper.GetContextString("String_NewAvatar")));
            AvatarTypeIndex = EditorGUILayout.Popup(LanguageHelper.GetContextString("String_AvatarType"), AvatarTypeIndex, AvatarType);
            (target as AvatarRebuilder).TargetAvatarIndexEditor = AvatarTypeIndex;
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(SerializedAvatarRootBone, new GUIContent(LanguageHelper.GetContextString("String_RootBone")));
            GUI.enabled = true;
            EditorGUILayout.PropertyField(SerializedToggleRestoreArmatureTransform, new GUIContent(LanguageHelper.GetContextString("String_RestoreTransform")));
            EditorGUILayout.PropertyField(SerializedToggleResetRestPose, new GUIContent(LanguageHelper.GetContextString("String_RestPose")));
            EditorGUILayout.PropertyField(SerializedToggleReorderGameObject, new GUIContent(LanguageHelper.GetContextString("String_ReorderGameObject")));
            GUI.enabled = false;
            EditorGUILayout.PropertyField(SerializedNewAvatarSkinnedMeshRenderers, new GUIContent(LanguageHelper.GetContextString("String_SkinnedMeshRendererList")));
            GUI.enabled = true;
            if (GUILayout.Button(LanguageHelper.GetContextString("String_ImportSkinnedMeshRenderer"))) {
                (target as AvatarRebuilder).UpdateSkinnedMeshRendererList();
            }
			if (!string.IsNullOrEmpty(SerializedStatusString.stringValue)) {
				EditorGUILayout.HelpBox(LanguageHelper.GetContextString(SerializedStatusString.stringValue), MessageType.Warning);
			}
			serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUILayout.HelpBox(LanguageHelper.GetContextString("String_Warning"), MessageType.Warning);
            if (GUILayout.Button(LanguageHelper.GetContextString("String_ReplaceAvatar"))) {
                (target as AvatarRebuilder).ReplaceSkinnedMeshRendererGameObjects();
            }
        }
    }
}