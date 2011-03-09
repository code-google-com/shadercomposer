using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    public class Add : INode
    {
        public string GetName()
        {
            return "Add";
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
        private Variable varAPlusB;

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

            varAPlusB = new Variable("AplusB");
            varAPlusB.Type = Variable.VariableType.Output;
            varAPlusB.Text = "A + B";
            variables.Add(varAPlusB);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varAPlusB] + " = input." + i[varA] + " + input." + i[varB] + ";\n";

            return result;
        }
    }
}
