using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Libraries;

namespace LogicLibrary
{
    public class LogicLibrary : ILibrary
    {
        public string GetName()
        {
            return "Logic";
        }

        private const string IDENTIFIER = "6dccc1f3-1d02-479d-832f-357966953afb";

        public Guid GetIdentifier()
        {
            return new Guid(IDENTIFIER);
        }

        public List<Type> GetNodeTypes()
        {
            List<Type> types = new List<Type>();

            return types;
        }
    }
}
