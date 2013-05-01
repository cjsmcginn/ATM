using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace ATM.Core.Domain
{
    [Serializable]
    public class WorkflowInstance
    {
        public string Id { get; set; }     
        public string InstanceData { get; set; }
    }
}
