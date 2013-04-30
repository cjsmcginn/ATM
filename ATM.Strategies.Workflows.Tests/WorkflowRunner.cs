using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Activities.UnitTesting;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;
using System.Activities;
using ATM.Core.Services;
using ATM.Data.Raven;

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
            wfa.Completed += (args) =>
                {
                    //Assert.IsFalse(true);
                };
           
            wfa.TestActivity();
            wfa.WaitForCompletedEvent(new TimeSpan(100000000));
            //wfa.Persist(new TimeSpan(100000));
            
            
        }

        [TestMethod]
        public void TestLoad()
        {
            var id = new Guid("5da8d0ca-92ad-4d28-b6e3-45fc22fdd94e");
            IDataService ds = new RavenDataService();
            IWorkflowInstanceService workflowInstanceService = new WorkflowInstanceService(ds);
            var store = new WorkflowInstanceStore(workflowInstanceService,id);
            var activity = new ATM.Strategies.Workflows.Persistence();

            
            var wfa = WorkflowApplicationTest.Create<ATM.Strategies.Workflows.Persistence>(activity);
            wfa.InstanceStore = store;
            wfa.Load(id, new TimeSpan(1000));

           
        }
    }
}
