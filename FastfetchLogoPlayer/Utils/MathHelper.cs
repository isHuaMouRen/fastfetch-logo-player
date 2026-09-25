using System;
using System.Collections.Generic;
using System.Text;

namespace FastfetchLogoPlayer.Utils
{
    public static class MathHelper
    {
        public static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            return ((value - min) % range + range) % range + min;
        }
    }
}
