using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ShaderComposer.FileManagement;
using ShaderComposer.Interface.Designer;
using ShaderComposer.Interface.XMLViewing;
using ShaderComposer.Interface.ShaderIO;
using ShaderComposer.Interface.VisualTrailing;
using ShaderComposer.FileManagers;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Interface.Designer.Canvas;
using ShaderComposer.Compilers.HLSL;
using ShaderComposer.Renderers;

namespace ShaderComposer.Interface.FileViewing
{
    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : UserControl
    {
        public FileView()
        {
            InitializeComponent();
        }

        public void Initialize(File file)
        {
            // Notify the file that we are its viewer
            file.FileView = this;
        }

        // Design area
        public DesignArea DesignArea
        {
            get
            {
                return DesignAreaTab.Content as DesignArea;
            }
        }

        // XML
        public XMLView XMLView
        {
            get
            {
                return XMLTab.Content as XMLView;
            }
        }

        // Tree
        public TreeViewing.TreeView TreeView
        {
            get
            {
                return TreeTab.Content as TreeViewing.TreeView;
            }
        }

        // Shader Input
        public ShaderInput ShaderInput
        {
            get
            {
                return ShaderInputTab.Content as ShaderInput;
            }
        }

        // Shader Output
        public ShaderOutput ShaderOutput
        {
            get
            {
                return ShaderOutputTab.Content as ShaderOutput;
            }
        }
        
        // Visual Trail
        public VisualTrail VisualTrail
        {
            get
            {
                return VisualTrailTab.Content as VisualTrail;
            }
        }

        // Sets a new preview source
        public void SetPreviewSource(ImageSource imageSource)
        {
            PreviewImage.Source = imageSource;
        }

        public Image PreviewImage
        {
            get
            {
                return (((PreviewTabItem.Items[0] as FileViewTabItem).Content as StackPanel).Children[0] as Grid).Children[0] as Image;
            }
        }

        public Label TestValueLabel
        {
            get
            {
                return (((PreviewTabItem.Items[0] as FileViewTabItem).Content as StackPanel).Children[1] as StackPanel).Children[1] as Label;
            }
        }

        // Preview pixel clicked event
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;

            Line HorizontalLine2 = g.Children[2] as Line;
            Line VerticalLine1 = g.Children[1] as Line;

            double x = e.GetPosition(sender as IInputElement).X / PreviewImage.Width;
            double y = e.GetPosition(sender as IInputElement).Y / PreviewImage.Height;

            HorizontalLine2.Y1 = e.GetPosition(sender as IInputElement).Y;
            HorizontalLine2.Y2 = e.GetPosition(sender as IInputElement).Y;

            VerticalLine1.X1 = e.GetPosition(sender as IInputElement).X;
            VerticalLine1.X2 = e.GetPosition(sender as IInputElement).X;
        }

