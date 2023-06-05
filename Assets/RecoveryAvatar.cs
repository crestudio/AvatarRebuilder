#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using VRC.SDK3.Avatars.Components;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from emilianavt/ReassignBoneWeigthsToNewMesh.cs ( https://gist.github.com/emilianavt/721cd4dd2d4a62ba54b002b63f894dbf )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class RecoveryAvatar : AvatarRebuilder {

		// 정적 변수
		private static GameObject[] NewAvatarGameObjects;
		private static GameObject[] OldAvatarGameObjects;

		private static Transform[] NewArmatureTransforms;
		private static Transform[] OldArmatureTransforms;

		private static VRCAvatarDescriptor OldVRCAvatarDescriptor;
		private static SkinnedMeshRenderer NewAvatarHeadVisemeSkinnedMeshRenderer;
		private static SkinnedMeshRenderer NewAvatarHeadEyelidsSkinnedMeshRenderer;

		private static GameObject[] NewCheekBoneGameObjects;
		private static GameObject[] OldCheekBoneGameObjects;

		private static GameObject[] NewFeetBoneGameObjects;

		private static SkinnedMeshRenderer[] OldAvatarSkinnedMeshRenderers;

		// 사전 데이터
		private static List<HumanBodyBones> HumanBodyBoneList = GetHumanBoneList();
		private static readonly string[] ArmatureNames = { "Armature", "armature", "Sonia", "Ash" };
		private static readonly Dictionary<BoneNameType, string[]> dictCheekBoneNames = new Dictionary<BoneNameType, string[]>() {
			{ BoneNameType.General, new string[] { "Cheek1_L", "Cheek1_R", "Cheek2_L", "Cheek2_R" } },
			{ BoneNameType.Komado, new string[] { "Cheek_Root_L", "Cheek_Root_R", "Cheek_L", "Cheek_R" } },
			{ BoneNameType.Yoll, new string[] { "Cheek1_L", "Cheek1_R", "ho_L", "ho_R" } }
		};
		private static readonly string[,] dictToeNames = {
			{ "ThumbToe1_L", "ThumbToe2_L", "ThumbToe3_L" },
			{ "ThumbToe1_R", "ThumbToe2_R", "ThumbToe3_R" },
			{ "IndexToe1_L", "IndexToe2_L", "IndexToe3_L" },
			{ "IndexToe1_R", "IndexToe2_R", "IndexToe3_R" },
			{ "MiddleToe1_L", "MiddleToe2_L", "MiddleToe3_L" },
			{ "MiddleToe1_R", "MiddleToe2_R", "MiddleToe3_R" },
			{ "RingToe1_L", "RingToe2_L", "RingToe3_L" },
			{ "RingToe1_R", "RingToe2_R", "RingToe3_R" },
			{ "LittleToe1_L", "LittleToe2_L", "LittleToe3_L" },
			{ "LittleToe1_R", "LittleToe2_R", "LittleToe3_R" }
		};

		/* 아바타 복구 메인 프로세스 */
		/// <summary>신규 아바타와 기존 아바타와 비교하여 패치해야 되는 SkinnedMeshRender 목록을 반환합니다.</summary>
		/// <returns>패치 적용 대상인 SkinnedMeshRenderer 배열</returns>
		internal static SkinnedMeshRenderer[] GetSkinnedMeshRenderers() {
			SkinnedMeshRenderer[] AllNewAvatarSkinnedMeshRenderers = NewAvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			SkinnedMeshRenderer[] AllOldAvatarSkinnedMeshRenderers = OldAvatarGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

			NewAvatarGameObjects = new GameObject[AllNewAvatarSkinnedMeshRenderers.Length];
			OldAvatarGameObjects = new GameObject[AllOldAvatarSkinnedMeshRenderers.Length];

			NewAvatarSkinnedMeshRenderers = new SkinnedMeshRenderer[AllNewAvatarSkinnedMeshRenderers.Length];
			OldAvatarSkinnedMeshRenderers = new SkinnedMeshRenderer[AllOldAvatarSkinnedMeshRenderers.Length];

			int Index = 0;
			foreach (SkinnedMeshRenderer NewSkinnedMeshRenderer in AllNewAvatarSkinnedMeshRenderers) {
				foreach (SkinnedMeshRenderer OldSkinnedMeshRenderer in AllOldAvatarSkinnedMeshRenderers) {
					if (NewSkinnedMeshRenderer.name == OldSkinnedMeshRenderer.name) {
						OldAvatarSkinnedMeshRenderers[Index] = OldSkinnedMeshRenderer;
						NewAvatarSkinnedMeshRenderers[Index] = NewSkinnedMeshRenderer;
						OldAvatarGameObjects[Index] = OldSkinnedMeshRenderer.gameObject;
						NewAvatarGameObjects[Index] = NewSkinnedMeshRenderer.gameObject;
						Index++;
						break;
					}
				}
			}
			Array.Resize(ref NewAvatarSkinnedMeshRenderers, Index);
			Array.Resize(ref OldAvatarSkinnedMeshRenderers, Index);
			Array.Resize(ref NewAvatarGameObjects, Index);
			Array.Resize(ref OldAvatarGameObjects, Index);
			return NewAvatarSkinnedMeshRenderers;
		}

		internal static void Recovery() {
			GetHeadSkinnedMeshRenderers();
			ResizeNewAvatarTransform();
			GetArmatureTransforms();
			GetCheekTransforms();
			GetFeetTransforms();
			if (TargetAvatar == AvatarType.Komado || TargetAvatar == AvatarType.Yoll) GetOldCheekBoneGameObjects();
			RenameGameObjects();
			UnpackPrefab();
			if (ToggleReorderGameObject) ReorderGameObjects();
			if (ToggleRestoreArmatureTransform) {
				RetransformNewAvatarArmatureTransforms();
				GetArmatureTransforms();
			}
			CopyBlendshapeSettings();
			CopyGameObjectSettings();
			ReplaceSkinnedMeshRendererBoneSettings();
			CopyGameObjectActive();
			MoveGameObjects();
			MoveCheekBoneGameObjects();
			MoveFeetBoneGameObjects();
			DeleteGameObjects();
			UpdateVRCAvatarDescriptor();
			return;
		}

		// HumanBodyBones 목록화
		private static List<HumanBodyBones> GetHumanBoneList() {
			return Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>().ToList();
		}

		// 각 아바타의 Armature 목록 얻기
		private static void GetArmatureTransforms() {
			Transform[] NewAvatarTransforms = NewAvatarGameObject.GetComponentsInChildren<Transform>(true);
			Transform[] OldAvatarTransforms = OldAvatarGameObject.GetComponentsInChildren<Transform>(true);
			NewArmatureTransforms = Array.Find(NewAvatarTransforms, NewTransform => Array.Exists(ArmatureNames, ArmatureName => NewTransform.gameObject.name == ArmatureName) == true).GetComponentsInChildren<Transform>(true);
			OldArmatureTransforms = Array.Find(OldAvatarTransforms, OldTransform => Array.Exists(ArmatureNames, ArmatureName => OldTransform.gameObject.name == ArmatureName) == true).GetComponentsInChildren<Transform>(true);
			return;
		}

		// 신규 아바타에서 루트 볼 본 목록 얻기
		private static void GetCheekTransforms() {
			if (TargetAvatar != AvatarType.SELESTIA) {
				string[] CheekBoneNames = dictCheekBoneNames[TargetAvatar].Take(2).ToArray();
				NewCheekBoneGameObjects = Array.FindAll(NewArmatureTransforms, ArmatureTransform => Array.Exists(CheekBoneNames, BoneName => ArmatureTransform.gameObject.name == BoneName) == true).Select(Transform => Transform.gameObject).ToArray();
			}
			return;
		}

		// 신규 아바타에서 루트 발가락 본 목록 얻기
		private static void GetFeetTransforms() {
			string[] FeetRootBoneNames = Enumerable.Range(0, dictToeNames.GetLength(0)).Select(x => dictToeNames[x, 0]).ToArray();
			NewFeetBoneGameObjects = Array.FindAll(NewArmatureTransforms, ArmatureTransform => Array.Exists(FeetRootBoneNames, BoneName => ArmatureTransform.gameObject.name == BoneName) == true).Select(Transform => Transform.gameObject).ToArray();
			return;
		}

		// VRC AvatarDescriptor 작업에 필요한 SkinnedMeshRenderer 구하는 함수 
		private static SkinnedMeshRenderer GetHeadSkinnedMeshRenderers() {
			if (OldAvatarGameObject.GetComponent<VRCAvatarDescriptor>()) {
				OldVRCAvatarDescriptor = OldAvatarGameObject.GetComponent<VRCAvatarDescriptor>();
				SkinnedMeshRenderer OldAvatarHeadVisemeSkinnedMeshRenderer = null;
				SkinnedMeshRenderer OldAvatarHeadEyelidsSkinnedMeshRenderer = null;
				if (!OldVRCAvatarDescriptor.VisemeSkinnedMesh) OldAvatarHeadVisemeSkinnedMeshRenderer = OldVRCAvatarDescriptor.VisemeSkinnedMesh;
				if (!OldVRCAvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh) OldAvatarHeadEyelidsSkinnedMeshRenderer = OldVRCAvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh;
				if (!OldAvatarHeadVisemeSkinnedMeshRenderer) NewAvatarHeadVisemeSkinnedMeshRenderer = Array.Find(NewAvatarSkinnedMeshRenderers, NewSkinnedMeshRenderer => OldAvatarHeadVisemeSkinnedMeshRenderer.gameObject.name == NewSkinnedMeshRenderer.gameObject.name);
				if (!OldAvatarHeadEyelidsSkinnedMeshRenderer) NewAvatarHeadEyelidsSkinnedMeshRenderer = Array.Find(NewAvatarSkinnedMeshRenderers, NewSkinnedMeshRenderer => OldAvatarHeadEyelidsSkinnedMeshRenderer.gameObject.name == NewSkinnedMeshRenderer.gameObject.name);
			}
			return NewAvatarHeadVisemeSkinnedMeshRenderer;
		}

		// EYO 및 IMERIS 헤어 관련 버그 문제 해결
		private static void RenameGameObjects() {
			foreach (Transform TargetTransform in NewArmatureTransforms) {
				switch (TargetTransform.name) {
					case "Eyo_hair 1":
						TargetTransform.name = "Eyo_hair";
						break;
					case "Imeris_hair 1":
						TargetTransform.name = "Imeris_hair";
						break;
				}
			}
			return;
		}

		// GameObject Prefab 풀기
		private static void UnpackPrefab() {
			for (int Try = 0; Try < 5; Try++) {
				if (PrefabUtility.GetPrefabAssetType(NewAvatarGameObject) == PrefabAssetType.NotAPrefab) {
					break;
				} else {
					PrefabUtility.UnpackPrefabInstance(NewAvatarGameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
				}
			}
			for (int Try = 0; Try < 5; Try++) {
				if (PrefabUtility.GetPrefabAssetType(OldAvatarGameObject) == PrefabAssetType.NotAPrefab) {
					break;
				} else {
					if (PrefabUtility.GetPrefabAssetType(OldAvatarGameObject) != PrefabAssetType.NotAPrefab) PrefabUtility.UnpackPrefabInstance(OldAvatarGameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
				}
			}
			return;
		}

		// 신규 아바타의 월드 Transform 및 로컬 Transform 수정
		private static void ResizeNewAvatarTransform() {
			NewAvatarGameObject.transform.SetPositionAndRotation(OldAvatarGameObject.transform.position, OldAvatarGameObject.transform.rotation);
			NewAvatarGameObject.transform.localPosition = OldAvatarGameObject.transform.localPosition;
			NewAvatarGameObject.transform.localRotation = OldAvatarGameObject.transform.localRotation;
			NewAvatarGameObject.transform.localScale = OldAvatarGameObject.transform.localScale;
			return;
		}

		// 기존에 이미 존재하는 아바타 루트 볼 본 목록 얻기
		private static GameObject[] GetOldCheekBoneGameObjects() {
			string[] CheekBoneNames = dictCheekBoneNames[TargetAvatar].Take(2).ToArray();
			OldCheekBoneGameObjects = Array.FindAll(OldArmatureTransforms, OldTransform => Array.Exists(CheekBoneNames, BoneName => OldTransform.gameObject.name == BoneName) == true).Select(Item => Item.gameObject).ToArray();
			return OldCheekBoneGameObjects;
		}

		// 신규 아바타로 기존 Armature의 Transform 값 복사
		private static void RetransformNewAvatarArmatureTransforms() {
			for (int NewIndex = 0; NewIndex < NewArmatureTransforms.Length; NewIndex++) {
				for (int OldIndex = 0; OldIndex < OldArmatureTransforms.Length; OldIndex++) {
					if (NewArmatureTransforms[NewIndex] && OldArmatureTransforms[OldIndex] && NewArmatureTransforms[NewIndex].name == OldArmatureTransforms[OldIndex].name) {
						if (TargetAvatar == AvatarType.Komado || TargetAvatar == AvatarType.Yoll) {
							if (Array.Exists(dictCheekBoneNames[TargetAvatar], BoneName => NewArmatureTransforms[NewIndex].name == BoneName)) continue;
						}
						NewArmatureTransforms[NewIndex].localPosition = OldArmatureTransforms[OldIndex].localPosition;
						NewArmatureTransforms[NewIndex].localRotation = OldArmatureTransforms[OldIndex].localRotation;
						NewArmatureTransforms[NewIndex].localScale = OldArmatureTransforms[OldIndex].localScale;
						break;
					}
				}
			}
			for (int ArmatureIndex = 0; ArmatureIndex < NewArmatureTransforms.Length; ArmatureIndex++) {
				foreach (HumanBodyBones HumanBone in HumanBodyBoneList) {
					if (HumanBone == HumanBodyBones.LastBone) continue;
					if (OldAvatarAnimator.GetBoneTransform(HumanBone) == null) continue;
					if (NewArmatureTransforms[ArmatureIndex].name == OldAvatarAnimator.GetBoneTransform(HumanBone).name) {
						NewArmatureTransforms[ArmatureIndex].localPosition = OldAvatarAnimator.GetBoneTransform(HumanBone).localPosition;
						NewArmatureTransforms[ArmatureIndex].localRotation = OldAvatarAnimator.GetBoneTransform(HumanBone).localRotation;
						NewArmatureTransforms[ArmatureIndex].localScale = OldAvatarAnimator.GetBoneTransform(HumanBone).localScale;
					}
				}
			}
			return;
		}

		// 신규 아바타에 GameObject 및 SkinnedMeshRenderer 데이터 복사
		private static void CopyGameObjectSettings() {
			for (int Index = 0; Index < NewAvatarSkinnedMeshRenderers.Length; Index++) {
				// GameObject
				NewAvatarSkinnedMeshRenderers[Index].gameObject.isStatic = OldAvatarSkinnedMeshRenderers[Index].gameObject.isStatic;
				NewAvatarSkinnedMeshRenderers[Index].gameObject.tag = OldAvatarSkinnedMeshRenderers[Index].gameObject.tag;
				NewAvatarSkinnedMeshRenderers[Index].gameObject.layer = OldAvatarSkinnedMeshRenderers[Index].gameObject.layer;

				// Transform
				NewAvatarSkinnedMeshRenderers[Index].gameObject.transform.localPosition = OldAvatarSkinnedMeshRenderers[Index].gameObject.transform.localPosition;
				NewAvatarSkinnedMeshRenderers[Index].gameObject.transform.localRotation = OldAvatarSkinnedMeshRenderers[Index].gameObject.transform.localRotation;
				NewAvatarSkinnedMeshRenderers[Index].gameObject.transform.localScale = OldAvatarSkinnedMeshRenderers[Index].gameObject.transform.localScale;

				// SkinnedMeshRenderer Settings
				NewAvatarSkinnedMeshRenderers[Index].localBounds = OldAvatarSkinnedMeshRenderers[Index].localBounds;
				NewAvatarSkinnedMeshRenderers[Index].quality = OldAvatarSkinnedMeshRenderers[Index].quality;
				NewAvatarSkinnedMeshRenderers[Index].updateWhenOffscreen = OldAvatarSkinnedMeshRenderers[Index].updateWhenOffscreen;

				Material[] NewSharedMaterials = new Material[OldAvatarSkinnedMeshRenderers[Index].sharedMaterials.Length];
				for (int MaterialIndex = 0; MaterialIndex < NewSharedMaterials.Length; MaterialIndex++) {
					NewSharedMaterials[MaterialIndex] = OldAvatarSkinnedMeshRenderers[Index].sharedMaterials[MaterialIndex];
				}
				NewAvatarSkinnedMeshRenderers[Index].sharedMaterials = NewSharedMaterials;
				for (int MaterialIndex = 0; MaterialIndex < OldAvatarSkinnedMeshRenderers[Index].sharedMaterials.Length; MaterialIndex++) {
					NewAvatarSkinnedMeshRenderers[Index].sharedMaterials[MaterialIndex] = OldAvatarSkinnedMeshRenderers[Index].sharedMaterials[MaterialIndex];
				}

				NewAvatarSkinnedMeshRenderers[Index].shadowCastingMode = OldAvatarSkinnedMeshRenderers[Index].shadowCastingMode;
				NewAvatarSkinnedMeshRenderers[Index].receiveShadows = OldAvatarSkinnedMeshRenderers[Index].receiveShadows;
				NewAvatarSkinnedMeshRenderers[Index].lightProbeUsage = OldAvatarSkinnedMeshRenderers[Index].lightProbeUsage;
				NewAvatarSkinnedMeshRenderers[Index].reflectionProbeUsage = OldAvatarSkinnedMeshRenderers[Index].reflectionProbeUsage;
				NewAvatarSkinnedMeshRenderers[Index].probeAnchor = OldAvatarSkinnedMeshRenderers[Index].probeAnchor;
				NewAvatarSkinnedMeshRenderers[Index].skinnedMotionVectors = OldAvatarSkinnedMeshRenderers[Index].skinnedMotionVectors;
				NewAvatarSkinnedMeshRenderers[Index].allowOcclusionWhenDynamic = OldAvatarSkinnedMeshRenderers[Index].allowOcclusionWhenDynamic;
			}
			return;
		}

		// Blendshape 리스트 목록 작성 및 신규 아바타로 Blendshape 수치 복사
		private static void CopyBlendshapeSettings() {
			for (int Index = 0; Index < NewAvatarSkinnedMeshRenderers.Length; Index++) {
				Mesh NewAvatarMesh = NewAvatarSkinnedMeshRenderers[Index].sharedMesh;
				Mesh OldAvatarMesh = OldAvatarSkinnedMeshRenderers[Index].sharedMesh;
				string[] OldAvatarBlendshapeList = new string[OldAvatarMesh.blendShapeCount];
				string[] NewAvatarBlendshapeList = new string[NewAvatarMesh.blendShapeCount];
				if (OldAvatarMesh.blendShapeCount > 0) {
					for (int Offset = 0; Offset < OldAvatarMesh.blendShapeCount; Offset++) {
						OldAvatarBlendshapeList[Offset] = OldAvatarMesh.GetBlendShapeName(Offset);
					}
				}
				if (NewAvatarMesh.blendShapeCount > 0) {
					for (int Offset = 0; Offset < NewAvatarMesh.blendShapeCount; Offset++) {
						NewAvatarBlendshapeList[Offset] = NewAvatarMesh.GetBlendShapeName(Offset);
					}
				}
				for (int NewIndex = 0; NewIndex < NewAvatarBlendshapeList.Length; NewIndex++) {
					for (int OldIndex = 0; OldIndex < OldAvatarBlendshapeList.Length; OldIndex++) {
						if (NewAvatarBlendshapeList[NewIndex] == OldAvatarBlendshapeList[OldIndex]) {
							NewAvatarSkinnedMeshRenderers[Index].SetBlendShapeWeight(NewIndex, OldAvatarSkinnedMeshRenderers[Index].GetBlendShapeWeight(OldIndex));
							break;
						}
					}
				}
			}
			return;
		}

		// 신규 아바타의 SkinnedMeshRenderer의 본 데이터를 기존 Armature 본에 맞춰 이전 작업
		private static void ReplaceSkinnedMeshRendererBoneSettings() {
			foreach (SkinnedMeshRenderer NewSkinnedMeshRenderer in NewAvatarSkinnedMeshRenderers) {
				Transform[] ChildBones = NewSkinnedMeshRenderer.bones;
				NewSkinnedMeshRenderer.rootBone = AvatarRootBone;
				for (int BoneIndex = 0; BoneIndex < ChildBones.Length; BoneIndex++) {
					for (int TransformIndex = 0; TransformIndex < OldArmatureTransforms.Length; TransformIndex++) {
						if (ChildBones[BoneIndex] && OldArmatureTransforms[TransformIndex] && ChildBones[BoneIndex].name == OldArmatureTransforms[TransformIndex].name) {
							if (TargetAvatar == AvatarType.Komado || TargetAvatar == AvatarType.Yoll) {
								if (Array.Exists(dictCheekBoneNames[TargetAvatar], BoneName => ChildBones[BoneIndex].name == BoneName)) continue;
							}
							if (ToggleResetRestPose == true) {
								OldArmatureTransforms[TransformIndex].transform.localPosition = ChildBones[BoneIndex].transform.localPosition;
								OldArmatureTransforms[TransformIndex].transform.localRotation = ChildBones[BoneIndex].transform.localRotation;
								OldArmatureTransforms[TransformIndex].transform.localScale = ChildBones[BoneIndex].transform.localScale;
							}
							ChildBones[BoneIndex] = OldArmatureTransforms[TransformIndex];
							break;
						}
					}
				}
				for (int BoneIndex = 0; BoneIndex < ChildBones.Length; BoneIndex++) {
					foreach (HumanBodyBones HumanBone in HumanBodyBoneList) {
						if (HumanBone == HumanBodyBones.LastBone) continue;
						if (OldAvatarAnimator.GetBoneTransform(HumanBone) == null) continue;
						if (ChildBones[BoneIndex].name == OldAvatarAnimator.GetBoneTransform(HumanBone).name) {
							ChildBones[BoneIndex] = OldAvatarAnimator.GetBoneTransform(HumanBone);
						}
					}
				}
				NewSkinnedMeshRenderer.bones = ChildBones;
			}
			return;
		}

		// 신규 아바타에 GameObject 활성화 상태 복사
		private static void CopyGameObjectActive() {
			for (int Index = 0; Index < NewAvatarGameObjects.Length; Index++) {
				NewAvatarGameObjects[Index].SetActive(OldAvatarGameObjects[Index].activeSelf);
			}
			return;
		}

		// Armature에 존재하는 GameObject 순서를 최상단으로 올리기
		private static void ReorderGameObjects() {
			string[] NewArmatureTransformNames = NewArmatureTransforms.Select(NewTransform => NewTransform.name).ToArray();
			for (int Index = OldArmatureTransforms.Length - 1; Index >= 0; Index--) {
				if (Array.Exists(NewArmatureTransformNames, Name => Name == OldArmatureTransforms[Index].name) == true) {
					OldArmatureTransforms[Index].transform.SetAsFirstSibling();
				}
			}
			for (int Index = HumanBodyBoneList.Count - 1; Index >= 0; Index--) {
				if (HumanBodyBoneList[Index] == HumanBodyBones.LastBone) continue;
				if (OldAvatarAnimator.GetBoneTransform(HumanBodyBoneList[Index]) == null) continue;
				OldAvatarAnimator.GetBoneTransform(HumanBodyBoneList[Index]).SetAsFirstSibling();
			}
			return;
		}

		// 신규 아바타의 GameObject를 기존 아바타로 이전 및 동일한 순서로 편성 
		private static void MoveGameObjects() {
			for (int Index = 0; Index < NewAvatarGameObjects.Length; Index++) {
				NewAvatarGameObjects[Index].transform.SetParent(OldAvatarGameObjects[Index].transform.parent, false);
				NewAvatarGameObjects[Index].transform.SetSiblingIndex(OldAvatarGameObjects[Index].transform.GetSiblingIndex() + 1);
			}
			return;
		}

		// 신규 아바타의 볼 본 GameObject를 기존 아바타로 이동
		private static void MoveCheekBoneGameObjects() {
			if (NewCheekBoneGameObjects.Length > 0) {
				foreach (GameObject CheekBoneGameObject in NewCheekBoneGameObjects) {
					CheekBoneGameObject.transform.SetParent(OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Head), false);
				}
			}
			return;
		}

		// 신규 아바타의 발가락 본 GameObject를 기존 아바타로 이동
		private static void MoveFeetBoneGameObjects() {
			if (NewFeetBoneGameObjects.Length > 0) {
				Transform LeftFoot = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
				Transform RightFoot = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
				Transform LeftToe = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.LeftToes);
				Transform RightToe = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.RightToes);
				Transform TargetLeft = null;
				Transform TargetRight = null;
				if (!LeftToe) { 
					TargetLeft = LeftFoot;
				} else {
					TargetLeft = LeftToe;
				}
				if (!RightToe) {
					TargetRight = RightFoot;
				} else {
					TargetRight = RightToe;
				}
				foreach (GameObject CheekBoneGameObject in NewFeetBoneGameObjects) {
					CheekBoneGameObject.transform.SetParent(OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Head), false);
				}
			}
			return;
		}

		// 작업 완료 후 불필요한 GameObject 삭제
		private static void DeleteGameObjects() {
			if (TargetAvatar == AvatarType.Komado || TargetAvatar == AvatarType.Yoll) {
				if (OldCheekBoneGameObjects.Length > 0) {
					for (int Index = 0; Index < OldCheekBoneGameObjects.Length; Index++) {
						DestroyImmediate(OldCheekBoneGameObjects[Index]);
					}
				}
			}
			if (OldAvatarGameObjects.Length > 0) {
				for (int Index = 0; Index < OldAvatarGameObjects.Length; Index++) {
					DestroyImmediate(OldAvatarGameObjects[Index]);
				}
			}
			DestroyImmediate(NewAvatarGameObject);
			return;
		}

		// VRCAvatarDescriptor 컴포넌트에 새로운 아바타 메쉬 업데이트
		private static void UpdateVRCAvatarDescriptor() {
			if (OldVRCAvatarDescriptor) {
				if (NewAvatarHeadVisemeSkinnedMeshRenderer) {
					OldVRCAvatarDescriptor.VisemeSkinnedMesh = NewAvatarHeadVisemeSkinnedMeshRenderer;
				}
				if (NewAvatarHeadEyelidsSkinnedMeshRenderer) {
					OldVRCAvatarDescriptor.customEyeLookSettings.eyelidsSkinnedMesh = NewAvatarHeadEyelidsSkinnedMeshRenderer;
				}
			}
			return;
		}
	}
}
#endif