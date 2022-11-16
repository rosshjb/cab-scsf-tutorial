# WorkItem

- `Microsoft.Patterns.CompositeUI.WorkItem`
- use case 단위의 DI container : use case 수행을 목적으로 하는 논리적으로 연관된 객체들에 대한 팩토리
- DI 대상 객체들을 유형별로 분류한 컬렉션으로 다룸:
  - `Items` : 유형에 무관한 임의의 객체들
  - `Services` : CAB 서비스 객체들
  - `WorkItems` : 자식 `WorkItem`들
  - state, status

# 컨테이너 계층

- 하나의 `WorkItem`이 자신이 관리하는 컴포넌트 객체 외에도 자식 `WorkItem`들을 가질 수 있음 : 모듈을 계층적으로 구성하고 이 계층 관계를 활용해 다른 모듈의 컴포넌트를 사용하고자 할 때 유용한 성질.
- 이를테면 Shell 모듈이 root WorkItem이 되고 모듈 Blue, Red가 각각 별도의 WorkItem이다; Red에서는 `this.ParentWorkItem.Items["ComponentName"]` 형식으로 root WorkItem의 컴포넌트를 이용할 수 있다(단, tightly coupled - service locator).

# FormShellApplication

```cs
namespace Shell
{
    public class Program : FormShellApplication<WorkItem, Form1>
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 이 시점에 root WorkItem과 Form1이 인스턴스화됨.
            new Program().Run();
        }
    }
}
```

- root WorkItem에는 [`RootWorkItem` 프로퍼티](https://learn.microsoft.com/en-us/dotnet/api/microsoft.practices.compositeui.cabapplication-1.rootworkitem?view=dynamics-usd-3)로 접근 가능