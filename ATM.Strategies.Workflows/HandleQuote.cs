using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using ATM.Core.Domain;

namespace ATM.Strategies.Workflows
{

    public sealed class HandleQuote : NativeActivity<Double[]>
    {
        // Define an activity input argument of type string
        public InArgument<double> Quote { get; set; }
        public InArgument<double[]> Quotes { get; set; }
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            var quote = context.GetValue(this.Quote);
            var quotes = context.GetValue(this.Quotes);
            quotes.ToList().Add(quote);
            quotes = quotes.ToArray();
            context.CreateBookmark("NextQuote", new BookmarkCallback(OnReadComplete));
        }
        void OnReadComplete(NativeActivityContext context, Bookmark bookmark, object state)
        {
            double input = Convert.ToDouble(state);
            var quotes = context.GetValue<double[]>(this.Quotes);
            var added = quotes.ToList();
            added.Add(input);
            double[] result = added.ToArray();
            context.SetValue(this.Result,result );
        }
        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}
