using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    class Dot : INode
    {
        public string GetName()
        {
            return "Dot product";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae339";

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
        private Variable varA;
        private Variable varB;
        private Variable varADotB;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varA = new Variable();
            varA.Type = Variable.VariableType.Input;
            varA.Text = "A";
            variables.Add(varA);

            varB = new Variable();
            varB.Type = Variable.VariableType.Input;
            varB.Text = "B";
            variables.Add(varB);

            varADotB = new Variable();
            varADotB.Type = Variable.VariableType.Output;
            varADotB.Text = "A dot B";
            variables.Add(varADotB);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varADotB] + " = dot(input." + i[varA] + ", input." + i[varB] + ");\n";

            return result;
        }
    }
}
