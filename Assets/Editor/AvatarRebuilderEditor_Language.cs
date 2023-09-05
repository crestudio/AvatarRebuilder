using System.Collections.Generic;
using System.Linq;

using UnityEditor;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	public class LanguageHelper : AvatarRebuilderEditor {

		/// <summary>요청한 값을 설정된 언어에 맞춰 값을 반환합니다.</summary>
		/// <returns>요청한 String의 현재 설정된 언어 버전</returns>
		internal static string GetContextString(string RequestContext) {
			string ReturnContext = RequestContext;
			switch (LanguageIndex) {
				case 0:
					if (String_English.ContainsKey(RequestContext)) {
						ReturnContext = String_English[RequestContext];
					}
					break;
				case 1:
					if (String_Korean.ContainsKey(RequestContext)) {
						ReturnContext = String_Korean[RequestContext];
					}
					break;
				case 2:
					if (String_Japanese.ContainsKey(RequestContext)) {
						ReturnContext = String_Japanese[RequestContext];
					}
					break;
			}
			return ReturnContext;
		}

		/// <summary>요청한 아바타 이름을 설정된 언어에 맞춰 리스트를 재작성합니다.</summary>
		/// <returns>아바타 이름의 현재 설정된 언어 버전</returns>
		internal static string[] ReturnAvatarName(SerializedProperty AvatarNameListProperty) {
			string[] ReturnAvatarList = new string[0];
			string[] AvatarNameList = AvatarNameListProperty.enumNames;
			AvatarRebuilder.Avatar[] InstalledVRSuyaProductAvatars = new AvatarRebuilder.Avatar[AvatarNameList.Length];
			for (int Index = 0; Index < AvatarNameList.Length; Index++) {
				InstalledVRSuyaProductAvatars[Index] = (AvatarRebuilder.Avatar)System.Enum.Parse(typeof(AvatarRebuilder.Avatar), AvatarNameList[Index]);
			}
			foreach (var AvatarName in InstalledVRSuyaProductAvatars) {
				if (dictAvatarNames.ContainsKey(AvatarName)) {
					ReturnAvatarList = ReturnAvatarList.Concat(new string[] { dictAvatarNames[AvatarName][LanguageIndex] }).ToArray();
				}
			}
			return ReturnAvatarList;
		}

		// 영어 사전 데이터
		private static readonly Dictionary<string, string> String_English = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "Language" },
			{ "String_Debug", "Debug" },
			{ "String_OriginalAvatar", "Original Avatar" },
			{ "String_NewAvatar", "New Avatar" },
			{ "String_AvatarType", "Avatar Type" },
			{ "String_Advanced", "Advanced" },
			{ "String_RootBone", "Avatar Root Bone" },
			{ "String_RestoreTransform", "Restore Transforms" },
			{ "String_RestPose", "Reset to Rest Pose" },
			{ "String_ReorderGameObject", "Reorder Armature" },
			{ "String_SkinnedMeshRendererList", "Replacement SkinnedMeshRenderer List" },
			{ "String_ImportSkinnedMeshRenderer", "Import SkinnedMeshRenderer List" },
			{ "String_ReplaceAvatar", "Replace Avatar" },

			// 상태 메시지
			{ "String_General", "Please select [General] for avatars that do not exist in the list" },
			{ "String_Warning", "Other components than SkinnedMeshRenderer will not be copied" },

			// 에러 코드
			{ "UPDATED_RENDERER", "Completed import SkinnedMeshRenderer List" },
			{ "NO_AVATAR", "No Avatar is selected" },
			{ "SAME_OBJECT", "Same as the original avatar! Select a new GameObject of the same avatar" },
			{ "NO_NEW_ANIMATOR", "Not found Animator Component in the New Avatar" },
			{ "NO_ROOTBONE", "Not found Hips bone in the New Avatar" },
			{ "NO_OLD_ANIMATOR", "Not found Animator Component in the Original Avatar" }
		};

		// 한국어 사전 데이터
		private static readonly Dictionary<string, string> String_Korean = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "언어" },
			{ "String_Debug", "디버그" },
			{ "String_OriginalAvatar", "원본 아바타" },
			{ "String_NewAvatar", "신규 아바타" },
			{ "String_AvatarType", "아바타 타입" },
			{ "String_Advanced", "고급" },
			{ "String_RootBone", "아바타 루트 본" },
			{ "String_RestoreTransform", "아바타 트랜스폼 복원" },
			{ "String_RestPose", "기본 포즈로 복원" },
			{ "String_ReorderGameObject", "아마추어 순서 복원" },
			{ "String_SkinnedMeshRendererList", "복원될 스킨드 메쉬 렌더러 목록" },
			{ "String_ImportSkinnedMeshRenderer", "스킨드 메쉬 렌더러 목록 가져오기" },
			{ "String_ReplaceAvatar", "아바타 교체" },

			// 상태 메시지
			{ "String_General", "목록에 존재하지 않는 아바타는 [일반]을 선택해 주세요" },
			{ "String_Warning", "스킨드 메쉬 렌더러 외의 속성은 가져오지 않습니다!" },

			// 에러 코드
			{ "UPDATED_RENDERER", "복원될 스킨드 메쉬 렌더러 목록을 가져왔습니다" },
			{ "NO_AVATAR", "아바타가 지정되지 않았습니다" },
			{ "SAME_OBJECT", "원본과 같은 아바타입니다, 복구하려는 아바타와 같은 종류의 아바타를 만들어 넣어주세요" },
			{ "NO_NEW_ANIMATOR", "새 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_ROOTBONE", "아바타에서 루트 본을 찾을 수 없습니다" },
			{ "NO_OLD_ANIMATOR", "원본 아바타에서 애니메이터를 찾을 수 없습니다" }
		};

		// 일본어 사전 데이터
		private static readonly Dictionary<string, string> String_Japanese = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "言語" },
			{ "String_Debug", "デバッグ" },
			{ "String_OriginalAvatar", "原本アバター" },
			{ "String_NewAvatar", "新規アバター" },
			{ "String_AvatarType", "アバタータイプ" },
			{ "String_Advanced", "詳細" },
			{ "String_RootBone", "アバタールートボーン" },
			{ "String_RestoreTransform", "アバターTransform復元" },
			{ "String_RestPose", "基本ポーズに復元" },
			{ "String_ReorderGameObject", "アーマチュア順序復元" },
			{ "String_SkinnedMeshRendererList", "復元されるSkinnedMeshRenderer一覧" },
			{ "String_ImportSkinnedMeshRenderer", "SkinnedMeshRendererリストを取得" },
			{ "String_ReplaceAvatar", "アバター交換" },

			// 상태 메시지
			{ "String_General", "リストに存在しないアバターは「一般」を選択してください" },
			{ "String_Warning", "SkinnedMeshRenderer以外のプロパティは取得しません！" },

			// 에러 코드
			{ "UPDATED_RENDERER", "復元されるSkinnedMeshRendererのリストを取得しました。" },
			{ "NO_AVATAR", "アバターが指定されていません" },
			{ "SAME_OBJECT", "原本と同じアバターです、復旧したいアバターと同じ種類のアバターを作って入れてください" },
			{ "NO_NEW_ANIMATOR", "新しいアバターにアニメーターが見つかりません" },
			{ "NO_ROOTBONE", "アバターにルートボーンが見つかりません" },
			{ "NO_OLD_ANIMATOR", "元のアバターにアニメーターが見つかりません" }
		};

		/// <summary>요청한 아바타 이름들을 설정된 언어에 맞춰 변환합니다.</summary>
		/// <returns>요청한 아바타 이름들의 현재 설정된 언어 버전</returns>
		private static readonly Dictionary<AvatarRebuilder.Avatar, string[]> dictAvatarNames = new Dictionary<AvatarRebuilder.Avatar, string[]>() {
			{ AvatarRebuilder.Avatar.NULL, new string[] { "General", "일반", "一般" } },
			{ AvatarRebuilder.Avatar.Karin, new string[] { "Karin", "카린", "カリン" } },
			{ AvatarRebuilder.Avatar.MANUKA, new string[] { "MANUKA", "마누카", "マヌカ" } },
			{ AvatarRebuilder.Avatar.Milk, new string[] { "Milk(New)", "밀크(신)", "ミルク（新）" } },
			{ AvatarRebuilder.Avatar.Mint, new string[] { "Mint", "민트", "ミント" } },
			{ AvatarRebuilder.Avatar.Rusk, new string[] { "Rusk", "러스크", "ラスク" } },
			{ AvatarRebuilder.Avatar.SELESTIA, new string[] { "SELESTIA", "셀레스티아", "セレスティア" } },
			{ AvatarRebuilder.Avatar.Yoll, new string[] { "Yoll", "요루", "ヨル" } }
		};
	}
}
