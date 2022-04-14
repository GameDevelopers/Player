# Player
 DarkRogue 플레이어 기능 test
 
 ![image](https://user-images.githubusercontent.com/86696817/162695043-e53398f9-e1fd-43ee-8c86-bfae3df7add6.png)


## 계획 

### 1주차 : 20220410까지
< 플레이어 >

움직임 먼저 구현
- 양쪽 : 방향키 / 0407 clear
- 점프(2번) : spacebar / 0407 clear
- 대쉬 : x / 0407 clear
- 벽점프 : 벽에 붙은 뒤 spacebar / 0407 clear

### 2주차 : 20220417까지
< 플레이어 >

공격 구현
 - 위 : 방향키 위쪽 누르면서 Z / 0414 clear
 - 아래 : 점프시 방향키 아래쪽 누르면서 Z / 0414 clear
 - 전면 : Z / 0414 clear
일단 있는 애니메이션(이 10Ge같은거 다른 분꺼 보면서 해도 하드함)은 적용.

애니메이션이 없는 부분은 게임 실행 시 갑자기 캐릭터 사라짐. > 애니메이션 적용을 위한 스프라이트 추가 필요.

공격시 나타나는 이미지 변경이나 파티클 효과는 추후에 추가. > 눈에 안거슬릴 정도만.

적용할 공격 이미지 스프라이트도 추가 필요.

### 3주차 : 20220424까지
< 플레이어 >

체력 구현 
 - 체력바 or 아이작처럼 소모성 체력 > 후자일듯
 - 몬스터(Enemy) or 작은 장애물(Obstacle)에 피격시 -1 > 피격 애니메이션 on
 - 체력이 0이 되서 죽으면 처음으로.

스킬 획득
 - 플레이어가 특정 태그 or 마스크를 가진 석상에 부딫히면 아래 구현한 스킬을 획득하는 것으로 하면 어떨까함.

 ∴ 수일님이 해주시면 좋은 것 > 맵 만드실 때 석상 두개 깔아주셨으면합니다.(콜라이더 추가해서)
   
    석상의 태그나 마스크는 HealStatue / SkillAttackStatue 같은 형식으로 추가해주시면 더 편할 거 같습니다.

스킬 구현 
 - 회복 : 체력 완성하고 할 예정.
 - 원거리 공격 > 연쇄(여러마리) or 도트(여러대) 만들어보고 더 괜찮은 걸로 할 예정.
   
### 4주차 : 20220431까지
