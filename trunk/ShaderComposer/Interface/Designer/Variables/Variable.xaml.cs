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

using Microsoft.Windows.Controls;
using System.Globalization;

namespace ShaderComposer.Interface.Designer.Variables
{
    /// <summary>
    /// Interaction logic for Variable.xaml
    /// </summary>
    public partial class Variable : UserControl
    {
        
        public Variable()
        {
            InitializeComponent();

            // Set default values
            Type = VariableType.Input;
            InputType = InputTypes.Link;

            // For float parsing
            ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
        }

        // Copy the variable
        public Variable(Variable variable)
        {
            InitializeComponent();

            Type = variable.Type;
            InputType = variable.InputType;

            Text = variable.Text;

            inputFloat1.Text = variable.inputFloat1.Text;
            inputFloat2.Text = variable.inputFloat2.Text;
            inputFloat3.Text = variable.inputFloat3.Text;
            inputFloat4.Text = variable.inputFloat4.Text;
            inputColor.SelectedColor = variable.inputColor.SelectedColor;
            inputBoolean.SelectedIndex = variable.inputBoolean.SelectedIndex;
            inputVarying.SelectedIndex = variable.inputVarying.SelectedIndex;
        }

        private CultureInfo ci;

        // Node that this variable belongs to
        public Node Node { get; set; }

        // DesignArea that this variable belongs to
        public DesignArea DesignArea { get { return Node.DesignArea;  } }

        // Name text
        public string Text
        {
            get
            {
                return ContentName.Text;
            }

            set
            {
                ContentName.Text = value;
            }
        }

        // Variable type: either input or output
        public enum VariableType
        {
            Input,
            Output
        }

        private VariableType type;
        public VariableType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;

                // Adjust control order
                if (type == VariableType.Input)
                {
                    Grid.SetColumn(ContentInputTypes, 0);
                    Grid.SetColumn(ContentName, 1);
                    Grid.SetColumn(ContentTypeIdentifiers, 2);

                    // Disable context menu to change input types
                    ContentInputTypes.ContextMenu.Visibility = Visibility.Visible;
                }
                else if (type == VariableType.Output)
                {
                    Grid.SetColumn(ContentInputTypes, 2);
                    Grid.SetColumn(ContentName, 1);
                    Grid.SetColumn(ContentTypeIdentifiers, 0);

                    // Force input type to link
                    InputType = InputTypes.Link;

                    // Disable context menu to change input types
                    ContentInputTypes.ContextMenu.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Validate float input
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;

            if (!validateFloat(textBox.Text))
            {
                textBox.Foreground = Brushes.Red;
            }
            else
            {
                textBox.Foreground = Brushes.Black;
            }
        }
        
        private bool validateFloat(string value)
        {
            float floatValue;
            return float.TryParse(value, NumberStyles.Any, ci, out floatValue);
        }

        // Retreive floats
        public string getFloat1()
        {
            return inputFloat1.Text; // float.Parse(inputFloat1.Text, NumberStyles.Any, ci);
        }

        public string getFloat2()
        {
            return inputFloat2.Text; // float.Parse(inputFloat2.Text, NumberStyles.Any, ci);
        }

        public string getFloat3()
        {
            return inputFloat3.Text; // float.Parse(inputFloat3.Text, NumberStyles.Any, ci);
        }

        public string getFloat4()
        {
            return inputFloat4.Text; // float.Parse(inputFloat4.Text, NumberStyles.Any, ci);
        }

        // Retreive color value
        public Color getColor()
        {
            return inputColor.SelectedColor;
        }

        // Input types: link, constant value or varying
        public enum InputTypes
        {
            Link,
            Float1,
            Float2,
            Float3,
            Float4,
            Color,
            Boolean,
            Varying
        }

