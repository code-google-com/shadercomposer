using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    public class Multiply : INode
    {
        public string GetName()
        {
            return "Multiply";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae331";

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
        private Variable varATimesB;

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

            varATimesB = new Variable();
            varATimesB.Type = Variable.VariableType.Output;
            varATimesB.Text = "A * B";
            variables.Add(varATimesB);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varATimesB] + " = input." + i[varA] + " * input." + i[varB] + ";\n";

            return result;
        }
    }
}
