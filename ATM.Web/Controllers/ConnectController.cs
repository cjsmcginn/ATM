using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATM.Core.Domain;
using ATM.Core.Services;
using ATM.Data.Raven;
namespace ATM.Web.Controllers
{
    public class ConnectController : ApiController
    {
        IDataService _dataService;
        IQuoteService _quoteService;

        public ConnectController()
        {
            _dataService = new RavenDataService();
            _quoteService = new QuoteService(_dataService);
        }

        // GET api/connect
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/connect/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/connect
        public string Post()
        {
            var item = Request.GetQueryNameValuePairs().ToDictionary(x=>x.Key,x=>x.Value);
            var q = Quote.FromXmlString(item["value"]);
            _quoteService.SaveQuote(q);
            var result = "Signal";
            return result;
        }

        // PUT api/connect/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/connect/5
        public void Delete(int id)
        {
        }
    }
}
