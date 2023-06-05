using System.Collections.Generic;

/*
 * VRSuya Reassign Bone In SkinnedMeshRenderer Editor for Mogumogu Project
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	public class LanguageHelper : AvatarRebuilderEditor {

		// 요청한 컨텍스트 문장을 언어 속성에 맞춰서 반환
		internal static string GetContextString(string RequestContext) {
			string ReturnContext = "";
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

		// 요청한 아바타를 언어 속성에 맞춰서 반환
		internal static string[] GetAvatarString() {
			switch (LanguageIndex) {
				case 0:
					AvatarType = String_Avatar_English;
					break;
				case 1:
					AvatarType = String_Avatar_Korean;
					break;
				case 2:
					AvatarType = String_Avatar_Japanese;
					break;
			}
			return AvatarType;
		}

		// 영어 사전 데이터
		private static readonly Dictionary<string, string> String_English = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "Language" },
			{ "String_Debug", "Debug" },
			{ "String_OriginalAvatar", "Original Avatar" },
			{ "String_NewAvatar", "New Avatar" },
			{ "String_AvatarType", "Avatar Type" },
			{ "String_RootBone", "Avatar Root Bone" },
			{ "String_RestoreTransform", "Restore Transforms" },
			{ "String_RestPose", "Reset to Rest Pose" },
			{ "String_ReorderGameObject", "Reorder Armature" },
			{ "String_SkinnedMeshRendererList", "Replacement SkinnedMeshRenderer List" },
			{ "String_ImportSkinnedMeshRenderer", "Import SkinnedMeshRenderer List" },
			{ "String_ReplaceAvatar", "Replace SkinnedMeshRenderer and Avatar" },

			// 상태 메시지
			{ "String_Warning", "Other components than SkinnedMeshRenderer will not be copied" },

			// 에러 코드
			{ "UPDATED_RENDERER", "Completed import SkinnedMeshRenderer List" },
			{ "NO_AVATAR", "No Avatar is selected" },
			{ "SAME_OBJECT", "Same as the original avatar! Select a new GameObject of the same avatar" },
			{ "NO_NEW_ANIMATOR", "Not found Animator Component in the New Avatar" },
			{ "NO_ROOTBONE", "Not found Hips bone in the New Avatar" },
			{ "NO_OLD_ANIMATOR", "Not found Animator Component in the Original Avatar" }
		};

		private static readonly string[] String_Avatar_English = new string[] {
			"Generic", "Komado's Avatars", "SELESTIA", "YOLL"
		};

		// 한국어 사전 데이터
		private static readonly Dictionary<string, string> String_Korean = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "언어" },
			{ "String_Debug", "디버그" },
			{ "String_OriginalAvatar", "원본 아바타" },
			{ "String_NewAvatar", "신규 아바타" },
			{ "String_AvatarType", "아바타 타입" },
			{ "String_RootBone", "아바타 루트 본" },
			{ "String_RestoreTransform", "아바타 트랜스폼 복원" },
			{ "String_RestPose", "기본 포즈로 복원" },
			{ "String_ReorderGameObject", "아마추어 순서 복원" },
			{ "String_SkinnedMeshRendererList", "복원될 스킨드 메쉬 렌더러 목록" },
			{ "String_ImportSkinnedMeshRenderer", "스킨드 메쉬 렌더러 목록 가져오기" },
			{ "String_ReplaceAvatar", "스킨드 메쉬 렌더러 및 아바타 복원" },

			// 상태 메시지
			{ "String_Warning", "스킨드 메쉬 렌더러 외의 속성은 가져오지 않습니다!" },

			// 에러 코드
			{ "UPDATED_RENDERER", "복원될 스킨드 메쉬 렌더러 목록을 가져왔습니다" },
			{ "NO_AVATAR", "아바타가 지정되지 않았습니다" },
			{ "SAME_OBJECT", "원본과 같은 아바타입니다, 복구하려는 아바타와 같은 종류의 아바타를 만들어 넣어주세요" },
			{ "NO_NEW_ANIMATOR", "새 아바타에서 애니메이터를 찾을 수 없습니다" },
			{ "NO_ROOTBONE", "아바타에서 루트 본을 찾을 수 없습니다" },
			{ "NO_OLD_ANIMATOR", "원본 아바타에서 애니메이터를 찾을 수 없습니다" }
		};

		private static readonly string[] String_Avatar_Korean = new string[] {
			"일반", "코마도 아바타 종류", "셀레스티아", "요루"
		};

		// 일본어 사전 데이터
		private static readonly Dictionary<string, string> String_Japanese = new Dictionary<string, string>() {
			// UI 데이터
			{ "String_Language", "言語" },
			{ "String_Debug", "デバッグ" },
			{ "String_OriginalAvatar", "原本アバター" },
			{ "String_NewAvatar", "新規アバター" },
			{ "String_AvatarType", "アバタータイプ" },
			{ "String_RootBone", "アバタールートボーン" },
			{ "String_RestoreTransform", "アバターTransform復元" },
			{ "String_RestPose", "基本ポーズに復元" },
			{ "String_ReorderGameObject", "アーマチュア順序復元" },
			{ "String_SkinnedMeshRendererList", "復元されるSkinnedMeshRenderer一覧" },
			{ "String_ImportSkinnedMeshRenderer", "SkinnedMeshRendererリストを取得" },
			{ "String_ReplaceAvatar", "SkinnedMeshRenderer及びアバター復元" },

			// 상태 메시지
			{ "String_Warning", "SkinnedMeshRenderer以外のプロパティは取得しません！" },

			// 에러 코드
			{ "UPDATED_RENDERER", "復元されるSkinnedMeshRendererのリストを取得しました。" },
			{ "NO_AVATAR", "アバターが指定されていません" },
			{ "SAME_OBJECT", "原本と同じアバターです、復旧したいアバターと同じ種類のアバターを作って入れてください" },
			{ "NO_NEW_ANIMATOR", "新しいアバターにアニメーターが見つかりません" },
			{ "NO_ROOTBONE", "アバターにルートボーンが見つかりません" },
			{ "NO_OLD_ANIMATOR", "元のアバターにアニメーターが見つかりません" }
		};

		private static readonly string[] String_Avatar_Japanese = new string[] {
			"一般", "こまどアバターの種類", "セレスティア", "ヨル"
		};
	}
}
