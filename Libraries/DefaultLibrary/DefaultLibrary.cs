using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ShaderComposer.Libraries;

using DefaultLibrary.Nodes;

namespace DefaultLibrary
{
    public class DefaultLibrary : ILibrary
    {
        public string GetName()
        {
            return "Default";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae329";

        public Guid GetIdentifier()
        {
            return new Guid(IDENTIFIER);
        }

        public List<Type> GetNodeTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(Output));

            types.Add(typeof(Add));
            types.Add(typeof(Substract));
            types.Add(typeof(Multiply));
            types.Add(typeof(Power));

            types.Add(typeof(Normalize));
            types.Add(typeof(Saturate));
            types.Add(typeof(Dot));

            types.Add(typeof(Decompose));
            types.Add(typeof(MAD));
            types.Add(typeof(Fog));

            return types;
        }
    }
}
