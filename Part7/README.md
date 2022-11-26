# Services

service는 본질적으로 business/technical operation을 제공하는 객체(모듈)를 의미한다. CAB에서 `WorkItem`의 `Services` 컬렉션도 그러한 객체들만의 컨테이너이다:

- service layer
- domain service, application service

따라서 `Servies` 컬렉션과 `Items` 컬렉션은 그 책임부터 분명히 구분된다:

- `Services` : 정의된 business/technical functionality를 가진 객체를 타 모듈에서 사용될 목적으로 게이트웨이의 형태로 노출하기 위함.
- `Items` : 이외의 임의의 객체들.

# Services vs. Items

## 타입 유일성

- `Services` 컬렉션은 주어진 타입에 대해 반드시 하나의 인스턴스만 포함할 수 있다. 그 이상의 인스턴스를 추가하려고 하면 `ArgumentException`이 던져진다.
  - 그래서 `ServiceDependency` attribute 사용 시 타입만으로 의존 객체를 주입받을 수 있다.
- `Items` 컬렉션은 그런 제약이 없다; 그렇기 때문에 동일 타입의 객체를 서로 구분하기 위해 유일한 문자열 ID가 부여된다(직접 부여하지 않으면 CAB가 GUID를 할당한다).
  - 그래서 `ComponentDependency` attribute 사용 시 반드시 ID를 지정해야 의존 객체를 주입받을 수 있다.

## 계층 검색

- `Services`에 있는 객체를 검색할 때(`ServiceDependency`)는 현재 `WorkItem`의 `Services` 컬렉션 뿐만 아니라 부모 `WorkItem`을 거쳐 root `WorkItem`까지 검색한다 — 그래도 찾지 못하면 attribute의 `Required` parameter에 따라 `null` 리턴 또는 `ServiceMissingException` 예외 발생.
- `Items`에 있는 객체를 검색할 때(`ComponentDependency`)는 해당 `WorkItem`의 `Items`에서만 검색한다 — 그래도 찾지 못하면 attribute의 `Required` parameter에 따라 `null` 리턴 또는 `DependencyMissingException` 예외 발생.

# DL

`Services`/`Items`/`WorkItems`의 `Get` 메서드를 이용하면 dependency lookup으로 객체를 능동적으로 조회해서 가져올 수 있다:

```cs
WorkItem rootWorkItem = RootWorkItem.Services.Get<WorkItem>(ensureExists : true);
Client1 client1 = RootWorkItem.Items.Get<Client1>("client1");
```

# service dependency interface

`Services` 컬렉션에 객체를 추가할 때 서비스에 대해 클라이언트 쪽에서 사용될 타입(일반적으로 인터페이스 타입)을 명시할 수 있다(서비스의 인터페이스):

```cs
RootWorkItem.Services.AddNew<components.ServiceImplementation, components.AbstractService>();
components.AbstractService iService = RootWorkItem.Services.Get<components.AbstractService>();
iService.foobar();
```