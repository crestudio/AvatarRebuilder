﻿#if UNITY_EDITOR
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

		// �����Ϳ� ����
		public GameObject NewAvatarGameObjectEditor = null;
		public GameObject OldAvatarGameObjectEditor = null;
		public int TargetAvatarIndexEditor = 0;
		public SkinnedMeshRenderer[] NewAvatarSkinnedMeshRenderersEditor = new SkinnedMeshRenderer[0];
		public Transform AvatarRootBoneEditor = null;
		public bool ToggleRestoreArmatureTransformEditor = true;
		public bool ToggleResetRestPoseEditor = false;
		public bool ToggleReorderGameObjectEditor = true;
		public string StatusStringEditor = "";

		// ���� ����
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
			TargetAvatar = (Avatar)TargetAvatarIndexEditor;
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
			StatusStringEditor = StatusString;
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
			StatusStringEditor = "";
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