        private InputTypes inputType;
        public InputTypes InputType 
        {
            get
            {
                return inputType;
            }

            set
            {
                inputType = value;

                // Change visibility of controls to collapsed
                inputLink.Visibility = Visibility.Collapsed;
                inputFloat1.Visibility = Visibility.Collapsed;
                inputFloat2.Visibility = Visibility.Collapsed;
                inputFloat3.Visibility = Visibility.Collapsed;
                inputFloat4.Visibility = Visibility.Collapsed;
                inputFloat4.Visibility = Visibility.Collapsed;
                inputColor.Visibility = Visibility.Collapsed;
                inputBoolean.Visibility = Visibility.Collapsed;
                inputVarying.Visibility = Visibility.Collapsed;

                // Break any connections if the type is not Link
                if (inputType != InputTypes.Link)
                {
                    foreach (Connection connection in GetLinks())
                    {
                        connection.removeConnection();
                    }
                }

                // Show the controls corresponding to the current input type
                switch (InputType)
                {
                    case InputTypes.Link:
                        inputLink.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Float1:
                        inputFloat1.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Float2:
                        inputFloat1.Visibility = Visibility.Visible;
                        inputFloat2.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Float3:
                        inputFloat1.Visibility = Visibility.Visible;
                        inputFloat2.Visibility = Visibility.Visible;
                        inputFloat3.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Float4:
                        inputFloat1.Visibility = Visibility.Visible;
                        inputFloat2.Visibility = Visibility.Visible;
                        inputFloat3.Visibility = Visibility.Visible;
                        inputFloat4.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Color:
                        inputColor.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Boolean:
                        inputBoolean.Visibility = Visibility.Visible;
                        break;

                    case InputTypes.Varying:
                        inputVarying.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
        
        // Switch between input types
        private void menuInputType_Clicked(object sender, RoutedEventArgs e)
        {
            // Construct references to all context menu items
            ContextMenu contextMenu = Resources["menuInputType"] as ContextMenu;

            MenuItem menuItemInputLink      = contextMenu.Items[0] as MenuItem;
            MenuItem menuItemInputConstant  = contextMenu.Items[1] as MenuItem;
            MenuItem menuItemInputVarying   = contextMenu.Items[2] as MenuItem;

            MenuItem menuItemInputFloat1  = menuItemInputConstant.Items[0] as MenuItem;
            MenuItem menuItemInputFloat2  = menuItemInputConstant.Items[1] as MenuItem;
            MenuItem menuItemInputFloat3  = menuItemInputConstant.Items[2] as MenuItem;
            MenuItem menuItemInputFloat4  = menuItemInputConstant.Items[3] as MenuItem;
            MenuItem menuItemInputColor   = menuItemInputConstant.Items[4] as MenuItem;
            MenuItem menuItemInputBoolean = menuItemInputConstant.Items[5] as MenuItem;

            // If the clicked menu item was already checked we are done
            MenuItem clickedMenuItem = e.Source as MenuItem;

            if (clickedMenuItem.IsChecked)
                return;

            // Uncheck all other items
            menuItemInputLink.IsChecked = false;
            menuItemInputFloat1.IsChecked = false;
            menuItemInputFloat2.IsChecked = false;
            menuItemInputFloat3.IsChecked = false;
            menuItemInputFloat4.IsChecked = false;
            menuItemInputColor.IsChecked = false;
            menuItemInputBoolean.IsChecked = false;
            menuItemInputVarying.IsChecked = false;

            clickedMenuItem.IsChecked = true;

            // Change input type
            if (clickedMenuItem == menuItemInputLink) {
                InputType = InputTypes.Link;
            } else if (clickedMenuItem == menuItemInputFloat1) {
                InputType = InputTypes.Float1;
            } else if (clickedMenuItem == menuItemInputFloat2) {
                InputType = InputTypes.Float2;
            } else if (clickedMenuItem == menuItemInputFloat3) {
                InputType = InputTypes.Float3;
            } else if (clickedMenuItem == menuItemInputFloat4) {
                InputType = InputTypes.Float4;
            } else if (clickedMenuItem == menuItemInputColor) {
                InputType = InputTypes.Color;
            } else if (clickedMenuItem == menuItemInputBoolean) {
                InputType = InputTypes.Boolean;
            } else if (clickedMenuItem == menuItemInputVarying) {
                InputType = InputTypes.Varying;
            }
        }

        // Connections made to the link
        private HashSet<Connection> links = new HashSet<Connection>() ;

        public void AddLink(Connection link)
        {
            links.Add(link);

            // Change color of link ellipse
            inputLink.Fill = Brushes.Black;
        }

        public void RemoveLink(Connection link)
        {
            links.Remove(link);

            // Change color of link ellipse if all connections have been removed
            if (links.Count == 0)
                inputLink.Fill = Brushes.DarkGray;
        }

        public List<Connection> GetLinks()
        {
            return links.ToList();
        }

        // Link point coordinates relative to this variable control
        public Point LinkPoint
        {
            get
            {
                return inputLink.TranslatePoint(new Point(5, 5), this);
            }
        }

        // Apply user enforced type constraints
        private void MenuItem_CheckedChanged(object sender, RoutedEventArgs e)
        {
            string typeText = "";

            if (typeMenuFloat1 != null && typeMenuFloat1.IsChecked)
                typeText += "1 ";
            if (typeMenuFloat2 != null && typeMenuFloat2.IsChecked)
                typeText += "2 ";
            if (typeMenuFloat3 != null && typeMenuFloat3.IsChecked)
                typeText += "3 ";
            if (typeMenuFloat4 != null && typeMenuFloat4.IsChecked)
                typeText += "4 ";
            if (typeMenuBoolean != null && typeMenuBoolean.IsChecked)
                typeText += "b ";

            ContentTypeIdentifiers.Content = typeText.Trim();
        }

        public bool IsFloat4() 
        {
            return typeMenuFloat4.IsChecked;
        }

        public bool IsFloat3()
        {
            return typeMenuFloat3.IsChecked;
        }

        public bool IsFloat2()
        {
            return typeMenuFloat2.IsChecked;
        }

        public bool IsFloat1()
        {
            return typeMenuFloat1.IsChecked;
        }

        // Start creating a connection when mouse down on the input link,
        // or remove a connection when mouse down on input link
        private void ContentInputTypes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Type == VariableType.Output)
            {
                if (e.ChangedButton == MouseButton.Left)
                    Connection.startCreation(this);
            }
            else if (Type == VariableType.Input && GetLinks().Count != 0)
            {
                if (e.ChangedButton == MouseButton.Left)
                    GetLinks()[0].breakConnection();
            }
        }
    }
}
