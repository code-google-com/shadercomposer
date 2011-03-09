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
using System.Windows.Media;
using System.Windows.Controls;

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
                        continue;
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
                            xmlReader.MoveToAttribute("Text"); string Text = xmlReader.ReadContentAsString();
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

                                    if (Type == "Float1" || Type == "Float2" || Type == "Float3" || Type == "Float4") {
                                        xmlReader.MoveToAttribute("value1");
                                        v.inputFloat1.Text = xmlReader.ReadContentAsString();
                                    } 
                                    if (Type == "Float2" || Type == "Float3" || Type == "Float4") {
                                        xmlReader.MoveToAttribute("value2");
                                        v.inputFloat2.Text = xmlReader.ReadContentAsString();
                                    } 
                                    if (Type == "Float3" || Type == "Float4") {
                                        xmlReader.MoveToAttribute("value3");
                                        v.inputFloat3.Text = xmlReader.ReadContentAsString();
                                    } 
                                    if (Type == "Float4") {
                                        xmlReader.MoveToAttribute("value4");
                                        v.inputFloat4.Text = xmlReader.ReadContentAsString();
                                    } 
                                    if (Type == "Color") {
                                        xmlReader.MoveToAttribute("value1");
                                        byte r = (byte)xmlReader.ReadContentAsInt();
                                        xmlReader.MoveToAttribute("value2");
                                        byte g = (byte)xmlReader.ReadContentAsInt();
                                        xmlReader.MoveToAttribute("value3");
                                        byte b = (byte)xmlReader.ReadContentAsInt();
                                        xmlReader.MoveToAttribute("value4");
                                        byte a = (byte)xmlReader.ReadContentAsInt();

                                        v.inputColor.SelectedColor = Color.FromArgb(a, r, g, b);
                                    }
                                    if (Type == "Varying")
                                    {
                                        xmlReader.MoveToAttribute("value");
                                        string varying = xmlReader.ReadContentAsString();

                                        foreach (ComboBoxItem item in v.inputVarying.Items)
                                            if ((string)item.Content == varying)
                                            {
                                                v.inputVarying.SelectedItem = item;
                                                break;
                                            }

                                        //v.inputVarying.SelectedItem = varying;
                                    }

                                    xmlReader.MoveToAttribute("dim1");
                                    v.typeMenuFloat1.IsChecked = xmlReader.ReadContentAsString() == "True" ? true : false;
                                    xmlReader.MoveToAttribute("dim2");
                                    v.typeMenuFloat2.IsChecked = xmlReader.ReadContentAsString() == "True" ? true : false;
                                    xmlReader.MoveToAttribute("dim3");
                                    v.typeMenuFloat3.IsChecked = xmlReader.ReadContentAsString() == "True" ? true : false;
                                    xmlReader.MoveToAttribute("dim4");
                                    v.typeMenuFloat4.IsChecked = xmlReader.ReadContentAsString() == "True" ? true : false;

                                    v.updateTypeMenu();

                                    v.Text = Text;
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
                    try
                    {
                        xmlReader.MoveToAttribute("SourceID"); string sourceID = xmlReader.ReadContentAsString();
                        xmlReader.MoveToAttribute("DestinationID"); string destinationID = xmlReader.ReadContentAsString();
                        xmlReader.MoveToAttribute("PreviewPinned"); bool previewPinned = xmlReader.ReadContentAsString() == "True" ? true : false;

                        // Create connection
                        Connection c = new Connection();
                        c.OutputVariable = vars[sourceID];
                        c.InputVariable = vars[destinationID];

                        file.FileView.DesignArea.AddConnection(c);
                        c.DesignArea = file.FileView.DesignArea;
                    }
                    catch { }

                } while (xmlReader.ReadToNextSibling("Connection"));
            }

            xmlReader.Close();
        }
    }
}
