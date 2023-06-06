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

		private static AvatarStatus CheckNewAvatarStatus() {
			AvatarStatus Result = AvatarStatus.NeedPatch;
			return Result;
		}
	}
}
#endif