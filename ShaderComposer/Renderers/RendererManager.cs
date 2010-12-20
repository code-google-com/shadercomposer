using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Reflection;

namespace ShaderComposer.Renderers
{
    public delegate void RendererAddedHandler(object sender, IRenderer library);

    public class RendererManager
    {
        // Singleton code
        private static RendererManager instance;

        public static RendererManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new RendererManager();

                return instance;
            }
        }

        private string baseDirectory;

        private RendererManager()
        {
            baseDirectory = Directory.GetCurrentDirectory();
        }

        //
        private List<IRenderer> renderers = new List<IRenderer>();

        public ReadOnlyCollection<IRenderer> Renderers
        {
            get
            {
                return new ReadOnlyCollection<IRenderer>(renderers);
            }
        }

        // Changed event
        public event RendererAddedHandler RendererAdded;

        protected virtual void OnRendererAdded(IRenderer renderer)
        {
            if (RendererAdded != null)
                RendererAdded(this, renderer);
        }

        //
        public void LoadDefaultRenderers()
        {
            // Search for all libraries in the default directory
            string searchDirectory = Path.Combine(baseDirectory, "Renderers");

            DirectoryInfo directoryInfo = new DirectoryInfo(searchDirectory);

            try
            {
                FileInfo[] filesFound = directoryInfo.GetFiles("*.dll");

                foreach (FileInfo fileInfo in filesFound)
                {
                    LoadRenderer(fileInfo.FullName);
                }
            }
            catch
            {
                MessageBox.Show("Failed to enumerate renderers.", "Error");
            }
        }

        public void LoadRenderer(string fileName)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName);

                Assembly loadedAssembly = Assembly.LoadFrom(fileName);

                foreach (IRenderer renderer in from type in loadedAssembly.GetTypes()
                                               where type.GetInterfaces().Contains(typeof(IRenderer))
                                               select type.GetConstructor(new Type[1] { typeof(string) }).Invoke(new Object[] { directoryName }) as IRenderer)
                {
                    renderers.Add(renderer);
                    OnRendererAdded(renderer);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load renderer: " + Path.GetFileName(fileName), "Error");
            }
        }


    }
}
