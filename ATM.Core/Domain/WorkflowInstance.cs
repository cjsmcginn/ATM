using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ATM.Core.Domain
{
    public class WorkflowInstance
    {
        public string Id { get; set; }
        public XDocument InstanceData { get; set; }
    }
}
