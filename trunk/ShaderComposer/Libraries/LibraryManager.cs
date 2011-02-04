using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;

namespace ShaderComposer.Libraries
{
    public delegate void LibraryAddedHandler(object sender, ILibrary library);

    public class LibraryManager
    {
        // Singleton code
        private static LibraryManager instance;

        public static LibraryManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LibraryManager();

                return instance;
            }
        }

        private string baseDirectory;

        private LibraryManager()
        {
            baseDirectory = Directory.GetCurrentDirectory();
        }

        //
        private List<ILibrary> libraries = new List<ILibrary>();

        public ReadOnlyCollection<ILibrary> Libraries
        {
            get
            {
                return new ReadOnlyCollection<ILibrary>(libraries);
            }
        }

        // Changed event
        public event LibraryAddedHandler LibraryAdded;

        protected virtual void OnLibraryAdded(ILibrary library)
        {
            if (LibraryAdded != null)
                LibraryAdded(this, library);
        }

        // 
        public void LoadDefaultLibraries()
        {
            // Search for all libraries in the default directory
            string searchDirectory = Path.Combine(baseDirectory, "Libraries");

            DirectoryInfo directoryInfo = new DirectoryInfo(searchDirectory);

            try
            {
                FileInfo[] filesFound = directoryInfo.GetFiles("*.dll");

                foreach (FileInfo fileInfo in filesFound)
                {
                    LoadLibrary(fileInfo.FullName);
                }
            }
            catch
            {
                MessageBox.Show("Failed to enumerate libraries.", "Error");
            }
        }

        public void LoadLibrary(string fileName)
        {
            try
            {
                Assembly loadedAssembly = Assembly.LoadFrom(fileName);

                foreach (ILibrary library in from type in loadedAssembly.GetTypes()
                                             where type.GetInterfaces().Contains(typeof(ILibrary))
                                             select type.GetConstructor(new Type[0]).Invoke(new object[0]) as ILibrary)
                {
                    libraries.Add(library);
                    OnLibraryAdded(library);
                }

            }
            catch
            {
                MessageBox.Show("Failed to load library: " + Path.GetFileName(fileName), "Error");
            }
        }

        public Type FindNode(string typeID)
        {
            foreach (ILibrary l in libraries)
            {
                foreach (Type t in l.GetNodeTypes())
                {
                    INode inode = t.GetConstructor(new Type[0]).Invoke(new Object[0]) as INode;

                    if (inode.GetIdentifier().ToString() == typeID)
                        return t;
                }
            }

            return null;
        }
    }
}
