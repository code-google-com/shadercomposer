using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Interface.Designer.Variables;

namespace ShaderComposer.Libraries
{
    public interface INode
    {
        string GetName();

        Guid GetIdentifier();

        bool IsOutputNode();

        List<Variable> GetVariables();
        
        string GetSource(Dictionary<Variable, string> variableIdentifiers);
    }
}
