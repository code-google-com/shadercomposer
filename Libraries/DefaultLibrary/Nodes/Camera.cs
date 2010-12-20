using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Libraries;

namespace DefaultLibrary.Nodes
{
    class Camera : INode
    {
        public string GetName()
        {
            return "Camera";
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

            varPosition = new Variable();
            varPosition.Type = Variable.VariableType.Output;
            varPosition.Text = "Camera";
            variables.Add(varPosition);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varPosition] + " = float4(varyings.camera, 1);\n";

            return result;
        }
    }
}
