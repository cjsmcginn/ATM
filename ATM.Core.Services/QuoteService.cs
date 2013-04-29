using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Core.Extensions;
namespace ATM.Core.Services
{
    public class QuoteService:IQuoteService
    {
        #region fields
        private readonly IDataService _dataService;

        #endregion

        #region construtor
        public QuoteService(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion

        public void SaveQuote(Quote quote)
        {
            quote.Id = quote.GetQuoteId();
            _dataService.Save<Quote>(quote);
        }

        public Quote GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
