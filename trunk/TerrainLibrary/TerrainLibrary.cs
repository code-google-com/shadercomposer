using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerrainLibrary.Nodes;
using ShaderComposer.Libraries;

namespace TerrainLibrary
{
    public class TerrainLibrary : ILibrary
    {
        public string GetName()
        {
            return "Terrain";
        }

        private const string IDENTIFIER = "3778ee0f-e93a-461a-8ff2-5e1ada9ae329";

        public Guid GetIdentifier()
        {
            return new Guid(IDENTIFIER);
        }

        public List<Type> GetNodeTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(Grass));
            types.Add(typeof(Sand));
            types.Add(typeof(Snow));
            types.Add(typeof(SnowRock));

            types.Add(typeof(Noise));

            types.Add(typeof(Tresholding));

            return types;
        }
    }
}