        public void UpdateTestValues()
        {
            if (FilesManager.Instance.ActiveFile == null)
                return;

            if (FilesManager.Instance.ActiveFile.ActiveState.Renderer == null)
                return;

            {
                Grid g = (((PreviewTabItem.Items[0] as FileViewTabItem).Content as StackPanel).Children[0] as Grid);

                Line HorizontalLine2 = g.Children[2] as Line;
                Line VerticalLine1 = g.Children[1] as Line;

                double x = VerticalLine1.X1 / PreviewImage.Width;
                double y = HorizontalLine2.Y1 / PreviewImage.Height;

                // Update the test value
                string value = FilesManager.Instance.ActiveFile.ActiveState.Renderer.GetValueAt(x, y);
                TestValueLabel.Content = value;
            }

            if (ir0 != null)
            {
                Grid g = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as StackPanel).Children[1] as Grid);
                Label l = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as StackPanel).Children[2] as StackPanel).Children[1] as Label;

                Line HorizontalLine2 = g.Children[2] as Line;
                Line VerticalLine1 = g.Children[1] as Line;

                double x = VerticalLine1.X1 / 192.0;
                double y = HorizontalLine2.Y1 / 192.0;

                // Update the test value
                string value = ir0.GetValueAt(x, y);
                l.Content = value;
            }

            if (ir1 != null)
            {
                Grid g = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[1] as StackPanel).Children[1] as Grid);
                Label l = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[1] as StackPanel).Children[2] as StackPanel).Children[1] as Label;

                Line HorizontalLine2 = g.Children[2] as Line;
                Line VerticalLine1 = g.Children[1] as Line;

                double x = VerticalLine1.X1 / 192.0;
                double y = HorizontalLine2.Y1 / 192.0;

                // Update the test value
                string value = ir1.GetValueAt(x, y);
                l.Content = value;
            }
        }

        public class VInf
        {
            public Variable v;

            public override string ToString()
            {
                return v.Node.NodeName.Text + " - " + v.Text;
            }
        }

        public void SetIntermediateOutputs(List<Variable> io)
        {
            ComboBox cb0 = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as StackPanel).Children[0] as ComboBox);
            ComboBox cb1 = ((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[1] as StackPanel).Children[0] as ComboBox);

            object old0 = cb0.SelectedValue;
            object old1 = cb1.SelectedValue;

            cb0.Items.Clear();
            cb1.Items.Clear();

            foreach(Variable v in io) {
                VInf vinf = new VInf();
                vinf.v = v;

                cb0.Items.Add(vinf);
                cb1.Items.Add(vinf);
            }

            cb0.SelectedValue = old0;
            cb1.SelectedValue = old1;
        }

        private void Grid0_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;

            Line HorizontalLine2 = g.Children[2] as Line;
            Line VerticalLine1 = g.Children[1] as Line;

            double x = e.GetPosition(sender as IInputElement).X / 192.0;
            double y = e.GetPosition(sender as IInputElement).Y / 192.0;

            HorizontalLine2.Y1 = e.GetPosition(sender as IInputElement).Y;
            HorizontalLine2.Y2 = e.GetPosition(sender as IInputElement).Y;

            VerticalLine1.X1 = e.GetPosition(sender as IInputElement).X;
            VerticalLine1.X2 = e.GetPosition(sender as IInputElement).X;
        }

        private void Grid1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;

            Line HorizontalLine2 = g.Children[2] as Line;
            Line VerticalLine1 = g.Children[1] as Line;

            double x = e.GetPosition(sender as IInputElement).X / 192.0;
            double y = e.GetPosition(sender as IInputElement).Y / 192.0;

            HorizontalLine2.Y1 = e.GetPosition(sender as IInputElement).Y;
            HorizontalLine2.Y2 = e.GetPosition(sender as IInputElement).Y;

            VerticalLine1.X1 = e.GetPosition(sender as IInputElement).X;
            VerticalLine1.X2 = e.GetPosition(sender as IInputElement).X;
        }

        private void ComboBox0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Image img = (((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as StackPanel).Children[1] as Grid).Children[0] as Image);

            if (ir0 != null)
            {
                ir0.Destroy();
                img.Source = null;
                ir0 = null;
            }

            //
            FileState altState = new FileState(FilesManager.Instance.ActiveFile.ActiveState, false);

            // Find output
            Node outputNode = null;

            foreach (Node node in altState.Nodes)
            {
                if (node.inode.IsOutputNode())
                {
                    outputNode = node;
                    break;
                }
            }

            if ((sender as ComboBox).SelectedValue == null)
                return;

            // Find selected node
            Variable oldSelectedVariable = ((sender as ComboBox).SelectedValue as VInf).v;
            Node selectedNode = altState.dictionaryOldToNew[oldSelectedVariable.Node];

            int oldIndex = oldSelectedVariable.Node.Variables.IndexOf(oldSelectedVariable);
            Variable selectedVariable = selectedNode.Variables[oldIndex];
            
            // Remove output input if it was linked
            if (outputNode.Variables[0].InputType == Variable.InputTypes.Link)
            {
                outputNode.Variables[0].GetLinks()[0].removeConnection();
            }

            // Find var output if it was linked
            Variable outputVariable = selectedVariable.GetLinks()[0].OutputVariable;

            // Create a new connection
            Connection c = new Connection();
            c.OutputVariable = outputVariable;
            c.InputVariable = outputNode.Variables[0];
            
            // Compile source
            HLSLCompiler comp = new HLSLCompiler(altState);
            comp.Compile();

            ir0 = FilesManager.Instance.ActiveFile.ActiveState.Renderer.Create();
            ImageSource images = ir0.Initialize();
            ir0.SetSourceCode(comp.SourceCode);
            
            img.Source = images;
        }
        IRenderer ir0;

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Image img = (((((IntermediatePreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[1] as StackPanel).Children[1] as Grid).Children[0] as Image);

            if (ir1 != null)
            {
                ir1.Destroy();
                img.Source = null;
                ir1 = null;
            }

            //
            FileState altState = new FileState(FilesManager.Instance.ActiveFile.ActiveState, false);

            // Find output
            Node outputNode = null;

            foreach (Node node in altState.Nodes)
            {
                if (node.inode.IsOutputNode())
                {
                    outputNode = node;
                    break;
                }
            }

            if ((sender as ComboBox).SelectedValue == null)
                return;

            // Find selected node
            Variable oldSelectedVariable = ((sender as ComboBox).SelectedValue as VInf).v;
            Node selectedNode = altState.dictionaryOldToNew[oldSelectedVariable.Node];

            int oldIndex = oldSelectedVariable.Node.Variables.IndexOf(oldSelectedVariable);
            Variable selectedVariable = selectedNode.Variables[oldIndex];
            
            // Remove output input if it was linked
            if (outputNode.Variables[0].InputType == Variable.InputTypes.Link)
            {
                outputNode.Variables[0].GetLinks()[0].removeConnection();
            }

            // Find var output if it was linked
            Variable outputVariable = selectedVariable.GetLinks()[0].OutputVariable;

            // Create a new connection
            Connection c = new Connection();
            c.OutputVariable = outputVariable;
            c.InputVariable = outputNode.Variables[0];

            // Compile source
            HLSLCompiler comp = new HLSLCompiler(altState);
            comp.Compile();

            ir1 = FilesManager.Instance.ActiveFile.ActiveState.Renderer.Create();
            ImageSource images = ir1.Initialize();
            ir1.SetSourceCode(comp.SourceCode);

            img.Source = images;
        }
        IRenderer ir1;

    }
}
