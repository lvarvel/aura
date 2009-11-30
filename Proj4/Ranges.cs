using System;
using System.Collections.Generic;

namespace Aura
{
    public class Range
    {
        public float First{get; set;}
        public float Second {get; set;}

        protected Range() { }
        public Range(float both)
        {
            First = both;
            Second = both;
        }
        public Range(float first, float second)
        {
            First = first;
            Second = second;
        }

        public float Interpolate(float factor)
        {
            return First + Second * factor;
        }
    }

    public class ColorRange
    {
        public Color4 First;
        public Color4 Second;

        protected ColorRange() { }
        public ColorRange(Color4 both)
        {
            First = (Color4)both.Clone();
            Second = (Color4)both.Clone();
        }
        public ColorRange(Color4 first, Color4 second)
        {
            First = (Color4)first.Clone();
            Second = (Color4)second.Clone();
        }

    }
}
