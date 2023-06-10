#if UNITY_EDITOR
using System;

using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from ModLunar ( https://forum.unity.com/threads/solved-duplicate-prefab-issue.778553/ )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	[ExecuteInEditMode]
	public static class DuplicateGameObject {

		/// <summary>요청한 타입의 첫번째 윈도우 창 오브젝트를 반환합니다.</summary>
		/// <returns>해당 타입의 첫번째 윈도우</returns>
		public static EditorWindow FindFirstWindow(Type EditorWindowType) {
			if (EditorWindowType == null)
				throw new ArgumentNullException(nameof(EditorWindowType));
			if (!typeof(EditorWindow).IsAssignableFrom(EditorWindowType))
				throw new ArgumentException("The given type (" + EditorWindowType.Name + ") does not inherit from " + nameof(EditorWindow) + ".");
			Object[] TypeOpenWindows = Resources.FindObjectsOfTypeAll(EditorWindowType);
			if (TypeOpenWindows.Length <= 0) return null;
			EditorWindow Window = (EditorWindow)TypeOpenWindows[0];
			return Window;
		}

		/// <summary>요청한 오브젝트의 복제 GameObject를 반환합니다.</summary>
		/// <returns>오브젝트의 복제 GameObject</returns>
		public static GameObject DuplicateGameObjectInstance(GameObject GameObjectInstance) {
			Selection.objects = new Object[] { GameObjectInstance };
			Selection.activeGameObject = GameObjectInstance;
			Type hierarchyViewType = Type.GetType("UnityEditor.SceneHierarchyWindow, UnityEditor");
			EditorWindow hierarchyView = FindFirstWindow(hierarchyViewType);
			hierarchyView.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
			GameObject CloneGameObject = Selection.activeGameObject;
			return CloneGameObject;
		}
	}
}
#endif