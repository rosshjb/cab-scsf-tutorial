1. Shell ���(������Ʈ)�� ����� �� ���(������Ʈ)�� ������ ���� ����� ���� �߰�:
	- `Microsoft.Practices.CompositeUI.dll`
	- `Microsoft.Practices.CompositeUI.WinForms.dll`
	- `Microsoft.Practices.ObjectBuilder.dll`
2. Shell ������Ʈ�� ������ �� ��⿡ ������ ���� `ModuleInit`�� ����� Ŭ������ �����, `Load` �޼��带 �������̵��Ͽ� Shell���� �ش� ����� �ε����� �� ����� �ڵ带 �ۼ��Ѵ�:
	```cs
	using Microsoft.Practices.CompositeUI;

	namespace Blue
	{
		public class Module : ModuleInit
		{
			public override void Load()
			{
				base.Load();

				Form1 form = new Form1();
				form.Show();
			}

		}
	}
	```
3. Shell ������Ʈ ���� �� �ٸ� ��⵵ �ε��� �� �ֵ��� ������ ���� `Main` �ڵ带 �����Ѵ�(`FormShellApplication`�� ��� �� `Run` ȣ��):
	```cs
	using Microsoft.Practices.CompositeUI;
	using Microsoft.Practices.CompositeUI.WinForms;
	using System;

	namespace Shell
	{
		public class Program : FormShellApplication<WorkItem, Form1>
		{
			/// <summary>
			/// �ش� ���ø����̼��� �� �������Դϴ�.
			/// </summary>
			[STAThread]
			static void Main()
			{
				new Program().Run();
			}
		}
	}
	```
4. Shell ������Ʈ�� `ProfileCatalog.xml`�� ������ ���� ����� ���� �Ӽ����� "��� ���͸��� ����"�� "�׻� ����"�� ����:
	```xml
	<?xml version="1.0" encoding="utf-8" ?>
	<SolutionProfile xmlns="http://schemas.microsoft.com/pag/cab-profile">
		<Modules>
			<ModuleInfo AssemblyFile="Red.exe" />
			<ModuleInfo AssemblyFile="Blue.exe" />
		</Modules>
	</SolutionProfile>
	```
5. Shell ������Ʈ�� ����� �� ���(������Ʈ)�� ���� ������Ƽ���� "��� ���"�� ��� ������ ���͸��� ����(e.g. `..\bin`).
6. �ַ�� �Ӽ����� Shell ����� ���� ������Ʈ�� ����.
7. �ַ�� �����Ͽ� ����.