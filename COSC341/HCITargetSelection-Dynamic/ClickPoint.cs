using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCITargetSelection
{
    public enum ClickPointType
    {
        Miss,
        White,
        ToolbarButton
    }
    public class ClickPoint
    {
        public ClickPointType Type { get; set; }
        public String Button { get; set; }
        public double MilliSeconds { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public ClickPoint(Stopwatch timer, double x, double y, ClickPointType type, String button = null)
        {
            Type = type;
            MilliSeconds = timer.Elapsed.TotalMilliseconds;
            X = x;
            Y = y;
            Button = button;
        }
    }
}
