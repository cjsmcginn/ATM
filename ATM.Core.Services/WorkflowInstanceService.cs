using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ATM.Core.Services
{
    public class WorkflowInstanceService:IWorkflowInstanceService
    {

        #region class variables
        private readonly IDataService _dataService;
        #endregion

        #region constructor
        public WorkflowInstanceService(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion

        public WorkflowInstance GetById(Guid id)
        {
            var key = id.ToString();
            return _dataService.Fetch<WorkflowInstance>((w) => { return w.Id == key; });
         
        }

        public WorkflowInstance CreateWorkflowInstance(Guid id)
        {
            var doc = new XDocument();
            var instanceValues = new XElement("InstanceValues");
            doc.Add(instanceValues);
            var result = new WorkflowInstance { Id = id.ToString(), InstanceData=doc };
            return result;
        }

        public void Save(WorkflowInstance workflowInstance)
        {
            _dataService.Save<WorkflowInstance>(workflowInstance);
        }
    }
}
