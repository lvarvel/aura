using System;
using System.Collections.Generic;

namespace Aura.Core
{
    public static class Util
    {
        /// <summary>
        /// Randomly returns either 1 or -1
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int NextSign(this Random r)
        {
            return (r.NextDouble() > .49) ? 1 : -1;
        }
        public static Random r = new Random();



        /// <summary>
        /// Returns a random, normalized vector3
        /// Remember to return it to the pool when you're done with it
        /// </summary>
        /// <param name="clamp"></param>
        /// <returns></returns>
        public static Vector3 GetRandomEmissionNormal(DirectionalClamp clamp)
        {
            //Note: code is very, very hacky.  There was't a whole lot I could do to fix this
            Vector3 result = Vector3Pool.Instance.Get();
            #region Check trivial case
            if (clamp.ClampedX && clamp.ClampedY && clamp.ClampedZ)
            {
                result.AssignValues(0, 0, 0);
                return result;
            }
            #endregion
            float l = 0;
            do
            {
                result.X = (float)r.NextDouble() * r.NextSign();
                result.Y = (float)r.NextDouble() * r.NextSign();
                result.Z = (float)r.NextDouble() * r.NextSign();
                l = result.length();
            } while (l == 0); //Note: There is a 1/2^96 % chance that the length will be zero, which will cause divide by zero errors 

            result.X /= l;
            result.Y /= l;
            result.Z /= l;

            if (clamp.NoClamp) return result;

            #region XClamping
            if (clamp.ClampedX)
            {
                result.X = 0;
            }
            else
            {
                if (clamp.ClampedPositiveX)
                {
                    result.X *= (result.X > 0) ? -1 : 1;
                }
                else if (clamp.ClampedNegativeX)
                {
                    result.X *= (result.X < 0) ? -1 : 1;
                }
            }
            #endregion

            #region YClamping
            if (clamp.ClampedY)
            {
                result.Y = 0;
            }
            else
            {
                if (clamp.ClampedPositiveY)
                {
                    result.Y *= (result.Y > 0) ? -1 : 1;
                }
                else if (clamp.ClampedNegativeY)
                {
                    result.Y *= (result.Y < 0) ? -1 : 1;
                }
            }
            #endregion

            #region ZClamping
            if (clamp.ClampedZ)
            {
                result.Z = 0;
            }
            else
            {
                if (clamp.ClampedPositiveZ)
                {
                    result.Z *= (result.Z > 0) ? -1 : 1;
                }
                else if (clamp.ClampedNegativeZ)
                {
                    result.Z *= (result.Z < 0) ? -1 : 1;
                }
            }
            #endregion

            result.normalize();
            return result;
        }
    }
}
