using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

            return _dataService.Load<WorkflowInstance>(id);
            
         
        }

        public WorkflowInstance CreateWorkflowInstance(Guid id)
        {
            //var doc = new XmlDocument();
            //doc.LoadXml("<InstanceValues/>");           
            var result = new WorkflowInstance { Id = id.ToString()};
            return result;
        }

        public void Save(WorkflowInstance workflowInstance)
        {
            _dataService.Save<WorkflowInstance>(workflowInstance);
        }
    }
}
