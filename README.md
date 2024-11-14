# AvatarRebuilder

![Component](https://github.com/crestudio/AvatarRebuilder/blob/master/Image/VRSuya_AvatarRebuilder.jpg?raw=true)

## 다운로드

[VCC 패키지 추가](http://macchiato.kr/docs/vrsuya/addon/VPM_Setup)

[UnityPackage 다운로드](https://github.com/crestudio/AvatarRebuilder/releases)

---

## 개요

 Armature가 변경된 SkinnedMeshRenderer 컴포넌트들을 재구성하는 VRSuya 전용 프로그램

- 세팅 되어 있는 아바타를 그대로 다른 FBX 파일 기반으로 이전
- FBX 파일 Import 설정 복사
- 모구모구 볼 본 및 HopeskyD 발가락 본 자동 구성
- 기존 아바타 자동 백업 및 보존

---

## 사용방법

1. 사용 중인 Unity 프로젝트에 해당 패키지를 임포트합니다
1. **적용을 원하는 아바타 GameObject → Add Component → VRSuya AvatarRebuilder 추가**합니다
1. 적용하려는 **패치된 모델 파일을 새 아바타에 넣고, 아바타 타입을 선택**합니다.
1. 아바타 복구 버튼을 누릅니다
   - 만약 잘못 적용한 경우 실행취소(Undo)를 할 수 있습니다

---

## 작동조건

- [x] 적용하려는 아바타가 휴머노이드 타입이어야 합니다
- [x] VRCSDK Avatar 3.0 패키지가 설치 및 올바르게 작동하고 있어야 합니다
- [x] 아바타에 VRC 아바타 디스크립터가 설정되어 있어야 합니다
- [x] 같은 아바타 모델 파일(FBX)에서 본의 추가 케이스에서만 작동합니다 (VRSuya의 모구모구와 발가락만 허용)
- [x] 머리와 몸이 같은 아바타 모델 파일을 사용해야 합니다
- [x] Marshmallow PB과 같은 휴머노이드 본의 구조를 변경한 사항이 없어야 합니다

---

## 알려진 버그

- [ ] Marshmallow PB 패치된 아바타는 패치를 할 수 없는 버그
- [ ] 머리와 몸이 다른 아바타 모델을 사용하는 경우 패치를 할 수 없는 버그

---

## Contact

- Twitter : https://twitter.com/VRSuya
- Mail : vrsuya@gmail.com