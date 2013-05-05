using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Extensions
{
    public static class TAExtensions
    {
        delegate TicTacTec.TA.Library.Core.RetCode TA(int startIdx, int endIdx, double[] inReal, int optInTimePeriod, out int outBegIdx, out int outNbElement, double[] outReal);
        static double[] RunTA(this double[] series,TA handler,int period=0)
        {
            int startIdx = 0;
            int endIdx = series.Length - 1;
            double[] inReal = new double[series.Length];
            double[] outReal = new double[series.Length];
            series.CopyTo(inReal, 0);
            if (period == 0)
                period = series.Length;
            int optInTimePeriod = period;
            int outBegIdx;
            int outNbElement;
            handler(startIdx, endIdx, inReal, optInTimePeriod, out outBegIdx, out outNbElement, outReal);
            return outReal;
        }

        public static double[] LinearRegressionSlope(this double[] series, int period=0)
        {
            return series.RunTA(TicTacTec.TA.Library.Core.LinearRegSlope,period);
        }
        public static double[] SMA(this double[] series, int period)
        {
            return series.RunTA(TicTacTec.TA.Library.Core.Sma, period);
        }
    }
}
