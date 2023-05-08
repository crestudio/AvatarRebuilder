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

		// �����Ϳ� ����
		public GameObject NewAvatarGameObjectEditor = null;
		public GameObject OldAvatarGameObjectEditor = null;
		public int TargetAvatarIndexEditor = 0;
		public SkinnedMeshRenderer[] NewAvatarSkinnedMeshRenderersEditor = new SkinnedMeshRenderer[0];
		public Transform AvatarRootBoneEditor = null;
		public bool ToggleRestoreArmatureTransformEditor = true;
		public bool ToggleResetRestPoseEditor = false;
		public bool ToggleReorderGameObjectEditor = true;

		// ���� ����
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

		// ������ ���� -> ���� ���� ����ȭ
		private void SetStaticVariable() {
			NewAvatarGameObject = NewAvatarGameObjectEditor;
			OldAvatarGameObject = OldAvatarGameObjectEditor;
			TargetAvatar = (AvatarType)TargetAvatarIndexEditor;
			NewAvatarSkinnedMeshRenderers = NewAvatarSkinnedMeshRenderersEditor;
			AvatarRootBone = AvatarRootBoneEditor;
            ToggleRestoreArmatureTransform = ToggleRestoreArmatureTransformEditor;
			return;
		}

		// ���� ���� -> ������ ���� ����ȭ
		private void SetEditorVariable() {
			NewAvatarGameObjectEditor = NewAvatarGameObject;
			OldAvatarGameObjectEditor = OldAvatarGameObject;
			NewAvatarSkinnedMeshRenderersEditor = NewAvatarSkinnedMeshRenderers;
			AvatarRootBoneEditor = AvatarRootBone;
			return;
		}

		// ��ġ ��� SkinnedMeshRenderer ��� ���
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

        /* �ƹ�Ÿ ���� ���� ���μ��� */
        public void ReplaceSkinnedMeshRendererGameObjects() {
            UpdateSkinnedMeshRendererList();
            RecoveryAvatar.Recovery();
			Debug.Log("[VRSuya AvatarRebuilder] Update Completed");
            DestroyImmediate(this);
            return;
        }

        // ���α׷� ���� �ʱ�ȭ
        private void ClearStatus() {
            StatusString = "";
            return;
        }

        // ���� �ʱ�ȭ
        private void ClearVariable() {
			NewAvatarSkinnedMeshRenderersEditor = new SkinnedMeshRenderer[0];
            return;
        }

		// ���� �˻� �� ����
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