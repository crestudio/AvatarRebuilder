#if UNITY_EDITOR
using System.Linq;
using System.Reflection;

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
					ApplyNewAvatarFBXModel();
					PlaceGameObejctInScene();
				}
			}
			return;
		}

		/// <summary>기존 아바타를 복제하여 백업본을 생성합니다.</summary>
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

		/// <summary>새 아바타 GameObject가 Scene에 존재하는지 여부를 알려줍니다.</summary>
		/// <returns>새 아바타 GameObject가 Scene에 존재하는지 여부</returns>
		private static bool IsSameFBX() {
			bool Result = false;
			UnityEngine.Avatar OldAnimatorAvatar = OldAvatarAnimator.avatar;
			UnityEngine.Avatar NewAnimatorAvatar = NewAvatarAnimator.avatar;
			string OldAvatarAssetPath = AssetDatabase.GetAssetPath(OldAnimatorAvatar);
			string NewAvatarAssetPath = AssetDatabase.GetAssetPath(NewAnimatorAvatar);
			if (OldAvatarAssetPath == NewAvatarAssetPath) Result = true;
			return Result;
		}

		/// <summary>새 아바타 GameObject가 Scene에 존재하는지 여부를 알려줍니다.</summary>
		/// <returns>새 아바타 GameObject가 Scene에 존재하는지 여부</returns>
		private static bool IsExistNewAvatarInScene() {
			return NewAvatarGameObject.scene.IsValid();
		}

		/// <summary>기존 아바타의 FBX 메타 데이터를 복제하여, 새 아바타의 FBX 메타 데이터에 적용 합니다.</summary>
		internal static void ApplyNewAvatarFBXModel() {
			UnityEngine.Avatar OldAnimatorAvatar = OldAvatarAnimator.avatar;
			string OldAvatarAssetPath = AssetDatabase.GetAssetPath(OldAnimatorAvatar);
			string NewAvatarAssetPath = AssetDatabase.GetAssetPath(NewAvatarGameObject);
			ModelImporter OldAvatarModelImporter = AssetImporter.GetAtPath(OldAvatarAssetPath) as ModelImporter;
			ModelImporter NewAvatarModelImporter = AssetImporter.GetAtPath(NewAvatarAssetPath) as ModelImporter;
			NewAvatarModelImporter.addCollider = OldAvatarModelImporter.addCollider;
			NewAvatarModelImporter.animationCompression = OldAvatarModelImporter.animationCompression;
			NewAvatarModelImporter.animationPositionError = OldAvatarModelImporter.animationPositionError;
			NewAvatarModelImporter.animationRotationError = OldAvatarModelImporter.animationRotationError;
			NewAvatarModelImporter.animationScaleError = OldAvatarModelImporter.animationScaleError;
			NewAvatarModelImporter.animationType = OldAvatarModelImporter.animationType;
			NewAvatarModelImporter.animationWrapMode = OldAvatarModelImporter.animationWrapMode;
			NewAvatarModelImporter.autoGenerateAvatarMappingIfUnspecified = OldAvatarModelImporter.autoGenerateAvatarMappingIfUnspecified;
			NewAvatarModelImporter.avatarSetup = OldAvatarModelImporter.avatarSetup;
			NewAvatarModelImporter.bakeIK = OldAvatarModelImporter.bakeIK;
			NewAvatarModelImporter.clipAnimations = OldAvatarModelImporter.clipAnimations;
			NewAvatarModelImporter.extraExposedTransformPaths = OldAvatarModelImporter.extraExposedTransformPaths;
			NewAvatarModelImporter.extraUserProperties = OldAvatarModelImporter.extraUserProperties;
			NewAvatarModelImporter.generateAnimations = OldAvatarModelImporter.generateAnimations;
			NewAvatarModelImporter.generateSecondaryUV = OldAvatarModelImporter.generateSecondaryUV;
			NewAvatarModelImporter.globalScale = OldAvatarModelImporter.globalScale;
			NewAvatarModelImporter.humanDescription = OldAvatarModelImporter.humanDescription;
			NewAvatarModelImporter.humanoidOversampling = OldAvatarModelImporter.humanoidOversampling;
			NewAvatarModelImporter.importAnimatedCustomProperties = OldAvatarModelImporter.importAnimatedCustomProperties;
			NewAvatarModelImporter.importAnimation = OldAvatarModelImporter.importAnimation;
			NewAvatarModelImporter.importBlendShapeNormals = OldAvatarModelImporter.importBlendShapeNormals;
			NewAvatarModelImporter.importBlendShapes = OldAvatarModelImporter.importBlendShapes;
			NewAvatarModelImporter.importCameras = OldAvatarModelImporter.importCameras;
			NewAvatarModelImporter.importConstraints = OldAvatarModelImporter.importConstraints;
			NewAvatarModelImporter.importLights = OldAvatarModelImporter.importLights;
			NewAvatarModelImporter.importNormals = OldAvatarModelImporter.importNormals;
			NewAvatarModelImporter.importTangents = OldAvatarModelImporter.importTangents;
			NewAvatarModelImporter.importVisibility = OldAvatarModelImporter.importVisibility;
			NewAvatarModelImporter.indexFormat = OldAvatarModelImporter.indexFormat;
			NewAvatarModelImporter.isReadable = OldAvatarModelImporter.isReadable;
			NewAvatarModelImporter.keepQuads = OldAvatarModelImporter.keepQuads;
			NewAvatarModelImporter.materialImportMode = OldAvatarModelImporter.materialImportMode;
			NewAvatarModelImporter.materialLocation = OldAvatarModelImporter.materialLocation;
			NewAvatarModelImporter.materialName = OldAvatarModelImporter.materialName;
			NewAvatarModelImporter.materialSearch = OldAvatarModelImporter.materialSearch;
			NewAvatarModelImporter.maxBonesPerVertex = OldAvatarModelImporter.maxBonesPerVertex;
			NewAvatarModelImporter.meshCompression = OldAvatarModelImporter.meshCompression;
			NewAvatarModelImporter.meshOptimizationFlags = OldAvatarModelImporter.meshOptimizationFlags;
			NewAvatarModelImporter.minBoneWeight = OldAvatarModelImporter.minBoneWeight;
			NewAvatarModelImporter.motionNodeName = OldAvatarModelImporter.motionNodeName;
			NewAvatarModelImporter.normalCalculationMode = OldAvatarModelImporter.normalCalculationMode;
			NewAvatarModelImporter.normalSmoothingAngle = OldAvatarModelImporter.normalSmoothingAngle;
			NewAvatarModelImporter.normalSmoothingSource = OldAvatarModelImporter.normalSmoothingSource;
			NewAvatarModelImporter.optimizeGameObjects = OldAvatarModelImporter.optimizeGameObjects;
			NewAvatarModelImporter.optimizeMeshPolygons = OldAvatarModelImporter.optimizeMeshPolygons;
			NewAvatarModelImporter.optimizeMeshVertices = OldAvatarModelImporter.optimizeMeshVertices;
			NewAvatarModelImporter.preserveHierarchy = OldAvatarModelImporter.preserveHierarchy;
			NewAvatarModelImporter.resampleCurves = OldAvatarModelImporter.resampleCurves;
			NewAvatarModelImporter.secondaryUVAngleDistortion = OldAvatarModelImporter.secondaryUVAngleDistortion;
			NewAvatarModelImporter.secondaryUVAreaDistortion = OldAvatarModelImporter.secondaryUVAreaDistortion;
			NewAvatarModelImporter.secondaryUVHardAngle = OldAvatarModelImporter.secondaryUVHardAngle;
			NewAvatarModelImporter.secondaryUVPackMargin = OldAvatarModelImporter.secondaryUVPackMargin;
			NewAvatarModelImporter.skinWeights = OldAvatarModelImporter.skinWeights;
			NewAvatarModelImporter.sortHierarchyByName = OldAvatarModelImporter.sortHierarchyByName;
			NewAvatarModelImporter.sourceAvatar = OldAvatarModelImporter.sourceAvatar;
			NewAvatarModelImporter.swapUVChannels = OldAvatarModelImporter.swapUVChannels;
			NewAvatarModelImporter.useFileScale = OldAvatarModelImporter.useFileScale;
			NewAvatarModelImporter.useFileUnits = OldAvatarModelImporter.useFileUnits;
			NewAvatarModelImporter.useSRGBMaterialColor = OldAvatarModelImporter.useSRGBMaterialColor;
			NewAvatarModelImporter.weldVertices = OldAvatarModelImporter.weldVertices;
			CheckLegacyBlendShapeNormals(OldAvatarModelImporter, NewAvatarModelImporter);
			Material[] OldAvatarMaterials = CheckOldAvatarMaterials(OldAvatarModelImporter);
			EditorUtility.SetDirty(NewAvatarModelImporter);
			NewAvatarModelImporter.SaveAndReimport();
		}

		/// <summary>Legacy Blend Shape Normals 속성을 강제 복제합니다.</summary>
		private static void CheckLegacyBlendShapeNormals(ModelImporter OldModelImporter, ModelImporter NewModelImporter) {
			string PropertyName = "legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes";
			PropertyInfo OldLegacyProperty = OldModelImporter.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			PropertyInfo NewLegacyProperty = NewModelImporter.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			OldModelImporter.isReadable = true;
			NewModelImporter.isReadable = true;
			NewLegacyProperty.SetValue(NewModelImporter, (bool)OldLegacyProperty.GetValue(OldModelImporter));
		}

		/// <summary>기존 아바타에서 지정된 Material이 있을 경우에 Array로 반환합니다.</summary>
		/// <returns>기존 아바타에서 지정된 Material 배열</returns>
		private static Material[] CheckOldAvatarMaterials(ModelImporter OldModelImporter) {
			Object[] OldAvatarObjects = OldModelImporter.GetExternalObjectMap().Values.ToArray();
			Material[] OldAvatarMaterials = new Material[0];
			foreach (Object OldObject in OldAvatarObjects) {
				Material TargetMaterial = (Material)OldObject;
				if (TargetMaterial) OldAvatarMaterials = OldAvatarMaterials.Concat(new Material[] { TargetMaterial }).ToArray();
			}
			return OldAvatarMaterials;
		}

		/// <summary>새 아바타 GameObject를 Scene에 배치를 합니다.</summary>
		private static void PlaceGameObejctInScene() {
			GameObject NewInstance = Instantiate(NewAvatarGameObject);
			Undo.RegisterCreatedObjectUndo(NewInstance, "Added New GameObject");
			Undo.CollapseUndoOperations(UndoGroupIndex);
			NewAvatarGameObject = NewInstance;
			return;
		}
	}
}
#endif