using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShaderComposer.Renderers;
using TerrainRenderer.SlimDX2;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace TerrainRenderer
{
    public class TerrainRenderer : IRenderer
    {
        public string GetName()
        {
            return "Terrain renderer";
        }

        //
        public string Directory { get; private set; }

        public TerrainRenderer(string directory)
        {
            Directory = directory;
        }

        // Creates a new Teapot renderer
        public IRenderer Create()
        {
            return new TerrainRenderer(Directory);
        }

        // Initializes the renderer
        SlimDXImage slimDXImage;
        ISlimDXScene slimDXScene;

        Stopwatch timer;

        public ImageSource Initialize()
        {
            slimDXImage = new SlimDXImage();
            slimDXImage.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            timer = new Stopwatch();

            // Start rendering
            slimDXScene = new TerrainScene(this);
            slimDXImage.SetBackBufferSlimDX(slimDXScene.SharedTexture);

            BeginRenderingScene();

            return slimDXImage;
        }

        public void SetSourceCode(string sourceCode)
        {
            (slimDXScene as TerrainScene).SetSourceCode(sourceCode);
        }

        void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // This fires when the screensaver kicks in, the machine goes into sleep or hibernate
            // and any other catastrophic losses of the d3d device from WPF's point of view
            if (slimDXImage.IsFrontBufferAvailable)
            {
                BeginRenderingScene();
            }
            else
            {
                StopRenderingScene();
            }
        }

        void OnRendering(object sender, EventArgs e)
        {
            if (slimDXScene != null)
            {
                slimDXScene.Render(timer.Elapsed.Milliseconds);
                slimDXImage.InvalidateD3DImage();

                OnSceneUpdated();
            }
        }

        void BeginRenderingScene()
        {
            if (slimDXImage.IsFrontBufferAvailable)
            {
                foreach (var item in SlimDX.ObjectTable.Objects) ;

                slimDXImage.SetBackBufferSlimDX(slimDXScene.SharedTexture);
                CompositionTarget.Rendering += OnRendering;

                timer.Start();
            }
        }

        void StopRenderingScene()
        {
            timer.Stop();
            CompositionTarget.Rendering -= OnRendering;
        }


        public string GetValueAt(double x, double y)
        {
            return slimDXImage.GetValueAt(x, y);
        }

        public event SceneUpdatedHandler SceneUpdated;

        void OnSceneUpdated()
        {
            if (SceneUpdated != null)
                SceneUpdated(this);
        }


        public void Destroy()
        {
        }
    }
}
