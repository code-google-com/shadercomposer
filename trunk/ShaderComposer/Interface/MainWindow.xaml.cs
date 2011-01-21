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

using ShaderComposer.Interface.Menus;
using ShaderComposer.FileManagers;
using ShaderComposer.Libraries;
using ShaderComposer.Renderers;

namespace ShaderComposer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //
            Instance = this;

            // Initialize the library manager
            LibraryManager.Instance.LoadDefaultLibraries();

            // Initialize the renderer manager
            RendererManager.Instance.LoadDefaultRenderers();

            // Initialize the files manager
            FilesManager.Instance.TabCollection = FilesTabCollection;
        }

        public static MainWindow Instance;

        //
        private void Command_New(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.New();
        }

        private void Command_Open(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.Open();
        }

        private void Command_Save(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.Save();
        }

        private void Command_CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesManager.Instance.ActiveFile != null && FilesManager.Instance.ActiveFile.IsChanged);
            e.Handled = true;
        }

        private void Command_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.SaveAs();
        }

        private void Command_CanSaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesManager.Instance.ActiveFile != null);
            e.Handled = true;
        }

        private void Command_Close(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.Close();
        }

        private void Command_CanClose(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesManager.Instance.ActiveFile != null);
            e.Handled = true;
        }

        //
        private void Command_Stop(object sender, ExecutedRoutedEventArgs e)
        {
            FilesManager.Instance.CloseAll();

            Application.Current.Shutdown();
        }

        //
        private void Command_Cut(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_CanCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void Command_Copy(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_CanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void Command_Paste(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void Command_Delete(object sender, ExecutedRoutedEventArgs e)
        {

        }
        
        private void Command_CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        //
        private void Command_Undo(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private void Command_Redo(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        //
        private void Command_IncreaseZoom(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_DecreaseZoom(object sender, ExecutedRoutedEventArgs e)
        {

        }

    }
}
