# Unity 코드 SOLID 점검 리포트

대상 폴더: `Assets/02Scripts`

## 총평
- 인터페이스(`IInteractable`)를 사용해 상호작용 대상을 추상화한 점은 **DIP/OCP 관점에서 좋은 시작**입니다.
- 다만 여러 컴포넌트가 입력 처리, 도메인 로직, UI 제어를 한 클래스에서 동시에 처리해 **SRP 위반**이 자주 보입니다.
- `ItemCatalogManager.Instance` 같은 전역 싱글톤 의존이 많아질수록 테스트 및 교체 가능성이 낮아질 수 있습니다.

## 원칙별 리뷰

### 1) SRP (단일 책임 원칙)

#### 좋은 점
- `PlayerInteract`는 상호작용 트리거 수집 + 실행에 초점을 맞추고 있습니다.

#### 개선 포인트
- `PlayerInventory`가 인벤토리 데이터 관리, 골드 관리, UI 패널 토글, 커서 제어까지 담당하고 있어 책임이 큽니다.
  - 데이터/도메인: `itemIdByCount`, `AddItem`, `TrySpendGold`
  - UI 제어: `inventoryPanel.SetActive`, `equipmentPanel.SetActive`, `Cursor.lockState`
- `PlayerAttack`도 입력 감지, 회전 계산, 애니메이션 트리거, 무기 콜라이더 제어, 데미지 상태 관리를 모두 포함합니다.

#### 권장 리팩터링
- `PlayerInventoryDomain`(아이템/골드), `InventoryUIController`(패널/커서), `InventoryInputHandler`(입력)로 분리.
- `PlayerAttack`을
  - `AttackInputHandler`
  - `AttackAnimatorBridge`
  - `DamageCalculator`
  로 분리해 단위 테스트 가능성 향상.

### 2) OCP (개방-폐쇄 원칙)

#### 좋은 점
- `IInteractable` 기반 설계 덕분에 새 상호작용 오브젝트를 추가할 때 `PlayerInteract` 변경 없이 확장 가능합니다.

#### 개선 포인트
- `ShopManager`는 구매 로직(쿨다운/골드 체크/아이템 지급/롤백)과 UI 토글이 강결합이라, 할인 정책/통화 타입 추가 시 내부 수정이 많아질 수 있습니다.

#### 권장 리팩터링
- `IPricePolicy`, `IPurchaseService` 같은 정책 인터페이스를 도입해 확장 지점을 분리.

### 3) LSP (리스코프 치환 원칙)

#### 관찰
- 현재 인터페이스 계층이 복잡하지 않아 명백한 LSP 위반은 크지 않습니다.
- 다만 `IInteractable.Interact(PlayerInteract player)` 시그니처는 구현체가 `PlayerInteract` 구체 타입 세부사항에 기대기 쉬워, 대체 가능성을 떨어뜨릴 여지가 있습니다.

#### 권장 리팩터링
- `IInteractionContext`(예: 위치, 인벤토리 접근, 실행 주체 ID)로 매개변수를 추상화하면 대체 가능성 증가.

### 4) ISP (인터페이스 분리 원칙)

#### 좋은 점
- `IInteractable`은 작고 명확합니다.

#### 개선 포인트
- 인벤토리/상점/아이템 카탈로그에서 앞으로 기능이 늘어날 경우, 큰 매니저 클래스 하나에 메서드가 집중될 가능성이 큽니다.

#### 권장 리팩터링
- 예시:
  - `IReadOnlyInventory`
  - `IInventoryWriter`
  - `IGoldWallet`
  - `IItemCatalogReader`

### 5) DIP (의존성 역전 원칙)

#### 좋은 점
- `PlayerInteract -> IInteractable` 의존은 DIP 방향이 바람직합니다.

#### 개선 포인트
- `ItemCatalogManager.Instance` 정적 접근은 상위 정책이 구체 구현에 직접 의존하게 만들어 테스트 대역 주입이 어렵습니다.
- `GetComponent`, `GameObject.Find`를 런타임 로직에서 직접 호출하면 교체 가능성과 예측 가능성이 낮아집니다.

#### 권장 리팩터링
- `IItemCatalog` 인터페이스 주입(SerializeField로 ScriptableObject 브리지 또는 Zenject/VContainer 등 DI 도입).
- 참조는 가능한 Inspector 할당 + 초기 검증(Null Guard)로 고정.

## 우선순위 액션 플랜 (현실적인 순서)
1. `PlayerInventory`에서 UI 토글/커서 제어를 별도 컴포넌트로 분리 (SRP 개선 효과 큼).
2. `ItemCatalogManager.Instance` 직접 호출부를 `IItemCatalog` 래퍼로 치환 (DIP 개선).
3. `ShopManager` 구매 로직을 서비스 클래스로 분리하고 정책 인터페이스 추가 (OCP 개선).
4. `PlayerAttack` 데미지 계산/애니메이션/입력 처리 분리 (SRP + 테스트성 개선).

## 빠른 체크리스트
- [ ] 입력 처리와 도메인 로직이 한 클래스에 섞여 있지 않은가?
- [ ] UI 상태 변경 코드가 도메인 객체 내부에 들어가 있지 않은가?
- [ ] 싱글톤 정적 접근 대신 인터페이스 주입이 가능한가?
- [ ] 새 정책(할인/버프/통화)을 추가할 때 기존 클래스 수정이 최소화되는가?
