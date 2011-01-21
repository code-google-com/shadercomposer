using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Interface.Designer.Variables;

namespace TerrainLibrary.Nodes
{
    class SnowRock : INode
    {
        public string GetName()
        {
            return "SnowRock Texture";
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
        private Variable varUV;
        private Variable varColor;

        public List<Variable> GetVariables()
        {
            List<Variable> variables = new List<Variable>();

            varUV = new Variable("uv");
            varUV.Type = Variable.VariableType.Input;
            varUV.Text = "UV";
            variables.Add(varUV);

            varColor = new Variable("color");
            varColor.Type = Variable.VariableType.Output;
            varColor.Text = "Color";
            variables.Add(varColor);

            return variables;
        }

        // Source code
        public string GetSource(Dictionary<Variable, string> i)
        {
            string result = "\toutput." + i[varColor] + " = SampleDetail(input." + i[varUV] + ", snowRockTexture);\n";

            return result;
        }
    }
}
