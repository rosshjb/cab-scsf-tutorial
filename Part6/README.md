# constructor injection in CAB

## InjectionConstructor attribute

constructor에 `InjectionConstructor` attribute를 사용하면 객체 생성 시에 dependency가 주입된다(`AddNew` 시에 CAB가 이 attribute를 가진 생성자를 lookup해서 호출함). ctor injection도 setter injection과 마찬가지로 기본적인 DI API(`ComponentDependency`, `CreateNew`, `ServiceDependency`) 사용 방식은 동일하다:

```cs
RootWorkItem.Items.AddNew<User.Service>("svc1");
RootWorkItem.Items.AddNew<User.Client1>("client1");
```

```cs
[InjectionConstructor]
public Client1([ComponentDependency("svc1")] Service svc1, [CreateNew] Service svc2, [ServiceDependency] WorkItem rootWorkItem)
{
    // ...
}
```

- 당연하게도 객체를 `new`로 직접 생성한 뒤에 `Add`로 컨테이너에 객체를 추가하는 경우에는 ctor injection이 동작하지 않음.

## InjectionConstructor attribute의 생략

클래스에 생성자가 하나 뿐이라면 `InjectionConstructor` attribute를 생략할 수 있다:

```cs
RootWorkItem.Items.AddNew<User.Service>("svc3");
RootWorkItem.Items.AddNew<User.Client2>("client2");
```

```cs
public class Client2
    {
        public Client2([ComponentDependency("svc3")] Service svc3, [CreateNew] Service svc4, [ServiceDependency] WorkItem rootWorkItem)
        {
            // ...
        }
    }
```

- 생성자가 둘 이상인데 모두 attribute를 지정하지 않으면 가장 위쪽의 생성자가 호출되는 것으로 보이는데 implementation detail일 수 있으니 주의.

클래스에 생성자가 여러 개이면 CAB는 `InjectionConstructor` attribute를 가진 생성자를 호출한다:

```cs
RootWorkItem.Items.AddNew<User.Service>("svc5");
RootWorkItem.Items.AddNew<User.Client3>("client3");
```

```cs
public Client3(WorkItem rootWorkItem)
{
    throw new NotSupportedException();
}

[InjectionConstructor]
public Client3([ComponentDependency("svc5")] Service svc5, [CreateNew] Service svc6, WorkItem rootWorkItem)
{
    // this is called
}
```

## 암묵적 ServiceDependency

ctor injection에 사용되는 생성자의 dependency parameter에 attribute를 생략하면 암묵적으로 `ServiceDependency`가 적용된다. 즉, 암묵적 `ServiceDependency`가 적용된 상태에서 dependency를 `Services` 컬렉션에서 찾지 못하면, CAB는 dependency 객체를 생성해서 `Services` 컬렉션에 추가한 후 주입한다:

```cs
// Service를 찾을 수 없으므로 먼저 생성되어 Services 컬렉션에 추가된 후에 Client4가 생성될 때 주입된다.
RootWorkItem.Items.AddNew<User.Client4>("client4");
```

```cs
public Client4(Service svc7, WorkItem rootWorkItem)
{
    // ...
}
```

단, 명시적으로 `ServiceDependency` attribute가 적용된 경우에는 동작이 다르다. dependency를 `Services` 컬렉션에서 찾지 못하면 바로 `ServiceMissingException` 예외가 던져진다:

```cs
try {
    RootWorkItem.Items.AddNew<User.Client5>("client5");
} catch (ServiceMissingException e) {
    // 예외 발생: 'Microsoft.Practices.CompositeUI.Services.ServiceMissingException'(Microsoft.Practices.CompositeUI.dll)
    // Service User.NotFoundService is not available in the current context.
    System.Console.WriteLine(e.Message);  
}
User.Client5 client5 = (User.Client5)RootWorkItem.Items["client5"];
client5.printFields();  // NotFoundService is null? True, rootWorkItem is null? True
```

```cs
public class Client5
{
    private NotFoundService svc8;
    private WorkItem rootWorkItem;
    public Client5([ServiceDependency] NotFoundService svc8, WorkItem rootWorkItem)
    {
        this.svc8 = svc8;
        this.rootWorkItem = rootWorkItem;
    }

    public void printFields()
    {
        System.Console.WriteLine($"NotFoundService is null? {svc8 == null}, rootWorkItem is null? {rootWorkItem == null}");
    }
}

public class NotFoundService { }
```

- `NotFoundService` dependency를 `Services` 컬렉션에서 찾지 못해 예외가 발생한다. `Client5` 객체가 생성되어 `Items` 컬렉션에 추가되기는 했으나 이는 implementation detail일 수 있으니 주의.