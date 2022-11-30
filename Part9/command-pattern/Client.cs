namespace command_pattern
{
    /// <summary>
    /// creates a ConcreteCommand object and sets its receiver.
    /// </summary>
    internal class Client {
        static void Main(string[] args) {
            var receiver = new Receiver();

            var command = new ConcreteCommand(receiver);

            var invoker = new toolkit.Invoker();
            invoker.command = command;
            invoker.request();  // do something.

            System.Console.ReadLine();
        }
    }

    /// <summary>
    /// <list type="bullet">
    /// <item><description>knows how to perform the operations associated with carrying out a request.</description></item>
    /// <item><description>Any class may serve as a Receiver.</description></item>
    /// </list>
    /// </summary>
    internal class Receiver {
        internal void action() {
            System.Console.WriteLine("do something.");
        }
    }

    /// <summary>
    /// <list type="bullet">
    /// <item><description>defines a binding between a Receiver object and an action.</description></item>
    /// <item><description>implements Execute by invoking the corresponding operation(s) on Receiver.</description></item>
    /// </list>
    /// </summary>
    internal class ConcreteCommand : toolkit.Command {
        private readonly Receiver receiver;

        public ConcreteCommand(Receiver receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.action();
        }
    }
}

namespace toolkit
{
    /// <summary>
    /// declares an interface for executing an operation:
    /// <list type="bullet">
    /// <item><description>decouples the object that invokes the operation from the one that knows how to perform it.</description></item>
    /// <item><description>are first-class objects. They can be manipulated and extended like any other object.</description></item>
    /// </list>
    /// </summary>
    public interface Command {
        void Execute();
    }

    /// <summary>
    /// asks the command to carry out the request:
    /// <list type="bullet">
    /// <item><description>doesn't know which subclass of Command they use.</description></item>
    /// <item><description>has no way of knowing the receiver of the request or the operations that will carry it out.</description></item>
    /// </list>
    /// </summary>
    public class Invoker {
        public Command command { get; set; }

        public void request() {
            command.Execute();
        }
    }
}
