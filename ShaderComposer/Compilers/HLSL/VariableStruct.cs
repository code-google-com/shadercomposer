using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Interface.Designer;
using ShaderComposer.Interface.Designer.Variables;

namespace ShaderComposer.Compilers
{
    class VariableStruct
    {
        // Parent node
        private Node node;

        public Dictionary<Variable, string> dictonaryVariableIdentifiers = new Dictionary<Variable, string>();

        public VariableStruct(Node node, Variable.VariableType variableType)
        {
            this.node = node;

            // Create the identifier
            Identifier = "s" + getUniqueID();

            // Create the hlsl description
            HLSLDescription = "struct " + Identifier + " {\n";

            foreach (Variable variable in node.Variables)
            {
                if (variable.Type == variableType)
                {
                    string variableIdentifier = "v" + getUniqueID();

                    dictonaryVariableIdentifiers[variable] = variableIdentifier;

                    HLSLDescription += "\t" + determineTypeIdentifier(variable) + " " + variableIdentifier + ";\n";
                }
            }

            HLSLDescription += "};\n\n"; 
        }

        // Provides unique id's
        private int getUniqueID()
        {
            return uniqueID++;
        }

        private static int uniqueID = 0;

        // Struct identifier
        public string Identifier { get; private set; }

        // HLSL description
        public string HLSLDescription { get; private set; }

        // Determine the variable type
        private string determineTypeIdentifier(Variable variable)
        {
            if (variable.Node.inode.IsOutputNode() && variable.InputType == Variable.InputTypes.Link && variable.GetLinks().Count == 1)
            {
                return determineTypeIdentifier(variable.GetLinks()[0].OutputVariable);
            }

            if (variable.IsFloat4())
                return "float4";
            else if (variable.IsFloat3())
                return "float3";
            else if (variable.IsFloat2())
                return "float2";
            else if (variable.IsFloat1())
                return "float";
            else
                return "float4";
        }
    }
}
