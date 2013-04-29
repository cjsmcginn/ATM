using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Services
{
    public interface IWorkflowInstanceService
    {
        WorkflowInstance GetById(Guid id);
        WorkflowInstance CreateWorkflowInstance(Guid id);
        void Save(WorkflowInstance workflowInstance);
    }
}
