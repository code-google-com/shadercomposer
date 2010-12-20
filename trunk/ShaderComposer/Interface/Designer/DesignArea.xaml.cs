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

using ShaderComposer.Interface.Designer.Canvas;
using ShaderComposer.Interface.Designer.Variables;

namespace ShaderComposer.Interface.Designer
{
    /// <summary>
    /// Interaction logic for DesignArea.xaml
    /// </summary>
    public partial class DesignArea : UserControl
    {
        public DesignArea()
        {
            InitializeComponent();

        }

        // Methos to add nodes and connections
        public void AddNode(Node newNode)
        {
            Canvas.Children.Add(newNode);

            newNode.DesignArea = this;
        }

        public void RemoveNode(Node node)
        {
            Canvas.Children.Remove(node);
        }

        public void AddConnection(Connection newConnection)
        {
            Canvas.Children.Add(newConnection);
        }

        public void RemoveConnection(Connection connection)
        {
            Canvas.Children.Remove(connection);
        }

        // Z-Index
        private int highestZIndex = 5000;

        public int getHighestZIndex()
        {
            return highestZIndex++;
        }

        // Zooming
        private double currentZoom = 0.0;

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.MinWidth = ScrollViewer.ActualWidth / Math.Pow(2, currentZoom);
            Canvas.MinHeight = ScrollViewer.ActualHeight / Math.Pow(2, currentZoom);

            Canvas.UpdateLayout();

            Viewbox.Width = Canvas.ActualWidth * Math.Pow(2, currentZoom);
            Viewbox.Height = Canvas.ActualHeight * Math.Pow(2, currentZoom);

            double delta = 0.01;

            ScrollViewer.HorizontalScrollBarVisibility = ScrollViewer.ActualWidth < Viewbox.Width - delta ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
            ScrollViewer.VerticalScrollBarVisibility = ScrollViewer.ActualHeight < Viewbox.Height - delta ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer.HorizontalScrollBarVisibility = ScrollViewer.ActualWidth < Viewbox.Width - 0.01 ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
            ScrollViewer.VerticalScrollBarVisibility = ScrollViewer.ActualHeight < Viewbox.Height - 0.01 ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Viewbox.Width = Canvas.ActualWidth * Math.Pow(2, currentZoom);
            Viewbox.Height = Canvas.ActualHeight * Math.Pow(2, currentZoom);
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                currentZoom += e.Delta / 500.0;

                Canvas.MinWidth = ScrollViewer.ActualWidth / Math.Pow(2, currentZoom);
                Canvas.MinHeight = ScrollViewer.ActualHeight / Math.Pow(2, currentZoom);

                Canvas.UpdateLayout();

                Viewbox.Width = Canvas.ActualWidth * Math.Pow(2, currentZoom);
                Viewbox.Height = Canvas.ActualHeight * Math.Pow(2, currentZoom);

                e.Handled = true;
            }
        }
    }
}
