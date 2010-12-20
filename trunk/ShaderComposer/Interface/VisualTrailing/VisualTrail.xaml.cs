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
using ShaderComposer.Interface.Designer.Canvas;

namespace ShaderComposer.Interface.VisualTrailing
{
    /// <summary>
    /// Interaction logic for VisualTrail.xaml
    /// </summary>
    public partial class VisualTrail : UserControl
    {
        public VisualTrail()
        {
            InitializeComponent();
        }

        private double leftValue = 5;

        public void AddStateNode(FileState fileState)
        {
            StateNode stateNode = new StateNode();
            stateNode.FileState = fileState;

            DynamicCanvas.SetLeft(stateNode, leftValue);
            DynamicCanvas.SetTop(stateNode, 5);

            leftValue += 120;

            Canvas.Children.Add(stateNode);
        }
    }
}
