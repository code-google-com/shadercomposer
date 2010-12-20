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
            Image previewImage = ((PreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as Image;

            previewImage.Source = imageSource;
        }

        public Image PreviewImage
        {
            get
            {
                return ((PreviewTabItem.Items[0] as FileViewTabItem).Content as Grid).Children[0] as Image;
            }
        }

        // Preview pixel clicked event
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;

            Line HorizontalLine2 = g.Children[2] as Line;
            Line VerticalLine1 = g.Children[1] as Line;

            HorizontalLine2.Y1 = e.GetPosition(sender as IInputElement).Y;
            HorizontalLine2.Y2 = e.GetPosition(sender as IInputElement).Y;

            VerticalLine1.X1 = e.GetPosition(sender as IInputElement).X;
            VerticalLine1.X2 = e.GetPosition(sender as IInputElement).X;
        }
    }
}
