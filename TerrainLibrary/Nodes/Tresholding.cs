using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace TerrainLibrary.Nodes
{
    class Tresholding : INode
    {
        public string GetName()
        {
            return "Tresholding";
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
        private Variable varInput;
        private Variable varTreshold;
        private Variable varBlend;
        private Variable varResult;

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

            varInput = new Variable();
            varInput.Type = Variable.VariableType.Input;
            varInput.Text = "Input";
            variables.Add(varInput);

            varTreshold = new Variable();
            varTreshold.Type = Variable.VariableType.Input;
            varTreshold.Text = "Treshold";
            variables.Add(varTreshold);

            varBlend = new Variable();
            varBlend.Type = Variable.VariableType.Input;
            varBlend.Text = "Blend";
            variables.Add(varBlend);

            varResult = new Variable();
            varResult.Type = Variable.VariableType.Output;
            varResult.Text = "Result";
            variables.Add(varResult);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "";
            result += "\tif (input." + i[varInput] + " < input." + i[varTreshold] + " - input." + i[varBlend] + ") \n";
            result += "\t{\n";
            result += "\t\toutput." + i[varResult] + " = input." + i[varA] + ";\n";
            result += "\t}\n";
            result += "\telse if (input." + i[varInput] + " > input." + i[varTreshold] + " + input." + i[varBlend] + ") \n";
            result += "\t{\n";
            result += "\t\toutput." + i[varResult] + " = input." + i[varB] + ";\n";
            result += "\t}\n";
            result += "\telse\n";
            result += "\t{\n";
            result += "\t\tfloat f = (input." + i[varInput] + " - input." + i[varTreshold] + " + input." + i[varBlend] + ") / (2 * input." + i[varBlend] + ")\n;";
            result += "\t\toutput." + i[varResult] + " = lerp(input." + i[varA] + ", input." + i[varB] + ", f);\n";
            result += "\t}\n";

            return result;
        }
    }
}
