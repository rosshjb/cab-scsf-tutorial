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