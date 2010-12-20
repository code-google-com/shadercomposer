using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    public class Output : INode
    {
        public string GetName()
        {
            return "Output";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae332";

        public Guid GetIdentifier()
        {
            return new Guid(IDENTIFIER);
        }

        // Is output node
        public bool IsOutputNode()
        {
            return true;
        }

        // Variable definitions
        private Variable varFinalOutput;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varFinalOutput = new Variable();
            varFinalOutput.Type = Variable.VariableType.Input;
            varFinalOutput.Text = "Final output";
            variables.Add(varFinalOutput);
            
            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> variableIdentifiers)
        {
            return "\toutput = input;\n";
        }
    }
}
