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
using ShaderComposer.Renderers;
using ShaderComposer.FileManagement;
using ShaderComposer.FileManagers;
using ShaderComposer.Interface.Designer.Variables;
using ShaderComposer.Compilers.HLSL;

namespace ShaderComposer.Interface.Designer
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : UserControl
    {
        public PreviewWindow()
        {
            InitializeComponent();

            PinImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ShaderComposer;component/Interface/Icons/Pin.gif"));
        }

        private bool pinned;
        public bool Pinned
        {
            get
            {
                return pinned;
            }

            set
            {
                if (parent.InputVariable == null)
                {
                    pinned = false;
                    return;
                }

                pinned = value;

                // Update icon
                if (pinned)
                {
                    PinImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ShaderComposer;component/Interface/Icons/Unpin.gif"));
                }
                else
                {
                    PinImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/ShaderComposer;component/Interface/Icons/Pin.gif"));
                }
            }
        }

        public Path path;
        public Connection parent;

        //
        private void MouseEnterLeave(object sender, MouseEventArgs e)
        {
            bool mouseOverPath = Mouse.DirectlyOver == path;
            bool mouseOverPreview = (Mouse.DirectlyOver == PreviewGrid || Mouse.DirectlyOver == PreviewBorder || Mouse.DirectlyOver == PreviewImage || Mouse.DirectlyOver == PinImage);

            if ((mouseOverPath || mouseOverPreview))
                Visibility = Visibility.Visible;
            else
                Visibility = Visibility.Collapsed;

            if (Pinned)
                Visibility = Visibility.Visible;

            Opacity = Pinned ? 1.0 : 0.6;
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pinned = !Pinned;

            MouseEnterLeave(null, null);
            
            e.Handled = true;
        }

        IRenderer ir0;
        Variable oldSelectedVariable;

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (parent.InputVariable == oldSelectedVariable)
                return;

            Image img = PreviewImage;

            if (ir0 != null)
            {
                ir0.Destroy();
                img.Source = null;
                ir0 = null;
            }

            //
            if (!FilesManager.Instance.ActiveFile.ActiveState.IsComplete)
                return;

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
            
            // Find selected node
            oldSelectedVariable = parent.InputVariable;
            Node selectedNode = altState.dictionaryOldToNew[oldSelectedVariable.Node];

            int oldIndex = oldSelectedVariable.Node.Variables.IndexOf(oldSelectedVariable);
            Variable selectedVariable = selectedNode.Variables[oldIndex];
            
            // Remove output input if it was linked
            if (outputNode.Variables[0].InputType == Variable.InputTypes.Link)
            {
                outputNode.Variables[0].GetLinks()[0].removeConnection();
            }

            // Find var output if it was linked
            if (selectedVariable.GetLinks().Count > 0 && FilesManager.Instance.ActiveFile.ActiveState.Renderer != null)
            {
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
        }
        
    }
}
