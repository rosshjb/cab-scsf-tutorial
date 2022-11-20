namespace Configuration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Framework.Container container = new Framework.Container(new SourceAdapter());

            container.registerObjects();
            container.registerInjectors(new System.ValueTuple<string, object>[] {
                ("client", container.getObject<HighLevelPolicy.Service>("service")),
                ("service", new VendorNameInjector("awesome vendor")),
            });

            HighLevelPolicy.Service service = container.getObject<HighLevelPolicy.Service>("service");
            service.foobar();
             
            System.Console.ReadLine();
        }
    }

    internal class SourceAdapter : Framework.ConfigurationSource
    {
        public string this[string key] => System.Configuration.ConfigurationManager.AppSettings[key];

        public string[] keys => System.Configuration.ConfigurationManager.AppSettings.AllKeys;
    }

    internal class VendorNameInjector : Framework.Injector
    {
        private string name;

        public VendorNameInjector(string name)
        {
            this.name = name;
        }

        public void inject(object target)
        {
            ((LowLevelDetail.SetVendorName)target).setVendorName(name);
        }
    }
}

namespace Framework
{
    using System.Linq;
    using System.Collections.Generic;
    
    public class Container
    {
        private ConfigurationSource source;

        private Dictionary<string, object> objects;

        public Container(ConfigurationSource source)
        {
            this.source = source;
            objects = new Dictionary<string, object>();
        }

        /// <summary>
        /// configuration source에서 설정 정보를 가져와, 컨테이너에서 관리할 객체를 등록한다.
        /// </summary>
        public void registerObjects()
        {
            source.keys
                .Where(key => key.EndsWith("_object"))
                .Select(key => (key : key.Split('_')[0], value : source[key]))
                .ToList()
                .ForEach(pair =>
                {
                    System.Type type = System.Type.GetType(pair.value);
                    objects[pair.key] = System.Activator.CreateInstance(type);
                });
        }

        /// <summary>
        /// configuration 측으로부터 Injector를 제공받음으로써, target 객체들에 dependency들이 injection될 수 있도록 한다.
        /// </summary>
        /// <param name="injectors"></param>
        public void registerInjectors(System.ValueTuple<string, object>[] injectors)
        {
            injectors.ToList()
                .ForEach(pair => ((Injector)pair.Item2).inject(getObject<object>(pair.Item1)));
        }

        public R getObject<R>(string key)
        {
            return (R)objects[key];
        }
    }

    public interface ConfigurationSource
    {
        string this[string key] { get; }

        string[] keys { get; }
    }

    public interface Injector
    {
        void inject(object target);
    }
}

namespace HighLevelPolicy
{
    /// <summary>
    /// Client는 Injector에서 사용될 목적으로 정의된 인터페이스 SetService를 구현한다.
    /// 특정 인터페이스 구현을 해야 한다는 사실에 큰 부담은 없는데, 왜냐하면 인터페이스가 동일 수준의 클라이언트 측에 정의되어 있고,
    /// dependency injection을 사용하지 않더라도 여전히 dependency를 수동으로 넘겨받기 위한 setter로 사용될 수 있기 때문이다.
    /// </summary>
    public class Client : SetService
    {
        private Service service;

        public void setService(Service service)
        {
            this.service = service;
        }

        public void doHighLevelLogic()
        {
            service.foobar();
        }
    }

    /// <summary>
    /// Injector에서 사용될 목적으로 정의된 인터페이스. Client의 dependency인 Service를 injection한다.
    /// </summary>
    public interface SetService
    {
        void setService(Service service);
    }

    public interface Service
    {
        void foobar();
    }
}

namespace LowLevelDetail
{
    /// <summary>
    /// ServiceVendor는 스스로가 Client에 대한 Injector로 사용되었다; 특정 인터페이스를 구현해야 하므로 어디까지나 타입이 통제 가능할 때 사용할 수 있는 방식이다.<br/>
    /// 한 편 ServiceVendor 또한 자신의 dependency(vendor name)를 갖는데, 이를 injection받기 위해 별도의 객체 VendorNameInjector의 도움을 받는다;
    /// 이는 vendor name의 타입 string이 빌트인 타입이어서 통제가 불가능하므로 스스로 Injector가 될 수 없기 때문이다.
    /// </summary>
    public class ServiceVendor : HighLevelPolicy.Service, SetVendorName, Framework.Injector
    {
        private string vendor;

        public void foobar()
        {
            System.Console.WriteLine($"[{vendor}] do low level detail in implementation");
        }

        public void setVendorName(string name)
        {
            vendor = name;
        }

        public void inject(object target)
        {
            ((HighLevelPolicy.SetService)target).setService(this);
        }
    }
    
    /// <summary>
    /// Injector에서 사용될 목적으로 정의된 인터페이스. ServiceVendor의 dependency인 vendor name을 injection한다.
    /// </summary>
    public interface SetVendorName
    {
        void setVendorName(string name);
    }
}
