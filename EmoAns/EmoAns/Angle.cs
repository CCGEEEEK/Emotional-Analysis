using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoAns
{
    class Angle
    {
        public static double Angle2Rad(double angle)
        {
            return angle * Math.PI / 180;
        }
        public static double Rad2Angle(double rad)
        {
            return 180 * rad / Math.PI;
        }
    }
}
