using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.FileManagement;
using System.Xml;
using ShaderComposer.Interface.Designer;
using ShaderComposer.Interface.Designer.Canvas;
using ShaderComposer.Interface.Designer.Variables;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace ShaderComposer.Compilers.XML
{
    public class XMLCompiler
    {
        private FileState fileState;

        public XMLCompiler(FileState fileState)
        {
            this.fileState = fileState;
        }

        // Latest compiled source code
        public string SourceCode { get; private set; }

        public void Compile()
        {
            // Setup the XML writer
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            
            XmlWriter xmlWriter = XmlWriter.Create(sb, settings);
            
            // Begin description
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteComment(" Auto generated XML description ");
            
            xmlWriter.WriteStartElement("ShaderDescription");
            xmlWriter.WriteAttributeString("Version", "0.1");
            
            // Write node descriptions
            xmlWriter.WriteStartElement("Nodes");

            foreach (Node node in fileState.Nodes)
            {
                writeNode(xmlWriter, node);
            }

            xmlWriter.WriteEndElement();

            // Write connection descriptions
            xmlWriter.WriteStartElement("Connections");

            foreach (Node node in fileState.Nodes)
            {
                foreach (Variable variable in node.Variables.Where(x => x.Type == Variable.VariableType.Input))
                {
                    if (variable.InputType == Variable.InputTypes.Link)
                    {
                        Connection connection = variable.GetLinks()[0];

                        writeConnection(xmlWriter, connection);
                    }
                }
            }

            xmlWriter.WriteEndElement();

            // Finalize description
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();

            xmlWriter.Flush();

            // Store final string
            SourceCode = sb.ToString();
        }

        private void writeNode(XmlWriter xmlWriter, Node node)
        {
            xmlWriter.WriteStartElement("Node");

            // Type
            string guid = node.inode.GetIdentifier().ToString();
            xmlWriter.WriteAttributeString("TypeID", guid);

            // Position
            double positionX = DynamicCanvas.GetLeft(node); string valueX = "" + ((int)(positionX * 100.0) / 100.0);
            double positionY = DynamicCanvas.GetTop(node);  string valueY = "" + ((int)(positionY * 100.0) / 100.0);

            xmlWriter.WriteAttributeString("X", valueX);
            xmlWriter.WriteAttributeString("Y", valueY);

            // Write all variables
            xmlWriter.WriteStartElement("Variables");

            foreach (Variable variable in node.Variables)
            {
                xmlWriter.WriteStartElement("Variable");

                xmlWriter.WriteAttributeString("ID", variable.GetHashCode().ToString("X8"));
                
                xmlWriter.WriteAttributeString("Name", variable.Name);

                xmlWriter.WriteAttributeString("Text", variable.Text);

                xmlWriter.WriteAttributeString("Type", variable.InputType.ToString());

                if (variable.InputType == Variable.InputTypes.Boolean)
                    xmlWriter.WriteAttributeString("value", variable.getBoolean().ToString());
                if (variable.InputType == Variable.InputTypes.Varying)
                    xmlWriter.WriteAttributeString("value",  (variable.inputVarying.SelectedValue as ComboBoxItem).Content as string);
                if (variable.InputType == Variable.InputTypes.Float1 || variable.InputType == Variable.InputTypes.Float2 || variable.InputType == Variable.InputTypes.Float3 || variable.InputType == Variable.InputTypes.Float4)
                    xmlWriter.WriteAttributeString("value1", variable.getFloat1());
                if (variable.InputType == Variable.InputTypes.Float2 || variable.InputType == Variable.InputTypes.Float3 || variable.InputType == Variable.InputTypes.Float4)
                    xmlWriter.WriteAttributeString("value2", variable.getFloat2());
                if (variable.InputType == Variable.InputTypes.Float3 || variable.InputType == Variable.InputTypes.Float4)
                    xmlWriter.WriteAttributeString("value3", variable.getFloat3());
                if (variable.InputType == Variable.InputTypes.Float4)
                    xmlWriter.WriteAttributeString("value4", variable.getFloat4());
                if (variable.InputType == Variable.InputTypes.Color)
                {
                    xmlWriter.WriteAttributeString("value1", variable.getColor().R.ToString());
                    xmlWriter.WriteAttributeString("value2", variable.getColor().G.ToString());
                    xmlWriter.WriteAttributeString("value3", variable.getColor().B.ToString());
                    xmlWriter.WriteAttributeString("value4", variable.getColor().A.ToString());
                }

                xmlWriter.WriteAttributeString("dim1", variable.IsFloat1().ToString());
                xmlWriter.WriteAttributeString("dim2", variable.IsFloat2().ToString());
                xmlWriter.WriteAttributeString("dim3", variable.IsFloat3().ToString());
                xmlWriter.WriteAttributeString("dim4", variable.IsFloat4().ToString());

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        private void writeConnection(XmlWriter xmlWriter, Connection connection)
        {
            xmlWriter.WriteStartElement("Connection");

            xmlWriter.WriteAttributeString("SourceID", connection.OutputVariable.GetHashCode().ToString("X8"));
            xmlWriter.WriteAttributeString("DestinationID", connection.InputVariable.GetHashCode().ToString("X8"));
            xmlWriter.WriteAttributeString("PreviewPinned", connection.PreviewWindow.Pinned.ToString());

            xmlWriter.WriteEndElement();
        }
    }
}
