using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoCutter.HelperClasses
{
    class TimeHelper
    {
        public static string SimplifyTimestamp(string inputTimestamp)
        {
            var splitTimestamp = inputTimestamp.Split(':');
            List<double> timestampList = new List<double>();

            foreach (var timeUnit in splitTimestamp)
            {
                timestampList.Add(Convert.ToDouble(timeUnit));
            }

            if (timestampList[0] == 0 && timestampList[1] == 0)
            {
                return splitTimestamp[2];
            }
            else if (timestampList[0] == 0)
            {
                return splitTimestamp[1] + ":" + splitTimestamp[2];
            }
            else
            {
                return inputTimestamp;
            }
        }
    }
}
