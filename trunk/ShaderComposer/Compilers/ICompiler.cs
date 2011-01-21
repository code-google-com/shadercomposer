using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaderComposer.Compilers
{
    public interface ICompiler
    {
        void Compile();

        string SourceCode { get; }
    }
}
