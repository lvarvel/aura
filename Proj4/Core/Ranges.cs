using System;
using System.Collections.Generic;

namespace Aura.Core
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
            return First + ((Second - First) * factor);
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

    [UnitTest(2)]
    internal class ClampTest
    {
        [UnitTestMethod]
        public static void TestClamp(TestResults results)
        {
            DirectionalClamp d = new DirectionalClamp();
            results.ReportMessage("Begining test of Directional Clamp bitmask.");
            if (d.ClampedX || d.ClampedY || d.ClampedZ)
            {
                results.ReportError("Empty bitmask is not zero");
            }
            else
                results.ReportMessage("Empty bitmask is zero");

            results.ReportMessage("Starting test 1");
            d.ClampedPositiveX = true;
            if (d.ClampedPositiveX)
                results.ReportMessage("Test 1 success. Clamp is reflexive.");
            else
                results.ReportError("Test 1 failure. Clamp is not reflexive.");

            d.NoClamp = true;
            if (d.ClampedX || d.ClampedY || d.ClampedZ)
            {
                results.ReportError("Empty bitmask is not zero");
            }
            results.ReportMessage("Starting test 2");
            d.ClampedZ = true;
            if (d.ClampedNegativeZ && d.ClampedPositiveZ)
                results.ReportMessage("Test 2 success. Dimentional/Polar\n options interoperate correctly.");
            else
                results.ReportError("Test 2 failure. Dimentional/Polar options do not interoperate");
        }
    }

    /// <summary>
    /// Provides simple functionality for clamping certain directions (ie, for movement)
    /// Uses an underlying bitmask to minimize the ammount of memory consumed
    /// </summary>
    public struct DirectionalClamp
    {
        #region Declarations
        [Flags]
        private enum DimClamp : byte { None = 0, ClampX = 1, ClampNX = 2, ClampY = 4, ClampNY = 8, ClampZ = 16, ClampNZ = 32, DimX = 3, DimY = 12, DimZ = 48 }
        private DimClamp clamp;
        #endregion

        #region Constructors
        public DirectionalClamp(ClampState X = ClampState.None, ClampState Y = ClampState.None, ClampState Z = ClampState.None)
        {
            clamp = 0;
            switch (X)
            {
                case ClampState.None:
                    break;
                case ClampState.Negative:
                    clamp |= DimClamp.ClampNX;
                    break;
                case ClampState.Positive:
                    clamp |= DimClamp.ClampX;
                    break;
                case ClampState.Zero:
                    clamp |= DimClamp.ClampX | DimClamp.ClampNX;
                    break;
            }
            switch (Y)
            {
                case ClampState.None:
                    break;
                case ClampState.Negative:
                    clamp |= DimClamp.ClampNY;
                    break;
                case ClampState.Positive:
                    clamp |= DimClamp.ClampY;
                    break;
                case ClampState.Zero:
                    clamp |= DimClamp.ClampY | DimClamp.ClampNY;
                    break;
            }
            switch (Z)
            {
                case ClampState.None:
                    break;
                case ClampState.Negative:
                    clamp |= DimClamp.ClampNY;
                    break;
                case ClampState.Positive:
                    clamp |= DimClamp.ClampY;
                    break;
                case ClampState.Zero:
                    clamp |= DimClamp.ClampY | DimClamp.ClampNY;
                    break;
            }
        }
        #endregion

        #region Accessors
        public bool NoClamp
        {
            get { return clamp == 0; }
            set { if (value) clamp = 0; }
        }
        public bool ClampedPositiveX
        {
            get
            {
                return (clamp & DimClamp.ClampX) == DimClamp.ClampX;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampX;
                }
                else
                {
                    clamp &= ~DimClamp.ClampX;
                }
            }
        }
        public bool ClampedPositiveY
        {
            get
            {
                return (clamp & DimClamp.ClampY) == DimClamp.ClampY;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampY;
                }
                else
                {
                    clamp &= ~DimClamp.ClampY;
                }
            }
        }
        public bool ClampedPositiveZ
        {
            get
            {
                return (clamp & DimClamp.ClampZ) == DimClamp.ClampZ;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampZ;
                }
                else
                {
                    clamp &= ~DimClamp.ClampZ;
                }
            }
        }
        public bool ClampedNegativeX
        {
            get
            {
                return (clamp & DimClamp.ClampNX) == DimClamp.ClampNX;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampNX;
                }
                else
                {
                    clamp &= ~DimClamp.ClampNX;
                }
            }
        }
        public bool ClampedNegativeY
        {
            get
            {
                return (clamp & DimClamp.ClampNY) == DimClamp.ClampNY;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampNY;
                }
                else
                {
                    clamp &= ~DimClamp.ClampNY;
                }
            }
        }
        public bool ClampedNegativeZ
        {
            get
            {
                return (clamp & DimClamp.ClampNZ) == DimClamp.ClampNZ;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.ClampNZ;
                }
                else
                {
                    clamp &= ~DimClamp.ClampNZ;
                }
            }
        }

        public bool ClampedX
        {
            get
            {
                return (clamp & DimClamp.DimX) == DimClamp.DimX;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.DimX;
                }
                else
                {
                    clamp &= ~DimClamp.DimX;
                }
            }
        }
        public bool ClampedY
        {
            get
            {
                return (clamp & DimClamp.DimY) == DimClamp.DimY;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.DimY;
                }
                else
                {
                    clamp &= ~DimClamp.DimY;
                }
            }
        }
        public bool ClampedZ
        {
            get
            {
                return (clamp & DimClamp.DimZ) == DimClamp.DimZ;
            }
            set
            {
                if (value)
                {
                    clamp |= DimClamp.DimZ;
                }
                else
                {
                    clamp &= ~DimClamp.DimZ;
                }
            }
        }

        public static DirectionalClamp ZeroClamp
        {
            get { return new DirectionalClamp(); }
        }
        public static DirectionalClamp XClamped
        {
            get { return new DirectionalClamp(ClampState.Zero); }
        }
        public static DirectionalClamp YClamped
        {
            get { return new DirectionalClamp(ClampState.None, ClampState.Zero); }
        }
        public static DirectionalClamp ZClamped
        {
            get { return new DirectionalClamp(ClampState.None, ClampState.None, ClampState.Zero); }
        }
        #endregion
    }

    public enum ClampState : byte { None, Negative, Zero, Positive }
}
