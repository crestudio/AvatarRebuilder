#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from emilianavt/ReassignBoneWeigthsToNewMesh.cs ( https://gist.github.com/emilianavt/721cd4dd2d4a62ba54b002b63f894dbf )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class AvatarHandler : AvatarRebuilder {

		private enum AvatarStatus {
			Patched, NeedPatch
		}

		/// <summary>
		/// 본 프로그램의 메인 세팅 로직입니다.
		/// </summary>
		internal static void RequestCheckNewAvatar() {
			switch (CheckNewAvatarStatus()) {
				case AvatarStatus.NeedPatch:
					break;
				case AvatarStatus.Patched:
					break;
			}
			return;
		}

		internal static void CreateDuplicateAvatar() {
			Undo.RecordObject(OldAvatarGameObject, "Duplicated Old Avatar");
			GameObject DuplicatedAvatar = Instantiate(OldAvatarGameObject);
			Undo.RegisterCreatedObjectUndo(DuplicatedAvatar, "Duplicated Old Avatar");
			DuplicatedAvatar.name = OldAvatarGameObject.name;
			OldAvatarGameObject.name = DuplicatedAvatar.name + "(Backup)";
			OldAvatarGameObject.SetActive(false);
			Undo.CollapseUndoOperations(UndoGroupIndex);

			OldAvatarGameObject = DuplicatedAvatar;
			return;
		}

		private static AvatarStatus CheckNewAvatarStatus() {
			AvatarStatus Result = AvatarStatus.NeedPatch;
			return Result;
		}
	}
}
#endif