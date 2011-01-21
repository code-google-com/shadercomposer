using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Libraries;

namespace DefaultLibrary.Nodes
{
    class Normal : INode
    {
        public string GetName()
        {
            return "Normal";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae335";

        public Guid GetIdentifier()
        {
            return new Guid(IDENTIFIER);
        }

        // Is output node
        public bool IsOutputNode()
        {
            return false;
        }

        // Variable definitions
        private Variable varPosition;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varPosition = new Variable("Normal");
            varPosition.Type = Variable.VariableType.Output;
            varPosition.Text = "Normal";
            variables.Add(varPosition);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varPosition] + " = float4(varyings.normal, 1);\n";

            return result;
        }
    }
}
