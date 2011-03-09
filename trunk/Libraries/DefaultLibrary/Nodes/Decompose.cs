using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    class Decompose : INode
    {
        public string GetName()
        {
            return "Decompose";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ad39ae330";

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
        private Variable varInput;
        private Variable varX;
        private Variable varY;
        private Variable varZ;
        private Variable varW;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varInput = new Variable("Input");
            varInput.Type = Variable.VariableType.Input;
            varInput.Text = "Input";
            variables.Add(varInput);

            varX = new Variable("X");
            varX.Type = Variable.VariableType.Output;
            varX.Text = "X";
            variables.Add(varX);

            varY = new Variable("Y");
            varY.Type = Variable.VariableType.Output;
            varY.Text = "Y";
            variables.Add(varY);

            varZ = new Variable("Z");
            varZ.Type = Variable.VariableType.Output;
            varZ.Text = "Z";
            variables.Add(varZ);

            varW = new Variable("W");
            varW.Type = Variable.VariableType.Output;
            varW.Text = "W";
            variables.Add(varW);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varX] + " = input." + i[varInput] + ".x;\n";
            result += "\toutput." + i[varY] + " = input." + i[varInput] + ".y;\n";
            result += "\toutput." + i[varZ] + " = input." + i[varInput] + ".z;\n";
            result += "\toutput." + i[varW] + " = input." + i[varInput] + ".w;\n";

            return result;
        }
    }
}
