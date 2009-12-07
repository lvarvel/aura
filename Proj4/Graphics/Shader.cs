using System;
using System.Collections.Generic;

namespace Aura.Graphics
{
    public class Shader
    {
        protected int cgHandle;
        internal Shader(int _handle)
        {
            cgHandle = _handle;
        }
    }
}
