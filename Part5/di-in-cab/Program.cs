using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.ObjectBuilder;
using System;

namespace di_in_cab
{
    internal class Program : FormShellApplication<WorkItem, Form1>
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Program().Run();
        }

        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();

            /// dependency 등록
            RootWorkItem.Items.AddNew<LowLevelDetail.Service>("first_service");

            /// 1. 미리 생성된 client를 Items에 등록함과 함께 "first_service" dependency 주입.
            /// 2. client를 Items에 등록하는 과정에서 임의의 GUID를 갖는 dependency 생성 및 Items에 등록 후 주입.
            RootWorkItem.Items.Add(new HighLevelPolicy.Client());

            displayItemsCollection();
        }

        /// <summary>
        /// 출력 예시:
        /// <para>
        /// ITEMS:<br/>
        /// [a4b89ae7-d1a1-4c23-a434-53b0c0ca5906, Microsoft.Practices.CompositeUI.State]<br/>
        /// [466bfefb-3a51-41d2-afe6-6678d5da91e7, di_in_cab.Form1, Text: Form1]<br/>
        /// [first_service, LowLevelDetail.Service]<br/>
        /// [a0fe2489-f4b2-4fee-aa1a-3d6fc6691a1c, HighLevelPolicy.Client]<br/>
        /// [f815d936-5a4a-4d01-aeb8-f4eef3593965, LowLevelDetail.Service]
        /// </para>
        /// </summary>
        private void displayItemsCollection()
        {
            System.Diagnostics.Debug.WriteLine("ITEMS:");
            foreach (System.Collections.Generic.KeyValuePair<string, object> obj in RootWorkItem.Items)
            {
                System.Diagnostics.Debug.WriteLine(obj);
            }
        }
    }
}

namespace HighLevelPolicy
{
    public class Client
    {
        [ComponentDependency("first_service")]
        public LowLevelDetail.Service Service1 { get; set; }

        [CreateNew]
        public LowLevelDetail.Service Service2 { get; set; }
    }
}

namespace LowLevelDetail
{
    public class Service {}
}