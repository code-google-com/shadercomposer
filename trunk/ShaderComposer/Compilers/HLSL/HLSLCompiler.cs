using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.FileManagement;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Interface.Designer;
using System.Windows.Controls;

namespace ShaderComposer.Compilers.HLSL
{
    public class HLSLCompiler
    {
        private FileState fileState;

        public HLSLCompiler(FileState fileState)
        {
            this.fileState = fileState;
        }

        // Latest compiled source code
        public string SourceCode { get; private set; }

        //
        private Dictionary<Node, VariableStruct> dictonaryInputStruct;
        private Dictionary<Node, VariableStruct> dictonaryOutputStruct;

        //
        private Dictionary<Node, string> dictionaryExecuteIdentifiers;

        //
        private string varyingIdentifier;

        //
        private Node outputNode;

        public void Compile()
        {
            SourceCode = "// Auto generated shader (HLSL) \n\n";
            
            // Create dictionaries
            dictonaryInputStruct = new Dictionary<Node, VariableStruct>();
            dictonaryOutputStruct = new Dictionary<Node, VariableStruct>();

            // Varyings struct definition
            SourceCode += "// Varying struct definition \n\n";

            varyingIdentifier = "IOVaryings";

            SourceCode += "struct " + varyingIdentifier + " {\n";
            SourceCode += "\t float4 position;\n";
            SourceCode += "\t float4 normal;\n";
            SourceCode += "\t float4 camera;\n";
            SourceCode += "};\n\n";

            // Add all input and output structs definitions
            SourceCode += "// Input and output struct definitions \n\n";

            foreach (Node node in fileState.Nodes)
            {
                VariableStruct inputStruct = new VariableStruct(node, Variable.VariableType.Input);
                dictonaryInputStruct[node] = inputStruct;

                SourceCode += inputStruct.HLSLDescription;

                VariableStruct outputStruct = new VariableStruct(node, Variable.VariableType.Output);
                dictonaryOutputStruct[node] = outputStruct;

                SourceCode += outputStruct.HLSLDescription;
            }

            // Find the output node
            outputNode = null;

            foreach (Node node in fileState.Nodes)
            {
                if (node.inode.IsOutputNode()) {
                    outputNode = node;
                    break;
                }
            }

            dictionaryGetIdentifiers = new Dictionary<Node, string>();

            if (outputNode == null)
                return;

            // Add all execute functions
            SourceCode += "// Execution functions \n\n";

            dictionaryExecuteIdentifiers = new Dictionary<Node, string>();
                 
            foreach (Node node in fileState.Nodes)
            {
                VariableStruct inputStruct = dictonaryInputStruct[node];
                VariableStruct outputStruct = dictonaryOutputStruct[node];

                string outputType = outputStruct.Identifier;

                if (node == outputNode)
                    outputType = inputStruct.Identifier;

                string executionIdentifier = "exec" + getUniqueID();

                dictionaryExecuteIdentifiers[node] = executionIdentifier;

                SourceCode += outputType + " " + executionIdentifier + "(" + varyingIdentifier + " varyings, " + inputStruct.Identifier + " input) {\n";

                SourceCode += "\t" + outputType + " output;\n";

                Dictionary<Variable, string> mergedDictionary = new Dictionary<Variable,string>();

                foreach (Variable variable in inputStruct.dictonaryVariableIdentifiers.Keys)
                    mergedDictionary[variable] = inputStruct.dictonaryVariableIdentifiers[variable];

                foreach (Variable variable in outputStruct.dictonaryVariableIdentifiers.Keys)
                    mergedDictionary[variable] = outputStruct.dictonaryVariableIdentifiers[variable];

                SourceCode += node.inode.GetSource(mergedDictionary);
                
                SourceCode += "\treturn output;\n";

                SourceCode += "}\n\n";
            }

            // Add all get functions
            SourceCode += "// Get functions \n\n";

            generateGetFunctions(outputNode);

            // Add the interface function
            
            SourceCode += "float4 getColor(float4 position, float4 normal, float4 camera) {\n";

            SourceCode += "\t" + varyingIdentifier + " varyings;\n";
            SourceCode += "\tvaryings.position = position;\n";
            SourceCode += "\tvaryings.normal = normal;\n";
            SourceCode += "\tvaryings.camera = camera;\n";

            SourceCode += "\t" + dictonaryInputStruct[outputNode].Identifier + " output;\n";
            SourceCode += "\toutput = " + dictionaryGetIdentifiers[outputNode] + "(varyings); \n\n";

            string outputVarName = dictonaryInputStruct[outputNode].dictonaryVariableIdentifiers[outputNode.Variables[0]];

            if (outputNode.Variables[0].InputType == Variable.InputTypes.Link && outputNode.Variables[0].GetLinks().Count == 1)
            {
                if (outputNode.Variables[0].GetLinks()[0].OutputVariable.IsFloat4())
                    SourceCode += "\tfloat4 color = output." + outputVarName + ";\n";
                else if (outputNode.Variables[0].GetLinks()[0].OutputVariable.IsFloat3())
                    SourceCode += "\tfloat4 color = float4(output." + outputVarName + ", 1);\n";
                else if (outputNode.Variables[0].GetLinks()[0].OutputVariable.IsFloat2())
                    SourceCode += "\tfloat4 color = float4(output." + outputVarName + ", 0, 1);\n";
                else
                    SourceCode += "\tfloat4 color = float4(output." + outputVarName + ", output." + outputVarName + ", output." + outputVarName + ", 1);\n";
            }
            else
            {
                SourceCode += "\tfloat4 color = output." + outputVarName + ";\n";
            }
             
            SourceCode += "\treturn color;\n";
            SourceCode += "}\n";
            
        }

