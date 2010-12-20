using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace DefaultLibrary.Nodes
{
    class Fog : INode
    {
        public string GetName()
        {
            return "Fog";
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
        private Variable varDensity;
        private Variable varFogColor;
        private Variable varInputColor;
        private Variable varFinal;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varDensity = new Variable();
            varDensity.Type = Variable.VariableType.Input;
            varDensity.Text = "Density";
            variables.Add(varDensity);

            varFogColor = new Variable();
            varFogColor.Type = Variable.VariableType.Input;
            varFogColor.Text = "Color";
            variables.Add(varFogColor);

            varInputColor = new Variable();
            varInputColor.Type = Variable.VariableType.Input;
            varInputColor.Text = "Input";
            variables.Add(varInputColor);

            varFinal = new Variable();
            varFinal.Type = Variable.VariableType.Output;
            varFinal.Text = "Output";
            variables.Add(varFinal);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\tfloat d = length(varyings.position - varyings.camera);\n";
            result += "\tfloat fogFactor = 1 / exp(d * input." + i[varDensity] + ");\n";
            result += "\toutput." + i[varFinal] + " = input." + i[varInputColor] + " * (fogFactor) + input." + i[varFogColor] + " * (1.0 - fogFactor);\n";

            return result;
        }
    }
}
