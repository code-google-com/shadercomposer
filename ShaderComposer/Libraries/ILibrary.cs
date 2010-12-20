using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaderComposer.Libraries
{
    public interface ILibrary
    {
        string GetName();

        Guid GetIdentifier();

        List<Type> GetNodeTypes();
    }
}
