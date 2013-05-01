using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Activities.UnitTesting;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;
using System.Activities;
using ATM.Core.Services;
using ATM.Data.Raven;
using System.Activities.DynamicUpdate;

namespace ATM.Strategies.Workflows.Tests
{
    [TestClass]
    public class WorkflowRunner
    {
        Double[] GetTestData()
        {
            List<Double> result = new List<double>();
            TestResources.goog.Split(System.Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList().ForEach(line =>
                {
                    var items = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    result.Add(double.Parse(items[1]));
                });
            return result.ToArray();
        }
        [TestMethod]
        public void TestSMA()
        {
            Dictionary<string,object> arguments = new Dictionary<string,object>();
            var testData = GetTestData();
            var output = new Double[testData.Length - 1];
            arguments.Add("inputArray",testData);
            arguments.Add("timePeriod",20);      
            
            var target = WorkflowInvokerTest.Create(new ATM.Strategies.Workflows.SMA());

            var results = target.Invoker.Invoke(arguments);
            Assert.IsNotNull(results["result"]);
        }

        [TestMethod]
        public void TestPersistence()
        {
            IDataService ds = new RavenDataService();
            Dictionary<string, object> inputs = new Dictionary<string, object>();
            var testData = GetTestData();
            var output = new Double[testData.Length - 1];
            inputs.Add("inputArray", testData);
            inputs.Add("timePeriod", 20);     

            IWorkflowInstanceService workflowInstanceService = new WorkflowInstanceService(ds);
            var store = new WorkflowInstanceStore(workflowInstanceService);
            var activity = new ATM.Strategies.Workflows.Persistence();
            var wfa = WorkflowApplicationTest.Create<ATM.Strategies.Workflows.Persistence>(activity, inputs);
          
            wfa.InstanceStore = store;
            wfa.PersistableIdle += (args) =>
                {
                    return PersistableIdleAction.Persist;
                };
            wfa.Idle += (args) =>
                {
                    var x = "Y";
                };
            wfa.Completed += (args) =>
                {
                    //Assert.IsFalse(true);
                };
           
            wfa.TestActivity();
            wfa.WaitForPersistableIdleEvent();
            wfa.Persist();
        
       
            
        }

        [TestMethod]
        public void TestLoad()
        {
            var id = new Guid("f446e62b-1aa5-470e-8a79-600bf283c8b4");
            IDataService ds = new RavenDataService();
            IWorkflowInstanceService workflowInstanceService = new WorkflowInstanceService(ds);
            var store = new WorkflowInstanceStore(workflowInstanceService,id);

            Dictionary<string, object> inputs = new Dictionary<string, object>();
            var activity = new ATM.Strategies.Workflows.Persistence();
            
           var wfa = WorkflowApplicationTest.Create<ATM.Strategies.Workflows.Persistence>(activity);
        
          
     
            wfa.TestWorkflowApplication.InstanceStore = store;
            wfa.PersistableIdle += (args) =>
            {
                return PersistableIdleAction.Persist;
            };
            wfa.Completed += (args) =>
            {
                Assert.IsFalse(true);
            };
            wfa.Idle += (args) =>
                {
                };
            wfa.OnUnhandledException = (a) =>
                {
                    Console.Write(a.UnhandledException.Message);
                    return UnhandledExceptionAction.Abort;
                };
            wfa.Completed += (args) =>
            {
                //Assert.IsFalse(true);
            };
      //var um = new DynamicUpdateMap();
            
           wfa.TestWorkflowApplication.Load(id);


            
           var result = wfa.TestWorkflowApplication.ResumeBookmark("NextQuote",1.31d);

           System.Threading.Thread.Sleep(10000);
          // wfa.Persist(new TimeSpan(10000000));
           
           
        }
    }
}
