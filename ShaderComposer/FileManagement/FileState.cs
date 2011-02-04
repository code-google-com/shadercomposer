using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

using ShaderComposer.Libraries;
using ShaderComposer.Renderers;
using ShaderComposer.Interface.Designer;
using ShaderComposer.Compilers.HLSL;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Interface.Designer.Canvas;
using ShaderComposer.Compilers.XML;

namespace ShaderComposer.FileManagement
{
    public class FileState
    {
        public FileState(File file)
        {
            this.file = file;
        }

        // Copy the nodes
        public Dictionary<Node, Node> dictionaryOldToNew = new Dictionary<Node, Node>();

        // Create a copy of the file state
        public FileState(FileState fileState, bool newState = true)
        {
            this.file = fileState.File;
            this.renderer = fileState.Renderer;

            // Copy the nodes
            dictionaryOldToNew = new Dictionary<Node, Node>();

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
                    // newConnection.DesignArea = connection.DesignArea;
                    newConnection.OutputVariable = dictionaryOldToNew[connection.OutputVariable.Node].Variables[outIndex];
                    newConnection.InputVariable = dictionaryOldToNew[connection.InputVariable.Node].Variables[inIndex];
                }
            }

            if (newState)
            {
                PreviousState = fileState;
                fileState.NextStates.Add(this);
            }
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

                renderer.SceneUpdated += new SceneUpdatedHandler(renderer_SceneUpdated);

                Build();
            }
        }

        void renderer_SceneUpdated(object sender)
        {
            File.FileView.UpdateTestValues();
        }

        // All nodes present in the graph
        public List<Node> Nodes = new List<Node>();

        // Adds a new node to the graph
        public Node AddNewNode(Type nodeType, Point position)
        {
            INode inode = nodeType.GetConstructor(new Type[0]).Invoke(new Object[0]) as INode;

            Node node = new Node(inode);

            DynamicCanvas.SetLeft(node, position.X);
            DynamicCanvas.SetTop(node, position.Y);

            File.FileView.DesignArea.AddNode(node);

            Nodes.Add(node);

            return node;
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);

            File.FileView.DesignArea.RemoveNode(node);
        }

        // Rebuilds the shader
        public void Build()
        {
            if (!IsComplete)
                return;

            // Compile the shader
            HLSLCompiler hlslCompiler = new HLSLCompiler(this);
            hlslCompiler.Compile();

            // Update shader source code viewer
            File.FileView.ShaderOutput.SourceCode = hlslCompiler.SourceCode;

            // Update intermediate preview possibilities
            List<Variable> IntermediateOutputs = new List<Variable>();

            foreach (Node node in Nodes) 
            {
                if (!node.inode.IsOutputNode())
                    foreach (Variable variable in node.Variables)
                    {
                        if (variable.Type == Variable.VariableType.Input && variable.InputType == Variable.InputTypes.Link)
                        {
                            IntermediateOutputs.Add(variable);
                        }
                    }
            }

            File.FileView.SetIntermediateOutputs(IntermediateOutputs);

            // Update the current renderer
            if (renderer != null)
            {
                renderer.SetSourceCode(hlslCompiler.SourceCode);
            }
        }

        // Rebuilds the XML
        public void BuildXML()
        {
            if (!IsComplete)
                return;

            // Compile the shader to XML
            XMLCompiler xmlCompiler = new XMLCompiler(this);
            xmlCompiler.Compile();

            // Update xml viewer
            File.FileView.XMLView.TextEditor.Text = xmlCompiler.SourceCode;
        }

        // Checks if state is complete
        public bool IsComplete
        {
            get {
                // Check if every input variable with type = Link has exactly 1 link connection
                foreach (Node n in Nodes)
                {
                    foreach (Variable v in n.Variables)
                    {
                        if (v.Type == Variable.VariableType.Input)
                        {
                            if (v.InputType == Variable.InputTypes.Link)
                            {
                                if (v.GetLinks().Count != 1)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }
    }
}
