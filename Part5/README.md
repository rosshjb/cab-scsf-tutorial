# DI in CAB

## ServiceDependency

setter에 `ServiceDependency` attribute를 decorate해주면, 해당 module이 로드될 때 CAB framework가 해당 service를 주입해준다:

```cs
public class Module : ModuleInit
{
    [ServiceDependency]
    public WorkItem parentWorkItem { get; set; }

    public override void Load()
    {
        base.Load();

        // root WorkItem에 엑세스한다.
        System.Console.WriteLine($"parentWorkItem in Red module : {parentWorkItem}");

        Form1 form = new Form1();
        form.Show();
    }
}
```

## MDI

child module의 form을, root인 shell form의 child로 만들어 MDI를 구성한다:

1. `AfterShellCreated` 훅 메서드를 이용해, Shell module(project)의 `Form`을 root `WorkItem`의 `Items` 컬렉션에 추가한다:
    ```cs
    internal class Program : FormShellApplication<WorkItem, Form1>
    {
        [STAThread]
        static void Main()
        {
            new Program().Run();
        }

        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            this.Shell.IsMdiContainer = true;

            RootWorkItem.Items.Add(this.Shell, "Shell");
        }
    }
    ```
2. child module들에서는 root `WorkItem`의 `Items` 컬렉션에 엑세스하여 shell의 `Form`을 얻어온다:
    ```cs
    public class Module : ModuleInit
    {
        [ServiceDependency]
        public WorkItem ParentWorkItem { get; set; }

        public override void Load()
        {
            base.Load();

            Form shell = (Form)ParentWorkItem.Items["Shell"];

            Form1 form = new Form1();
            form.MdiParent = shell;
            form.Show();
        }
    }
    ```

API:

1. Shell module이 상속하는 `FormShellApplication`에 대하여, shell이 만들어진 후에 `AfterShellCreated` 이벤트가 발생한다.
2. `WorkItem`의 `Items` 컬렉션에 컴포넌트를 추가할 때, 컴포넌트를 식별하기 위한 ID를 지정할 수 있다.

DI가 동작하려면, 일반적으로 주입되는 객체(이하 dependency)는 물론 주입받는 객체(이하 client)까지 모두 container managed여야 한다:

- client가 dependency를 DI받으려면, 컨테이너가 dependency를 찾을 수 있어야 하므로 dependency는 container에서 managed되어야 한다.
  - setter에 `CreateNew` attribute 사용 시, dependency가 생성되어 `WorkItem`의 `Items` 컬렉션에 추가된 후 DI 발생.
- 컨테이너가 client에 dependency를 DI해주려면, 마찬가지로 컨테이너는 client에 대해서 생성과 dependency 설정 방법을 알고 있어야 하므로 container에서 managed되어야 한다.
  - `WorkItem`의 적절한 컬렉션의 `AddNew`를 호출했을 때, client 객체 생성과 함께 dependency의 DI 발생.
  - `WorkItem`의 적절한 컬렉션의 `Add`에 기존에 존재하는 client 객체를 넘겼을 때 dependency의 DI 발생.
  - `WorkItem`의 컬렉션에 `AddNew`/`Add` 했을 때, 적절하게 객체를 생성 및 dependency를 lookup하여 resolution하는 것은 CAB의 ObjectBuilder(i.e. Factory)의 책임.

## CAB의 DI 유형

1. `ComponentDependency(string id)` : 기존에 `WorkItem`의 `Items` 컬렉션에 존재하는 객체를 setter에서 주입받을 때 사용하는 attribute. 동일 타입의 여러 인스턴스가 존재할 수 있으므로 id를 명시해야 한다(객체를 등록하는 쪽에서 id를 생략했다면, CAB가 할당한 GUID를 id로 갖게 된다). 객체를 찾을 수 없으면 `DependencyMissingException`이 던져진다:
    ```cs
    [ComponentDependency(string id, [bool CreateIfNotFound, bool Required, System.Type Type])]
    ```
2. `ServiceDependency` : 기존에 `WorkItem`의 `Services` 컬렉션에 존재하는 객체를 setter에서 주입받을 때 사용하는 attribute. 하나의 `Services` 컬렉션에 임의 타입의 인스턴스는 하나만 존재할 수 있다(그래서 id가 필요없음):
    ```cs
    [ServiceDependency([bool Required, System.Type Type])]
    ```
3. `CreateNew` : 객체가 새로 만들어져 `WorkItem`의 `Items` 컬렉션에 추가된 후 setter에서 주입받을 때 사용하는 attribute:
    ```cs
    [CreateNew]
    ```

## ModuleInit과 WorkItem의 DI

- module이 `ServiceDependency` attribute를 통해 `WorkItem`을 주입받을 수 있는데, 이는 주입받는 module 또한 container managed object라는 뜻이다. 실제로 CAB는 module을 생성하면서 root `WorkItem`의 `Items` 컬렉션에 `ModuleInit` 객체들을 추가한다.
- 마찬가지로 module이 `ServiceDependency` attribute를 통해 `WorkItem`을 주입받을 수 있다는 것은, 주입되는 dependency인 `WorkItem` 또한 container managed object라는 뜻이다. 실제로 CAB는 임의의 `WorkItem`을 그 `WorkItem`의 `Services` 컬렉션에 추가한다.
- (번외) Shell form 또한 자동으로 root `WorkItem`의 `Items` 컬렉션에 추가되는 container managed object이다. 그러므로 dependency를 주입받을 수 있다.

## Items, Services, WorkItems

`WorkItem`은 자식 `WorkItem`들을 담는 `WorkItems` 컬렉션을 통해 계층 관계를 구성한다:

- 임의의 `WorkItem`의 `Items` 컬렉션에서 managed되는 dependency들은 동일 `WorkItem`의 객체에 대해서만 DI된다; 동일 `WorkItem` 상에 있지 않음에도 객체를 주입받으려 시도할 경우 `DependencyMissingException`.
- 하지만 임의의 `WorkItem`의 `Services` 컬렉션에서 managed되는 dependency들은 임의의 자손 `WorkItem`의 객체에 얼마든지 DI될 수 있다; `WorkItem`의 `Services` 컬렉션에서 찾을 수 없으면 조상을 타고 올라가 root 까지 찾아보기 때문.