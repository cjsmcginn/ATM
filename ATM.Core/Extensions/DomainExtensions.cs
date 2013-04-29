using ATM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Extensions
{
    public static class DomainExtensions
    {
        public static string GetQuoteId(this Quote quote)
        {
            return String.Format("{0}{1}", System.Text.RegularExpressions.Regex.Replace(quote.Instrument, "\\W", ""), quote.DateTime.ToFileTimeUtc());
        }
    }
}
