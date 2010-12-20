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
using System.Windows.Interop;

namespace ShaderComposer.Interface.VisualTrailing
{
    /// <summary>
    /// Interaction logic for StateNode.xaml
    /// </summary>
    public partial class StateNode : UserControl
    {
        public StateNode()
        {
            InitializeComponent();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            StateNodeBorder.BorderThickness = new Thickness(1);
        }

        private void StateNodeBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            StateNodeBorder.BorderThickness = new Thickness(0);
        }

        // Filestate
        private FileState fileState;
        public FileState FileState
        {
            get
            {
                return fileState;
            }

            set
            {
                fileState = value;

                D3DImage image = FileState.File.FileView.PreviewImage.Source as D3DImage;

                if (image != null)
                {
                   // D3DImage cloneImage = image.CloneCurrentValue();

                   // StateImage.Source = cloneImage;
                }
            }
        }
    }
}
