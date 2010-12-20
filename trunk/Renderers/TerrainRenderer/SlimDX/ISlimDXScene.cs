using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace TerrainRenderer.SlimDX2
{
    interface ISlimDXScene
    {
        Texture2D SharedTexture
        {
            get;
        }

        void Dispose();

        void Render(int p);
    }
}
