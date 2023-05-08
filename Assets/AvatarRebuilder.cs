#if UNITY_EDITOR
using UnityEngine;

/*
 * VRSuya Reassign Bone In SkinnedMeshRenderer for Mogumogu Project
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from emilianavt/ReassignBoneWeigthsToNewMesh.cs ( https://gist.github.com/emilianavt/721cd4dd2d4a62ba54b002b63f894dbf )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace VRSuya.AvatarRebuilder {

    [ExecuteInEditMode]
	[AddComponentMenu("VRSuya Avatar Rebuilder")]
	public class AvatarRebuilder : MonoBehaviour {

		// 에디터용 번수
		public GameObject NewAvatarGameObjectEditor = null;
		public GameObject OldAvatarGameObjectEditor = null;
		public int TargetAvatarIndexEditor = 0;
		public SkinnedMeshRenderer[] NewAvatarSkinnedMeshRenderersEditor = new SkinnedMeshRenderer[0];
		public Transform AvatarRootBoneEditor = null;
		public bool ToggleRestoreArmatureTransformEditor = true;
		public bool ToggleResetRestPoseEditor = false;
		public bool ToggleReorderGameObjectEditor = true;

		// 정적 변수
		protected static GameObject NewAvatarGameObject;
		protected static Animator NewAvatarAnimator;
		protected static GameObject OldAvatarGameObject;
		protected static Animator OldAvatarAnimator;
		protected static SkinnedMeshRenderer[] NewAvatarSkinnedMeshRenderers;

		protected static AvatarType TargetAvatar;
        protected enum AvatarType {
			Generic,
			Komado,
			SELESTIA,
			Yoll
		};

		protected static Transform AvatarRootBone;
		protected static bool ToggleRestoreArmatureTransform;
		protected static bool ToggleResetRestPose;
		protected static bool ToggleReorderGameObject;

		public string StatusString;

        void OnEnable() {
			if (!OldAvatarGameObjectEditor) {
				OldAvatarGameObjectEditor = this.gameObject;
            }
            if (OldAvatarGameObjectEditor.GetComponent<Animator>()) {
                OldAvatarAnimator = OldAvatarGameObjectEditor.GetComponent<Animator>();
            }
            if (OldAvatarAnimator) {
                if (OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips)) {
					AvatarRootBoneEditor = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
				}
            }
            SetStaticVariable();
		}

		// 에디터 변수 -> 정적 변수 동기화
		private void SetStaticVariable() {
			NewAvatarGameObject = NewAvatarGameObjectEditor;
			OldAvatarGameObject = OldAvatarGameObjectEditor;
			TargetAvatar = (AvatarType)TargetAvatarIndexEditor;
			NewAvatarSkinnedMeshRenderers = NewAvatarSkinnedMeshRenderersEditor;
			AvatarRootBone = AvatarRootBoneEditor;
            ToggleRestoreArmatureTransform = ToggleRestoreArmatureTransformEditor;
			return;
		}

		// 정적 변수 -> 에디터 변수 동기화
		private void SetEditorVariable() {
			NewAvatarGameObjectEditor = NewAvatarGameObject;
			OldAvatarGameObjectEditor = OldAvatarGameObject;
			NewAvatarSkinnedMeshRenderersEditor = NewAvatarSkinnedMeshRenderers;
			AvatarRootBoneEditor = AvatarRootBone;
			return;
		}

		// 패치 대상 SkinnedMeshRenderer 목록 얻기
		public void UpdateSkinnedMeshRendererList() {
            SetStaticVariable();
			ClearVariable();
            ClearStatus();
			if (VerifyVariable()) {
				RecoveryAvatar.GetSkinnedMeshRenderers();
				StatusString = "UPDATED_RENDERER";
			}
            SetEditorVariable();
			return;
        }

        /* 아바타 복구 메인 프로세스 */
        public void ReplaceSkinnedMeshRendererGameObjects() {
            UpdateSkinnedMeshRendererList();
            RecoveryAvatar.Recovery();
			Debug.Log("[VRSuya AvatarRebuilder] Update Completed");
            DestroyImmediate(this);
            return;
        }

        // 프로그램 상태 초기화
        private void ClearStatus() {
            StatusString = "";
            return;
        }

        // 변수 초기화
        private void ClearVariable() {
			NewAvatarSkinnedMeshRenderersEditor = new SkinnedMeshRenderer[0];
            return;
        }

		// 변수 검사 및 검증
		private bool VerifyVariable() {
			if (!NewAvatarGameObject) {
				StatusString = "NO_AVATAR";
				return false;
			}
			if (NewAvatarGameObject == OldAvatarGameObject) {
				StatusString = "SAME_OBJECT";
				return false;
			}
			if (!NewAvatarGameObject.GetComponent<Animator>()) {
				StatusString = "NO_NEW_ANIMATOR";
				return false;
			}
			if (!AvatarRootBone) {
				if (OldAvatarGameObject.GetComponent<Animator>()) {
					OldAvatarAnimator = OldAvatarGameObject.GetComponent<Animator>();
					if (OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips)) {
						AvatarRootBone = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
					} else {
						StatusString = "NO_ROOTBONE";
						return false;
					}
				} else {
					StatusString = "NO_OLD_ANIMATOR";
					return false;
				}
			}
			return true;
        }
    }
}
#endif