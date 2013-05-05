using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Extensions
{
    public static class MathExtensions
    {
        public static double[] LineIntersection(this double[] series, double[] series1)
        {
            double[] result = null;
            var A1 = 1;
            var B1 = series[0] - series[1];
            var C1 = A1 * series[0] + B1 * 0;

            var A2 = 1;
            var B2 = series1[0] - series1[1];
            var C2 = A2 * series1[0] + B2 * 0;

            var det = A1 * B2 - A2 * B1;
            if (det != 0)
            {
                double x = (B2 * C1 - B1 * C2) / det;
                double y = (A1 * C2 - A2 * C1) / det;
                result = new double[2] { x, y };
            }
            return result;
        }
        public static int Cross(this double[] series, double[] series1, int lookback = 0)
        {
            /*0=no cross,1=cross under,2=cross over*/
            int result = 0;
            var counter = 0;
            while (lookback < (counter + 1))
            {
                double[] seriesPeriod = { series[counter], series[counter + 1] };
                double[] series1Period = { series1[counter], series1[counter + 1] };
                var li = seriesPeriod.LineIntersection(series1Period);
                if (li != null)
                {
                    if (seriesPeriod[0] < series1Period[0] && seriesPeriod[1] > series1Period[1])
                        result = 2;
                    else if (seriesPeriod[0] > series1Period[0] && seriesPeriod[1] < series1Period[1])
                        result = 1;
                    if (result > 0)
                        break;
                }
                counter++;
            }
            return result;
        }
        public static bool CrossOver(this double[] series, double[] series1, int lookback = 0)
        {
            return series.Cross(series1) == 2;
        }
        public static bool CrossUnder(this double[] series, double[] series1, int lookback = 0)
        {
            return series.Cross(series1) == 1;
        }
        public static bool Rising(this double[] series,int period=0)
        {
            return series.LinearRegressionSlope(period).First() > 0;
        }
        public static bool Falling(this double[] series, int period=0)
        {
            return series.LinearRegressionSlope(period).First() < 0;
        }
    }
}
