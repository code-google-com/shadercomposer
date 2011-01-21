using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

namespace ShaderComposer.Renderers
{
    public delegate void SceneUpdatedHandler(object sender);

    public interface IRenderer
    {
        string GetName();

        IRenderer Create();

        ImageSource Initialize();

        void Destroy();

        void SetSourceCode(string sourceCode);

        string GetValueAt(double x, double y);

        // Scene updated event
        event SceneUpdatedHandler SceneUpdated;
    }
}
