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

namespace ShaderComposer.Interface.Toolbars
{
    /// <summary>
    /// Interaction logic for ToolbarTray.xaml
    /// </summary>
    public partial class ToolbarTray : UserControl
    {
        public ToolbarTray()
        {
            InitializeComponent();
        }

        // Menu for hiding or showing toolbars
        private void ToolbarsMenu_Opened(object sender, RoutedEventArgs e)
        {
            MenuItem_StandardToolbar.IsChecked = MainWindow.Instance.ToolbarTray.StandardToolbar.IsVisible;
            MenuItem_NodesToolbar.IsChecked = MainWindow.Instance.ToolbarTray.NodesToolbar.IsVisible;
        }

        private void StandardToolbar_CheckedChanged(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ToolbarTray.StandardToolbar.Visibility = MenuItem_StandardToolbar.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NodesToolbar_CheckedChanged(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ToolbarTray.NodesToolbar.Visibility = MenuItem_NodesToolbar.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
