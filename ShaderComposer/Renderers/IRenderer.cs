using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

namespace ShaderComposer.Renderers
{
    public interface IRenderer
    {
        string GetName();

        IRenderer Create();

        ImageSource Initialize();

        void SetSourceCode(string sourceCode);
    }
}
