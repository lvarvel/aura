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
        public static Color4 AlphaSpike(ColorRange range, float p)
        {
            Color4 newColor = range.First;
            newColor = (range.Second - range.First) * p;
            newColor.A = (float)(newColor.A % 0.5);
            return newColor;
        }
    }
}
