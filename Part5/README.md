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