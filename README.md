# Popup UI Implementation Using Unity

기본적으로 요구된 내용은
1. 사용자가 상품을 획득한 상황을 가정한 결과를 팝업 UI로 제작
2. 팝업이 나타날 때와 사라질 때 애니메이션 필요
3. 팝업에는 상품 이미지, 상품명, 등급, 가격 정보 표기
4. 등급이 높은 상품 효과 추가

이에 따라 구현했습니다.

https://youtu.be/XZBhl8qWEg0
실제 작동 영상은 위 링크

프로그램을 시작하면 Scripts/Manager/SingletonManager가 가장 먼저 실행되며, Init 함수 안의 GetApi 함수를 통해 Restful API를 이용 데이터를 받아옵니다.
받아온 데이터는 Fake Store Api라는 무료 restful api 제공 사이트에서 받아오고, 받아온 JSON 데이터를 유니티 자체 JsonUtility를 통해 파싱한 다음 저장합니다.
id를 저장해두는 list와 해당 id를 이용해 item data를 찾을 수 있는 dictionary로 되어있습니다. id가 순서대로 있으면 그냥 List<ItemData>로 해도 될 것 같은데 그렇지 않아서 일부러 이렇게 만들었습니다.
데이터의 image는 실제 texture image가 아닌 링크가 들어있어서 데이터 파싱 후에 한번 더 texture를 받아오는 작업을 LoadTexture 코루틴을 통해 진행합니다.

획득한 상황과 관련하여 이건 게임에서 획득한 것과 같이 구현했습니다. Scripts/ShopSphere 코드와 Scripts/SphereButton 코드에 해당 내용이 있습니다.
공에 근접했을 때 Press Space 팝업을 띄우는 것과 space 눌렀을 때 팝업을 띄울 수 있도록 하는 부분이 SphereButton 코드에 들어있습니다.
공에 닿았을 때 띄우는 건 ShopSphere 코드에 들어있습니다. 둘 다 Trigger Enter 시에 발동하게 만들었으며, SphereButton 코드에서 ShopSphere의 OpenUI를 콜백하게 해서 작동합니다.

popup이 뜰 때는 animation을 재생하는 방식으로 했습니다. 단순히 나타났다 사라지는 거라서 timeline같은 것보단 단순하게 animation을 이용하는게 나을 듯 해서 그렇게 구현했습니다.
ui open/close 시 PopupCoroution 코루틴을 진행하며, open인이 close인지에 따라 animator의 파라미터 speed 값을 다르게 주는 것으로 애니메이션 하나를 재활용 하면서 등장과 퇴장을 구현했습니다.
등장 시에 팝업이 다 뜨기 전에 띄울 데이터를 랜덤하게 SingletonManager의 GetRandomItem을 통해 받아오게 되며, 그 데이터 값을 전부 대응시켜 바꿔줍니다.
애니메이션 재생이 끝났는지 normalizedTime 값 체크 후에 코루틴을 종료시킵니다. 

item data의 rating 안의 값에 따라 rating 부분과 price 부분의 글자 색을 변경했습니다. rating의 count가 200 이하면 색을 파랑, 200 이상이고 점수가 4.5점 초과면 노랑, 3점 미만이면 빨강, 그 외 하양입니다.
