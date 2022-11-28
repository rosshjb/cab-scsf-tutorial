# Service 등록 방법

## AddNew, Add, AddOnDemand

`Services` 컬렉션의 `AddNew` 메서드를 이용하면, 서비스 객체를 생성함과 동시에 컬렉션에 등록할 수 있다:

```cs
RootWorkItem.Services.AddNew<Service1>();
```

`Services` 컬렉션의 `Add` 메서드를 이용하면, 기존에 존재하는 서비스 객체를 컬렉션에 등록할 수 있다:

```cs
Service2 svc2 = new Service2();
RootWorkItem.Services.Add<Service2>(svc2);
```

`Services` 컬렉션의 `AddOnDemand` 메서드를 이용하면, 서비스 객체의 placeholder만 컬렉션에 등록하고 실제로 객체를 생성하지는 않는다. 객체가 생성되는 시점은 클라이언트가 해당 서비스 dependency를 요청했을 때 발생된다(스프링 프레임워크의 prototype bean에 대응):

```cs
RootWorkItem.Services.AddOnDemand<Service3>();
```

## XML configuration

`App.config` XML 파일에 서비스 객체를 기술할 수도 있다. 코드 상에 기술되는 것이 아니므로 재컴파일없이도 주입될 서비스 객체의 구체 타입 변경이 런타임에 가능하다:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name="CompositeUI" type="Microsoft.Practices.CompositeUI.Configuration.SettingsSection, Microsoft.Practices.CompositeUI"  allowExeDefinition="MachineToLocalUser" />
	</configSections>
	<CompositeUI>
		<services>
			<add serviceType="components.Service, service-config" instanceType="components.ServiceImpl, service-config" />
		</services>
	</CompositeUI>
</configuration>
```

아래의 형식을 따르지 않으면 `System.Configuration.ConfigurationErrorsException` 예외가 던져진다:

```xml
<add serviceType="인터페이스의 FQCN, 어셈블리 이름" instanceType="클래스의 FQCN, 어셈블리 이름" />
```

## Service attribute

`Services` 컬렉션에서 관리할 클래스 자체에 `Service` attribute를 지정하는 방법도 가능하다:

```cs
public interface IService {
    void foobar();
}

[Service(registerAs: typeof(IService), AddOnDemand = false)]
public class ServiceImpl : IService {
    public void foobar()
    {
        System.Console.WriteLine("do implementation");
    }
}
```

- 해당 서비스 객체는 항상 root `WorkItem`에 추가되므로 `WorkItem`의 계층 관계를 이용할 수 없다.
- CAB가 생성 및 등록하기 때문에 인스턴스화 및 컬렉션에 등록되는 순서는 통제할 수 없게 된다.

CAB는 모듈을 로드할 때 모든 `Service` attribute가 decorated되어 있는 `public` 클래스를 스캔해서 리플렉션을 통해 객체를 생성 및 컬렉션에 등록해놓는다. 물론 해당 모듈은 `ProfileCatalog.xml`에 명시되어 있어야 한다.

# Service 조회 방법

## Contains, Remove

`Services` 컬렉션의 `Contains` 메서드는 주어진 타입의 객체가 컬렉션에서 관리되고 있는지 여부를 리턴하며, `Remove` 메서드는 주어진 타입의 객체를 컬렉션에서 제거한다:

```cs
if (RootWorkItem.Services.Contains<Service4>())
    RootWorkItem.Services.Remove<Service4>();
```

## Get

`Services` 컬렉션의 `Get` 메서드는 컬렉션에서 관리되고 있는 주어진 서비스 타입의 객체를 리턴한다(구체 타입과 분리된 별도의 타입으로 노출된 경우 해당 노출된 타입) — dependency lookup:

```cs
RootWorkItem.Services.AddNew<components.Service, components.IService>();

components.IService svc1 = RootWorkItem.Services.Get<components.IService>();
```

서비스를 찾지 못했을 때 `null`을 리턴하는 대신 `ServiceMissingException` 예외를 던지게 하려면, 오버로딩된 `Get`의 `ensureExists` 파라미터를 이용한다:

```cs
components.IService svc1 = RootWorkItem.Services.Get<components.IService>(ensureExists : true);
```

## ServiceDependency

클라이언트의 ctor의 parameter 또는 setter property에 `ServiceDependency` attribute를 사용하면, 해당 클라이언트 객체가 (생성 및) `WorkItem`에 등록될 때, 주어진 타입의 서비스 객체가 주입된다(구체 타입과 분리된 별도의 타입으로 노출된 경우 해당 노출된 타입) — dependency injection:

```cs
RootWorkItem.Services.AddNew<components.Service, components.IService>();

components.Client client = RootWorkItem.Items.AddNew<components.Client>();
```

```cs
public class Client
{
	// setter injection
	[ServiceDependency]
	public IService service1 { get; set; }

	private IService service2;

	// constructor injection
	public Client([ServiceDependency] IService service2)
	{
		this.service2 = service2;
	}

	public void doSomething()
	{
		service1.foobar();
		service2.foobar();
	}
}
```

## Services의 WorkItem 계층 관계 이용

CAB는 현재 `WorkItem`의 `Services` 컬렉션에서 서비스 객체를 찾지 못하면 root `WorkItem`의 `Services` 컬렉션까지 부모를 거슬러 올라가서 찾아본다:

```cs
RootWorkItem.Services.AddNew<components.Service, components.IService>();

WorkItem childWorkItem = RootWorkItem.WorkItems.AddNew<WorkItem>("child");

// child에서 조회했음에도 root에 등록한 service를 발견할 수 있음.
components.IService svc2 = childWorkItem.Services.Get<components.IService>();
```