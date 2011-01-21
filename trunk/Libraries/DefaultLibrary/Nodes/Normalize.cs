using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    class Normalize : INode
    {
        public string GetName()
        {
            return "Normalize";
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
        private Variable varInput;
        private Variable varOutput;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varInput = new Variable("unnormal");
            varInput.Type = Variable.VariableType.Input;
            varInput.Text = "Unnormalized value";
            variables.Add(varInput);

            varOutput = new Variable("normal");
            varOutput.Type = Variable.VariableType.Output;
            varOutput.Text = "Normalized value";
            variables.Add(varOutput);
            
            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varOutput] + " = normalize(input." + i[varInput] + ");\n";

            return result;
        }
    }
}
