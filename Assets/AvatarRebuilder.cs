#if UNITY_EDITOR
using UnityEngine;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from emilianavt/ReassignBoneWeigthsToNewMesh.cs ( https://gist.github.com/emilianavt/721cd4dd2d4a62ba54b002b63f894dbf )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

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
		public string StatusStringEditor = "";

		// 정적 변수
		protected static GameObject NewAvatarGameObject;
		protected static Animator NewAvatarAnimator;
		protected static GameObject OldAvatarGameObject;
		protected static Animator OldAvatarAnimator;
		protected static SkinnedMeshRenderer[] NewAvatarSkinnedMeshRenderers;

		protected static Avatar TargetAvatar;

		public enum Avatar {
			Karin, Milk, Mint, Rusk, SELESTIA, Yoll, NULL
		}

		protected enum BoneNameType {
			General, Komado, Yoll
		}

		protected static Transform AvatarRootBone;
		protected static bool ToggleRestoreArmatureTransform;
		protected static bool ToggleResetRestPose;
		protected static bool ToggleReorderGameObject;

		protected static string StatusString;

		// 컴포넌트 최초 로드시 동작
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

		/// <summary>에디터 변수 -> 정적 변수 동기화합니다.</summary>
		private void SetStaticVariable() {
			NewAvatarGameObject = NewAvatarGameObjectEditor;
			OldAvatarGameObject = OldAvatarGameObjectEditor;
			TargetAvatar = (Avatar)TargetAvatarIndexEditor;
			NewAvatarSkinnedMeshRenderers = NewAvatarSkinnedMeshRenderersEditor;
			AvatarRootBone = AvatarRootBoneEditor;
            ToggleRestoreArmatureTransform = ToggleRestoreArmatureTransformEditor;
			return;
		}

		/// <summary>정적 변수 -> 에디터 변수 동기화합니다.</summary>
		private void SetEditorVariable() {
			NewAvatarGameObjectEditor = NewAvatarGameObject;
			OldAvatarGameObjectEditor = OldAvatarGameObject;
			NewAvatarSkinnedMeshRenderersEditor = NewAvatarSkinnedMeshRenderers;
			AvatarRootBoneEditor = AvatarRootBone;
			StatusStringEditor = StatusString;
			return;
		}

		/// <summary>패치 대상 SkinnedMeshRenderer 목록을 가져옵니다</summary>
		public void UpdateSkinnedMeshRendererList() {
            SetStaticVariable();
			ClearVariable();
			if (VerifyVariable()) {
				RecoveryAvatar.GetSkinnedMeshRenderers();
				StatusString = "UPDATED_RENDERER";
			}
            SetEditorVariable();
			return;
        }

		/// <summary>
		/// 본 프로그램의 메인 세팅 로직입니다.
		/// </summary>
		public void ReplaceSkinnedMeshRendererGameObjects() {
            UpdateSkinnedMeshRendererList();
            RecoveryAvatar.Recovery();

			Debug.Log("[VRSuya AvatarRebuilder] Update Completed");
            DestroyImmediate(this);
            return;
        }

		/// <summary>정적 변수를 초기화 합니다.</summary>
		private void ClearVariable() {
			NewAvatarSkinnedMeshRenderers = new SkinnedMeshRenderer[0];
			StatusString = "";
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
			NewAvatarGameObject.TryGetComponent(typeof(Animator), out Component NewAnimator);
			if (!NewAnimator) {
				StatusString = "NO_NEW_ANIMATOR";
				return false;
			} else {
				NewAvatarAnimator = NewAvatarGameObject.GetComponent<Animator>();
			}
			if (!AvatarRootBone) {
				OldAvatarGameObject.TryGetComponent(typeof(Animator), out Component OldAnimator);
				if (!OldAnimator) {
					StatusString = "NO_OLD_ANIMATOR";
					return false;
				} else {
					OldAvatarAnimator = OldAvatarGameObject.GetComponent<Animator>();
					if (OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips)) {
						AvatarRootBone = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
					} else {
						StatusString = "NO_ROOTBONE";
						return false;
					}
				}
			}
			return true;
        }
    }
}
#endif