﻿using System;
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
using ShaderComposer.Libraries;
using ShaderComposer.FileManagers;

namespace ShaderComposer.Interface.Designer
{
    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Node : UserControl
    {
        // Interface node definition
        public INode inode { get; private set; }

        public Node(INode inode)
        {
            InitializeComponent();

            this.inode = inode;

            DynamicCanvas.SetLeft(this, 0);
            DynamicCanvas.SetTop(this, 0);

            // Add all variables
            foreach (Variable variable in inode.GetVariables()) 
            {
                AddVariable(variable);
            }

            // Set title
            NodeName.Text = inode.GetName();
        }

        // Copy the node
        public Node(Node node)
        {
            InitializeComponent();

            this.inode = node.inode.GetType().GetConstructor(new Type[]{}).Invoke(new object[]{}) as INode;

            DynamicCanvas.SetLeft(this, DynamicCanvas.GetLeft(node));
            DynamicCanvas.SetTop(this, DynamicCanvas.GetTop(node));

            Width = node.Width;
            Height = node.Height;

            // Add all variables
            List<Variable> vars = inode.GetVariables();

            for (int i = 0; i < vars.Count; i++)
            {
                Variable nvar = vars[i];
                Variable ovar = node.Variables[i];

                nvar.Type = ovar.Type;
                nvar.InputType = ovar.InputType;

                nvar.Text = ovar.Text;

                nvar.inputFloat1.Text = ovar.inputFloat1.Text;
                nvar.inputFloat2.Text = ovar.inputFloat2.Text;
                nvar.inputFloat3.Text = ovar.inputFloat3.Text;
                nvar.inputFloat4.Text = ovar.inputFloat4.Text;
                nvar.inputColor.SelectedColor = ovar.inputColor.SelectedColor;
                nvar.inputBoolean.SelectedIndex = ovar.inputBoolean.SelectedIndex;
                nvar.inputVarying.SelectedIndex = ovar.inputVarying.SelectedIndex;

                nvar.typeMenuFloat1.IsChecked = ovar.IsFloat1();
                nvar.typeMenuFloat2.IsChecked = ovar.IsFloat2();
                nvar.typeMenuFloat3.IsChecked = ovar.IsFloat3();
                nvar.typeMenuFloat4.IsChecked = ovar.IsFloat4();
                nvar.typeMenuBoolean.IsChecked = true;

                AddVariable(nvar);
            }

            NodeName.Text = node.NodeName.Text;
        }

        // DesignArea that this variable belongs to
        public DesignArea DesignArea { get; set; }

        // All node variables
        public List<Variable> Variables = new List<Variable>();

        public void AddVariable(Variable variable)
        {
            StackPanel.Children.Add(variable);

            Variables.Add(variable);

            variable.Node = this;
        }

        // Highlight the close button on mouse over
        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            closeButton.Fill = Brushes.Orange;
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            closeButton.Fill = Brushes.LightGray;
        }

        // Delete the node if the close button is clicked
        private void closeButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Remove all connections
            foreach (Variable variable in Variables)
            {
                foreach (Connection connection in variable.GetLinks())
                {
                    connection.removeConnection();
                }
            }

            // Remove this node
            FilesManager.Instance.ActiveFile.ActiveState.RemoveNode(this);
        }

        // Handle node dragging
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double left = DynamicCanvas.GetLeft(this);
            double top = DynamicCanvas.GetTop(this);

            left += e.HorizontalChange;
            top += e.VerticalChange;

            if (left < 0) left = 0;
            if (top < 0) top = 0;

            DynamicCanvas.SetLeft(this, left);
            DynamicCanvas.SetTop(this, top);

        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            // Enable transparency and position this node above all others
            Opacity = 0.7;

            DynamicCanvas.SetZIndex(this, DesignArea.getHighestZIndex());

            // Position all connections to variables in this node above all other elements
            foreach (Variable var in Variables)
                foreach (Connection conn in var.GetLinks())
                    DynamicCanvas.SetZIndex(conn, DesignArea.getHighestZIndex());
                
            // Update the layout
            DesignArea.Canvas.UpdateLayout();
        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Opacity = 1.0;
        }
    }
}
