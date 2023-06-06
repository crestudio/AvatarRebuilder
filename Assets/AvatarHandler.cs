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

		/// <summary>
		/// 본 프로그램의 메인 세팅 로직입니다.
		/// </summary>
		internal static void RequestCheckNewAvatar() {
			if (!IsSameFBX()) {
				if(!IsExistNewAvatarInScene()) {
					Debug.Log("Create New Avatar into Scene");
				}
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
			OldAvatarAnimator = OldAvatarGameObject.GetComponent<Animator>();
			AvatarRootBone = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
			return;
		}

		private static bool IsSameFBX() {
			bool Result = false;
			UnityEngine.Avatar OldAnimatorAvatar = OldAvatarAnimator.avatar;
			UnityEngine.Avatar NewAnimatorAvatar = NewAvatarAnimator.avatar;
			string OldAvatarAssetPath = AssetDatabase.GetAssetPath(OldAnimatorAvatar);
			string NewAvatarAssetPath = AssetDatabase.GetAssetPath(NewAnimatorAvatar);
			if (OldAvatarAssetPath == NewAvatarAssetPath) Result = true;
			return Result;
		}

		private static bool IsExistNewAvatarInScene() {
			return NewAvatarGameObject.scene.IsValid();
		}
	}
}
#endif