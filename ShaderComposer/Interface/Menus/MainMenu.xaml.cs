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

        private void ToolbarsMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void StandardToolbar_CheckedChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void NodesToolbar_CheckedChanged(object sender, RoutedEventArgs e)
        {

        }
        
        //
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
                FilesManager.Instance.ActiveFile.ActiveState.AddNewNode(node);
            }
        }

        //
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
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.Build();
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
    }
}
