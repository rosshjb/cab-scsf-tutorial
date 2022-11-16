1. Shell 모듈(프로젝트)을 비롯한 각 모듈(프로젝트)에 다음과 같은 어셈블리 참조 추가:
	- `Microsoft.Practices.CompositeUI.dll`
	- `Microsoft.Practices.CompositeUI.WinForms.dll`
	- `Microsoft.Practices.ObjectBuilder.dll`
2. Shell 프로젝트를 제외한 각 모듈에 다음과 같이 `ModuleInit`을 상속한 클래스를 만들고, `Load` 메서드를 오버라이딩하여 Shell에서 해당 모듈을 로드했을 때 실행될 코드를 작성한다:
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
3. Shell 프로젝트 시작 시 다른 모듈도 로드할 수 있도록 다음과 같이 `Main` 코드를 수정한다(`FormShellApplication`의 상속 및 `Run` 호출):
	```cs
	using Microsoft.Practices.CompositeUI;
	using Microsoft.Practices.CompositeUI.WinForms;
	using System;

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
				new Program().Run();
			}
		}
	}
	```
4. Shell 프로젝트에 `ProfileCatalog.xml`을 다음과 같이 만들고 파일 속성에서 "출력 디렉터리에 복사"를 "항상 복사"로 수정:
	```xml
	<?xml version="1.0" encoding="utf-8" ?>
	<SolutionProfile xmlns="http://schemas.microsoft.com/pag/cab-profile">
		<Modules>
			<ModuleInfo AssemblyFile="Red.exe" />
			<ModuleInfo AssemblyFile="Blue.exe" />
		</Modules>
	</SolutionProfile>
	```
5. Shell 프로젝트를 비롯한 각 모듈(프로젝트)의 빌드 프로퍼티에서 "출력 경로"를 모두 동일한 디렉터리로 수정(e.g. `..\bin`).
6. 솔루션 속성에서 Shell 모듈을 시작 프로젝트로 설정.
7. 솔루션 빌드하여 시작.