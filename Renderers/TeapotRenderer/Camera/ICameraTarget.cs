using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;

namespace TeapotRenderer.Camera
{
    interface ICameraTarget
    {
        Vector3 Position { get; }
    }
}
