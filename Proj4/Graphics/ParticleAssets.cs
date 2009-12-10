using System;
using System.Collections.Generic;
using Aura.Core;

namespace Aura.Graphics
{
    public static class FunctionAssets
    {
        public static float LinearInterpolation(Range range, float p)
        {
            return range.Interpolate(p);
        }
        public static Color4 LinearInterpolation(ColorRange range, float p)
        {
            return range.First + ((range.Second - range.First)* p);
        }
    }
}
