using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Libraries;

namespace LogicLibrary.Nodes
{
    class Select : INode
    {
        public string GetName()
        {
            return "Select";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1adaaae330";

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
        private Variable varSelect;
        private Variable varA;
        private Variable varB;
        private Variable varAOrB;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varSelect = new Variable("Value");
            varSelect.Type = Variable.VariableType.Input;
            varSelect.Text = "Value";
            variables.Add(varSelect);

            varA = new Variable("A");
            varA.Type = Variable.VariableType.Input;
            varA.Text = "A";
            variables.Add(varA);

            varB = new Variable("B");
            varB.Type = Variable.VariableType.Input;
            varB.Text = "B";
            variables.Add(varB);

            varAOrB = new Variable("AOrB");
            varAOrB.Type = Variable.VariableType.Output;
            varAOrB.Text = "A | B";
            variables.Add(varAOrB);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\tif (input." + i[varSelect] + ".x > 0) output." + i[varAOrB] + " = input." + i[varA] + "; else output." + i[varAOrB] + " = input." + i[varB] + ";\n";

            return result;
        }
    }
}
