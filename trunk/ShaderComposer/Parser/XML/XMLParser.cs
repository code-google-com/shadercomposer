using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.FileManagement;
using System.Xml;
using System.IO;

namespace ShaderComposer.Parser.XML
{
    class XMLParser
    {
        public static void Parse(string code, ShaderComposer.FileManagement.File file)
        {
            StringReader sr = new StringReader(code);

            XmlReader xmlReader = XmlReader.Create(sr);

            xmlReader.ReadToFollowing("ShaderDescription");
            xmlReader.ReadToFollowing("Nodes");

            while (xmlReader.ReadToDescendant("Node"))
            {
                xmlReader.MoveToAttribute("X"); float x = xmlReader.ReadContentAsFloat();
                xmlReader.MoveToAttribute("Y"); float y = xmlReader.ReadContentAsFloat();

                System.Diagnostics.Debug.WriteLine("X: " + x + ", Y: " + y);
            }

            xmlReader.Close();
        }
    }
}
