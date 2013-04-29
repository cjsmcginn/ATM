using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Strategies.Workflows
{
    public static class TAExtensions
    {
        public static double[] SMA(this double[] input, int period)
        {
            int startIdx=0;
            int endIdx;
            double[] inReal = new double[input.Length];
            double[] outReal = new double[input.Length];
            input.CopyTo(inReal, 0);
            int optInTimePeriod = period;
            int outBegIdx;
            int outNbElement;
            TicTacTec.TA.Library.Core.Sma(startIdx, inReal.Length-1, inReal, optInTimePeriod, out outBegIdx, out outNbElement, outReal);
            return outReal;
        }
    }
}
