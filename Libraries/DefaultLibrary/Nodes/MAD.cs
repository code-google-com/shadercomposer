using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Libraries;

namespace DefaultLibrary.Nodes
{
    class MAD : INode
    {
        public string GetName()
        {
            return "Multiply Add";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae330";

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
        private Variable varC;
        private Variable varAPlusBTimesC;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varA = new Variable("A");
            varA.Type = Variable.VariableType.Input;
            varA.Text = "A";
            variables.Add(varA);

            varB = new Variable("B");
            varB.Type = Variable.VariableType.Input;
            varB.Text = "B";
            variables.Add(varB);

            varC = new Variable("C");
            varC.Type = Variable.VariableType.Input;
            varC.Text = "C";
            variables.Add(varC);

            varAPlusBTimesC = new Variable("Result");
            varAPlusBTimesC.Type = Variable.VariableType.Output;
            varAPlusBTimesC.Text = "(A + B) * C";
            variables.Add(varAPlusBTimesC);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varAPlusBTimesC] + " = (input." + i[varA] + " + input." + i[varB] + ") * input." + i[varC] + ";\n";

            return result;
        }
    }
}
