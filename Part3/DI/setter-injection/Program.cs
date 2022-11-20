namespace Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            HighLevelPolicy.Client obj = Factory.client();
            obj.doHighLevelLogic();

            System.Console.ReadLine();
        }
    }

    internal static class Factory
    {
        internal static HighLevelPolicy.Client client()
        {
            HighLevelPolicy.Client obj = new HighLevelPolicy.Client();
            obj.Service = service();

            return obj;
        }

        internal static HighLevelPolicy.Service service()
        {
            string typeName = System.Configuration.ConfigurationManager.AppSettings["service"];
            System.Type type = System.Type.GetType(typeName);

            return (HighLevelPolicy.Service)System.Activator.CreateInstance(type);
        }
    }
}

namespace HighLevelPolicy
{
    public class Client
    {
        public Service Service { get; set; }

        public void doHighLevelLogic()
        {
            Service.foobar();
        }
    }

    public interface Service
    {
        void foobar();
    }
}

namespace LowLevelDetail
{
    public class ServiceVendor : HighLevelPolicy.Service
    {
        public void foobar()
        {
            System.Console.WriteLine("do low level detail in implementation");
        }
    }
}