using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Libraries;
using ShaderComposer.Renderers;
using System.Windows.Media;
using ShaderComposer.Interface.Designer;
using ShaderComposer.Compilers;
using ShaderComposer.Interface.Designer.Variables;

namespace ShaderComposer.FileManagement
{
    public class FileState
    {
        public FileState(File file)
        {
            this.file = file;
        }

        // Create a copy of the file state
        public FileState(FileState fileState)
        {
            this.file = fileState.File;
            this.renderer = fileState.Renderer;

            // Copy the nodes
            Dictionary<Node, Node> dictionaryOldToNew = new Dictionary<Node, Node>();

            foreach (Node node in fileState.Nodes)
            {
                Node newNode = new Node(node);

                dictionaryOldToNew[node] = newNode;

                Nodes.Add(newNode);
            }

            // Copy the connections
            foreach (Variable variable in fileState.Nodes.SelectMany(x => x.Variables).Where(v => v.Type == Variable.VariableType.Output && v.InputType == Variable.InputTypes.Link)) 
            {
                foreach (Connection connection in variable.GetLinks())
                {
                    int outIndex = connection.OutputVariable.Node.Variables.IndexOf(connection.OutputVariable);
                    int inIndex = connection.InputVariable.Node.Variables.IndexOf(connection.InputVariable);

                    Connection newConnection = new Connection();
                    newConnection.DesignArea = connection.DesignArea;
                    newConnection.OutputVariable = dictionaryOldToNew[connection.OutputVariable.Node].Variables[outIndex];
                    newConnection.InputVariable = dictionaryOldToNew[connection.InputVariable.Node].Variables[inIndex];
                }
            }

            PreviousState = fileState;
            fileState.NextStates.Add(this);
        }

        public void stop()
        {
            foreach (Node node in Nodes) {
                File.FileView.DesignArea.RemoveNode(node);

                foreach (Connection connection in Nodes.SelectMany(x => x.Variables).Where(v => v.Type == Variable.VariableType.Output && v.InputType == Variable.InputTypes.Link).SelectMany(v => v.GetLinks()))
                {
                    File.FileView.DesignArea.RemoveConnection(connection);
                }
            }
        }

        public void start()
        {
            foreach (Node node in Nodes)
            {
                File.FileView.DesignArea.AddNode(node);

                foreach (Connection connection in Nodes.SelectMany(x => x.Variables).Where(v => v.Type == Variable.VariableType.Output && v.InputType == Variable.InputTypes.Link).SelectMany(v => v.GetLinks()))
                {
                    if (connection.Parent == null)
                    {
                        File.FileView.DesignArea.AddConnection(connection);
                    }
                }
            }
        }

        // The parent file
        private File file;
        public File File
        {
            get 
            { 
                return file;
            }
        }

        //
        public FileState PreviousState;

        public List<FileState> NextStates = new List<FileState>();
        
        // Returns wether this file state is currently active
        public bool IsActiveState
        {
            get
            {
                return File.ActiveState == this;
            }
        }

        // The current renderer
        private IRenderer renderer;
        public IRenderer Renderer 
        {
            get
            {
                return renderer;
            }

            set
            {
                renderer = value.Create();

                ImageSource imageSource = renderer.Initialize();
                File.FileView.SetPreviewSource(imageSource);

                Build();
            }
        }

        // All nodes present in the graph
        public List<Node> Nodes = new List<Node>();

        // Adds a new node to the graph
        public void AddNewNode(Type nodeType)
        {
            INode inode = nodeType.GetConstructor(new Type[0]).Invoke(new Object[0]) as INode;

            Node node = new Node(inode);

            File.FileView.DesignArea.AddNode(node);

            Nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);

            File.FileView.DesignArea.RemoveNode(node);
        }

        // Rebuilds the shader
        public void Build()
        {
            // Compile the shader
            HLSLCompiler hlslCompiler = new HLSLCompiler(this);
            hlslCompiler.Compile();

            // Update shader source code viewer
            File.FileView.ShaderOutput.SourceCode = hlslCompiler.SourceCode;

            // Update the current renderer
            if (renderer != null)
            {
                renderer.SetSourceCode(hlslCompiler.SourceCode);
            }
        }
    }
}
