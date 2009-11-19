using System;
using System.Collections.Generic;

namespace Aura
{
    public interface IDrawable
    {
        void Draw();
    }

    public interface IVisualization
    {
        void Draw(Vector3 position);
    }
}