        // Recursively generates the get functions
        private Dictionary<Node, string> dictionaryGetIdentifiers;
        
        private void generateGetFunctions(Node node)
        {
            // Check if node was already generated
            if (dictionaryGetIdentifiers.Keys.Contains(node))
                return;

             // Get identifier
            string getIdentifier = "get" + getUniqueID();

            dictionaryGetIdentifiers[node] = getIdentifier;

            // Execute gets for all input nodes that are linked first
            foreach (Variable variable in node.Variables.Where(v => v.Type == Variable.VariableType.Input))
            {
                if (variable.InputType == Variable.InputTypes.Link)
                {
                    Connection connection = variable.GetLinks()[0];

                    Variable linkedVariable = connection.OutputVariable;

                    generateGetFunctions(linkedVariable.Node);
                }
            }

            // Construct input object
            string outputType = dictonaryOutputStruct[node].Identifier;

            if (node == outputNode)
                outputType = dictonaryInputStruct[node].Identifier;

            SourceCode += outputType + " " + getIdentifier + "(IOVaryings varyings) {\n";

            SourceCode += "\t" + dictonaryInputStruct[node].Identifier + " input; \n";

            foreach (Variable variable in node.Variables.Where(v => v.Type == Variable.VariableType.Input))
            {
                if (variable.InputType == Variable.InputTypes.Link)
                {
                    Connection connection = variable.GetLinks()[0];

                    Variable linkedVariable = connection.OutputVariable;

                    string linkedVariableIdentifier = dictonaryOutputStruct[linkedVariable.Node].dictonaryVariableIdentifiers[linkedVariable];

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + dictionaryGetIdentifiers[linkedVariable.Node] + "(varyings)." + linkedVariableIdentifier + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Float1)
                {
                    string variableValue = "float(" + variable.getFloat1() + ")";

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Float2)
                {
                    string variableValue = "float2(" + variable.getFloat1() + ", " + variable.getFloat2() + ")";

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Float3)
                {
                    string variableValue = "float3(" + variable.getFloat1() + ", " + variable.getFloat2() + ", " + variable.getFloat3() + ")";

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Float4)
                {
                    string variableValue = "float4(" + variable.getFloat1() + ", " + variable.getFloat2() + ", " + variable.getFloat3() + ", " + variable.getFloat4() + ")";

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Color)
                {
                    string variableValue = "float4(" + variable.getColor().R + " / 256.0f, " + variable.getColor().G + " / 256.0f, " + variable.getColor().B + " / 256.0f, " + variable.getColor().A + " / 256.0f)";

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
                else if (variable.InputType == Variable.InputTypes.Varying)
                {
                    string variableValue = "varyings." + (variable.inputVarying.SelectedValue as ComboBoxItem).Content;

                    SourceCode += "\tinput." + dictonaryInputStruct[node].dictonaryVariableIdentifiers[variable] + " = " + variableValue + ";\n";
                }
            }

            // Invoke exection function
            SourceCode += "\n\t" + outputType + " output = " + dictionaryExecuteIdentifiers[node] + "(varyings, input);\n";

            SourceCode += "\treturn output;\n";

            SourceCode += "}\n\n";
        }

        // Provides unique id's
        private int getUniqueID()
        {
            return uniqueID++;
        }

        private static int uniqueID = 0;
    }
}
