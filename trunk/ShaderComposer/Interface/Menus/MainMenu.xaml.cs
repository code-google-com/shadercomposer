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
using ShaderComposer.Libraries;
using ShaderComposer.Renderers;
using ShaderComposer.FileManagers;
using Microsoft.Win32;

namespace ShaderComposer.Interface.Menus
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();

            // Register for library added events
            LibraryManager.Instance.LibraryAdded += new LibraryAddedHandler(LibraryAdded);

            // Register for renderer added events
            RendererManager.Instance.RendererAdded += new RendererAddedHandler(RendererAdded);
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
        
        // Library added event
        private Dictionary<MenuItem, Type> nodeTypes = new Dictionary<MenuItem, Type>();

        private void LibraryAdded(object sender, ILibrary library)
        {
            MenuItem libraryMenuItem = new MenuItem();
            libraryMenuItem.Header = library.GetName();

            foreach (Type node in library.GetNodeTypes())
            {
                MenuItem libraryNodeItem = new MenuItem();
                libraryNodeItem.Header = node.Name;

                nodeTypes[libraryNodeItem] = node;
                libraryNodeItem.Click += new RoutedEventHandler(libraryNodeItem_Click);
                
                libraryMenuItem.Items.Add(libraryNodeItem);
            }

            Libraries.Items.Insert(Libraries.Items.Count-2, libraryMenuItem);
        }

        // Add a new node to the graph
        void libraryNodeItem_Click(object sender, RoutedEventArgs e)
        {
            Type node = nodeTypes[sender as MenuItem];

            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.AddNewNode(node, new Point(0, 0));
            }
        }

        // Renderer added event
        private void RendererAdded(object sender, IRenderer renderer)
        {
            MenuItem rendererMenuItem = new MenuItem();
            rendererMenuItem.Header = renderer.GetName();

            rendererMenuItem.IsCheckable = true;
            rendererMenuItem.Checked += new RoutedEventHandler((s, e) => rendererMenuItem_Checked(rendererMenuItem, renderer));

            Renderers.Items.Insert(Renderers.Items.Count - 2, rendererMenuItem);
        }

        // Change the renderer
        private void rendererMenuItem_Checked(MenuItem rendererMenuItem, IRenderer renderer)
        {
            // Uncheck all other items
            foreach (MenuItem item in Renderers.Items.OfType<MenuItem>())
            {
                if (item != rendererMenuItem)
                {
                    item.IsChecked = false;
                }
            }

            // Apply the renderer to the active file state
            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.Renderer = renderer;
            }
        }

        // Rebuild the shader from the tree
        private void MenuItem_Build_Click(object sender, RoutedEventArgs e)
        {
            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.Build();
                FilesManager.Instance.ActiveFile.ActiveState.BuildXML();
            }
        }

        // Create a new state
        private void NewState_Click(object sender, RoutedEventArgs e)
        {
            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.NewState();
            }
        }

        // Manually load a library from file
        private void MenuItem_LoadLibrary_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Node library file (.dll)|*.dll";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() ?? false)
            {
                foreach (string fileName in dialog.FileNames)
                {
                    LibraryManager.Instance.LoadLibrary(fileName);
                }
            }
        }

        // Manually load a renderer from file
        private void MenuItem_LoadRenderer_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Renderer (.dll)|*.dll";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() ?? false)
            {
                foreach (string fileName in dialog.FileNames)
                {
                    RendererManager.Instance.LoadRenderer(fileName);
                }
            }
        }

    }
}
