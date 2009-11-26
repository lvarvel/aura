using System;
using System.Collections.Generic;

namespace Aura
{
    public static class FunctionAssets
    {
        public static float LinearInterpolation(Range range, float p)
        {
            return range.Interpolate(p);
        }
        public static Color4 LinearInterpolation(ColorRange range, float p)
        {
            return range.First + (range.Second * p);
        }
    }
}
