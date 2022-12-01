# commands in CAB

## CAB의 command 사용법

> 일반적인 GUI 프레임워크/라이브러리에서 그러하듯, CAB에서도 command (command pattern 추상화)는 visual component(Invoker)에 어떤 event가 발생했을 때 추상화된 형태로 그 이벤트에 반응하는 다른 대상(Receiver)이 자신의 액션을 실행하게끔 하는 데에 사용된다.

`WorkItem`의 `Commands` 컬렉션에 커맨드의 이름을 넘기면 `Command` 객체를 가져올 수 있고 여기에 `AddInvoker` 메서드를 이용하면 Invoker를 추가할 수 있다:

```cs
Command command = RootWorkItem.Commands["ShowMessageCommand"];

ToolStripItem toolStripItemForCommand = this.Shell.toolStrip1.Items["button1"];

command.AddInvoker(toolStripItemForCommand, "Click");
```

- `WorkItem`은 command를 lazy하게 초기화한다. 즉, 기존에 `Commands` 컬렉션에 존재하면 그것을 리턴하고, 그렇지 않으면 생성 후 리턴한다; 실제로는 CAB가 `CommandHandler` attribute를 scan하는 시점에 생성된다.
- `RemoveInvoker` 메서드를 이용하면 Invoker를 제거할 수 있다.

command에 대한 Receiver는 `CommandHandler` attribute로 decorated되어 있는 메서드가 한다. 이 메서드는 `public`이어야 하며 `object sender, EventArgs e`를 parameter로 가져야 한다:

```cs
[CommandHandler("ShowMessageCommand")]
public void button1_command_handler(object sender, EventArgs e)
{
    MessageBox.Show("button using command handler clicked");
}
```

- command가 이 receiver를 인지하고 적절하게 command handler를 호출해줄 수 있으려면, CAB가 이 attribute를 스캔할 수 있어야 하므로 이 메서드가 선언된 클래스의 객체는 `WorkItem`에서 managed되는 객체여야 한다.

## .NET event handler의 단점

.NET event handler는 Invoker와 그에 대한 event handler가 일반적으로 동일 클래스에 있어야 한다(Invoker 클래스의 메서드의 형태로). 그렇다보니 어떤 action이 여러 Invoker들에 의해 범용적으로 요청받아 수행되어야 하는 경우에, event handler를 설정해주는 코드와 Receiver의 action을 실행하는 코드가 여러 곳에서 중복되기 마련이다:

```cs
this.button2.Click += new System.EventHandler(this.button2_Click);
```

```cs
private void button2_Click(object sender, EventArgs e)
{
    MessageBox.Show("buton using .NET event handler clicked");
}
```

하지만 CAB의 command를 이용하면 `WorkItem` 상에 존재하는 임의의 객체에 command handler를 하나만 설치해두고, 해당 command를 발생시킬 Invoker만 추가해주면 된다.