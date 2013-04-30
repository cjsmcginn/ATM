using ATM.Core.Services;
using System;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.ServiceModel.Persistence;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.Runtime.Hosting;
using System.Xml;
using System.Xml.Linq;

namespace ATM.Strategies.Workflows
{
    public class WorkflowInstanceStore:InstanceStore
    {
        Guid ownerInstanceID;
        private readonly IWorkflowInstanceService _workflowInstanceService;

        public WorkflowInstanceStore(IWorkflowInstanceService workflowInstanceService)
            : this(workflowInstanceService,Guid.NewGuid())
        {
                _workflowInstanceService = workflowInstanceService;
        }


        public WorkflowInstanceStore(IWorkflowInstanceService workflowInstanceService,Guid id)
        {
            _workflowInstanceService = workflowInstanceService;
            ownerInstanceID = id;
        }

        protected override IAsyncResult BeginTryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout, AsyncCallback callback, object state)
        {
            
            IDictionary<XName, InstanceValue> data = null;
            //The CreateWorkflowOwner command instructs the instance store to create a new instance owner bound to the instanace handle
            if (command is CreateWorkflowOwnerCommand)
            {
                context.BindInstanceOwner(ownerInstanceID, Guid.NewGuid());
            }//The SaveWorkflow command instructs the instance store to modify the instance bound to the instance handle or an instance key
            else if (command is SaveWorkflowCommand)
            {
                SaveWorkflowCommand saveCommand = (SaveWorkflowCommand)command;
                data = saveCommand.InstanceData;
                Save(data);
            }//The LoadWorkflow command instructs the instance store to lock and load the instance bound to the identifier in the instance handle
            else if (command is LoadWorkflowCommand)
            {
                //string fileName = IOHelper.GetFileName(this.ownerInstanceID);
                var instance = _workflowInstanceService.GetById(ownerInstanceID);
                try
                {
                    data = LoadInstanceData(XDocument.Parse(instance.InstanceData));
                    context.LoadedInstance(InstanceState.Initialized, data, null, null, null);
                }
                catch (Exception exception)
                {
                    
                  throw new PersistenceException(exception.Message);
         
                }
            }
            return new CompletedAsyncResult<bool>(true, callback, state);
        }
        protected override bool EndTryCommand(IAsyncResult result)
        {
            return CompletedAsyncResult<bool>.End(result);
        }
        #region Class Methods
        //Saves the persistence data to an xml file.
        void Save(IDictionary<XName, InstanceValue> instanceData)
        {     
            var instance = _workflowInstanceService.GetById(ownerInstanceID);
            if (instance == null)            
                instance = _workflowInstanceService.CreateWorkflowInstance(ownerInstanceID);
            var doc = XDocument.Parse(instance.InstanceData);
            XElement instanceValues = doc.Element("InstanceValues");

            foreach (KeyValuePair<XName, InstanceValue> valPair in instanceData)
            {
                XElement newInstance = new XElement("InstanceValue"); //doc.CreateElement("InstanceValue");

                XElement newKey = SerializeObject("key", valPair.Key);
                newInstance.Add(newKey);

                XElement newValue = SerializeObject("value", valPair.Value.Value);
                newInstance.Add(newValue);

                instanceValues.Add(newInstance);
                instance.InstanceData = instanceValues.ToString();
            }
            _workflowInstanceService.Save(instance);
        }

        //Reads data from xml file and creates a dictionary based off of that.
        IDictionary<XName, InstanceValue> LoadInstanceData(XDocument doc)
        {
            IDictionary<XName, InstanceValue> data = new Dictionary<XName, InstanceValue>();

            NetDataContractSerializer s = new NetDataContractSerializer();
            var instances = doc.Descendants("InstanceValue");
            foreach (XElement instanceElement in instances)
            {
                XElement keyElement = instanceElement.Element("key"); //(XmlElement)instanceElement.SelectSingleNode("descendant::key");
                XName key = (XName)DeserializeObject(s, keyElement);

                XElement valueElement = instanceElement.Element("value");//instanceElement.SelectSingleNode("descendant::value");
                object value = DeserializeObject(s, valueElement);
                InstanceValue instVal = new InstanceValue(value);
                data.Add(key, instVal);
            }

            return data;
        }
        object DeserializeObject(NetDataContractSerializer serializer, XElement element)
        {
            object deserializedObject = null;

            MemoryStream stm = new MemoryStream();
            //var x = new StringReader(element.ToString());
            var sb = new StringBuilder();
            var wtr = XmlWriter.Create(sb);
            element.WriteTo(wtr);
            wtr.Flush();
            stm.Position = 0;

            deserializedObject = serializer.Deserialize(stm);

            return deserializedObject;
        }
        XElement SerializeObject(string elementName, object o)
        {
            XElement result = null;
            NetDataContractSerializer s = new NetDataContractSerializer();
            result = new XElement(elementName);//doc.CreateElement(elementName);
            
            MemoryStream stm = new MemoryStream();

            s.Serialize(stm, o);
            stm.Position = 0;
            StreamReader rdr = new StreamReader(stm);
            result.Add(XElement.Parse(rdr.ReadToEnd()));
            return result;
        }
        #endregion
    }
}
