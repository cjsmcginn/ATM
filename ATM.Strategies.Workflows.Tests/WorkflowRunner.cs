using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Activities.UnitTesting;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;
using System.Activities;

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
    }
}
