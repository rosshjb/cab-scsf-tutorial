namespace Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            HighLevelPolicy.Client obj = client();
            obj.doHighLevelLogic();

            System.Console.ReadLine();
        }

        private readonly static Framework.HumbleFactory factory =
            new Framework.HumbleFactory(System.Configuration.ConfigurationManager.AppSettings);

        /// <summary>
        /// TODO : spring framework의 XML configuration에서 <c>bean</c> element처럼, object의 dependency를 child element로 표현할 수도 있을 것. 예를 들어:
        /// <example>
        /// <code>
        /// &lt;object type="HighLevelPolicy.Client">
        ///     &lt;object type="LowLevelDetail.ServiceVendor1" />
        /// &lt;/object>
        /// </code>
        /// </example>
        /// 그러면 framework의 client code가 다음처럼 되어 dependency를 코드 상에서 표현하지 않아도 될 수 있을 것이다:
        /// <example>
        /// <code>
        /// return factory.getObject&lt;HighLevelPolicy.Client>("client");
        /// </code>
        /// </example>
        /// App.config의 custom section, XML, JSON 등의 파일에 configuration을 작성하면 될 것.
        /// </summary>
        /// <returns>...</returns>
        private static HighLevelPolicy.Client client()
        {
            return factory.getObject<HighLevelPolicy.Client>("client", service());
        }

        private static HighLevelPolicy.Service service()
        {
            return factory.getObject<HighLevelPolicy.Service>("service");
        }
    }
}

namespace Framework
{
    using Source = System.Collections.Specialized.NameValueCollection;

    public class HumbleFactory
    {
        private readonly Source configurationSource;

        public HumbleFactory(Source configurationSource)
        {
            this.configurationSource = configurationSource;
        }

        public R getObject<R>(string id, params object[] args)
        {
            string typeString = configurationSource[id];
            System.Type type = System.Type.GetType(typeString);

            return (R)System.Activator.CreateInstance(type, args);
        }
    }
}

namespace HighLevelPolicy
{
    public class Client
    {
        private readonly Service service;

        public Client(Service service)
        {
            this.service = service;
        }

        public void doHighLevelLogic()
        {
            service.foobar();
        }
    }

    public interface Service
    {
        void foobar();
    }
}

namespace LowLevelDetail
{
    public class ServiceVendor1 : HighLevelPolicy.Service
    {
        public void foobar()
        {
            System.Console.WriteLine("do low level detail in implementation 1");
        }
    }

    public class ServiceVendor2 : HighLevelPolicy.Service
    {
        public void foobar()
        {
            System.Console.WriteLine("do low level detail in implementation 2");
        }
    }
}