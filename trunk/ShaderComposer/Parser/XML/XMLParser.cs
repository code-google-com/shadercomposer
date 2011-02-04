using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.FileManagement;
using System.Xml;
using System.IO;
using System.Globalization;
using ShaderComposer.Libraries;
using System.Windows;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Interface.Designer;

namespace ShaderComposer.Parser.XML
{
    class XMLParser
    {
        public static void Parse(string code, ShaderComposer.FileManagement.File file)
        {
            StringReader sr = new StringReader(code);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            XmlReader xmlReader = XmlReader.Create(sr, settings);
            
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ",";

            xmlReader.ReadToFollowing("ShaderDescription");
            xmlReader.ReadToFollowing("Nodes");

            Dictionary<String, Variable> vars = new Dictionary<string, Variable>();

            if (xmlReader.ReadToDescendant("Node"))
            {
                do
                {
                    xmlReader.MoveToAttribute("TypeID"); string typeID = xmlReader.ReadContentAsString();

                    Type t = Libraries.LibraryManager.Instance.FindNode(typeID);

                    if (t == null)
                    {
                        return;
                    }

                    xmlReader.MoveToAttribute("X"); float x = float.Parse(xmlReader.ReadContentAsString(), nfi);
                    xmlReader.MoveToAttribute("Y"); float y = float.Parse(xmlReader.ReadContentAsString(), nfi);

                    Node n = file.ActiveState.AddNewNode(t, new Point(x, y));

                    System.Diagnostics.Debug.WriteLine("X: " + x + ", Y: " + y);

                    // Variables
                    xmlReader.ReadToFollowing("Variables");

                    if (xmlReader.ReadToDescendant("Variable"))
                    {
                        do
                        {
                            xmlReader.MoveToAttribute("ID"); string ID = xmlReader.ReadContentAsString();
                            xmlReader.MoveToAttribute("Name"); string Name = xmlReader.ReadContentAsString();
                            xmlReader.MoveToAttribute("Type"); string Type = xmlReader.ReadContentAsString();

                            foreach (Variable v in n.Variables)
                            {
                                if (v.Name == Name)
                                {
                                    vars[ID] = v;
                                    if (Type == "Link") v.InputType = Variable.InputTypes.Link;
                                    if (Type == "Color") v.InputType = Variable.InputTypes.Color;
                                    if (Type == "Varying") v.InputType = Variable.InputTypes.Varying;
                                    if (Type == "Float1") v.InputType = Variable.InputTypes.Float1;
                                    if (Type == "Float2") v.InputType = Variable.InputTypes.Float2;
                                    if (Type == "Float3") v.InputType = Variable.InputTypes.Float3;
                                    if (Type == "Float4") v.InputType = Variable.InputTypes.Float4;
                                    if (Type == "Boolean") v.InputType = Variable.InputTypes.Boolean;
                                }
                            }
                        } while (xmlReader.ReadToNextSibling("Variable"));
                        
                        xmlReader.ReadEndElement();
                    }

                    xmlReader.ReadEndElement();

                } while (xmlReader.Name == "Node");
            }

            // Conn
            xmlReader.ReadToFollowing("Connections");
            if (xmlReader.ReadToDescendant("Connection"))
            {
                do
                {
                    xmlReader.MoveToAttribute("SourceID"); string sourceID = xmlReader.ReadContentAsString();
                    xmlReader.MoveToAttribute("DestinationID"); string destinationID = xmlReader.ReadContentAsString();

                    // Create connection
                    Connection c = new Connection();
                    c.OutputVariable = vars[sourceID];
                    c.InputVariable = vars[destinationID];

                    file.FileView.DesignArea.AddConnection(c);
                    c.DesignArea = file.FileView.DesignArea;
                } while (xmlReader.ReadToNextSibling("Connection"));
            }

            xmlReader.Close();
        }
    }
}
