using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.Windows;

using Device = SlimDX.Direct3D11.Device;
using TeapotRenderer.SlimDX2;
using TeapotRenderer.Camera;

namespace TeapotRenderer
{
    class TeapotScene : ISlimDXScene
    {
        Device renderingDevice;
        DepthStencilState depthStencilState;
        DepthStencilView depthStencilView;
        RenderTargetView renderTargetView;

        ThirdPersonCamera camera;

        TeapotObject teapotObject;

        public TeapotScene(TeapotRenderer teapotRenderer)
        {
            // Create a description of the display mode
            ModeDescription modeDescription = new ModeDescription()
            {
                Format = Format.R8G8B8A8_UNorm,
                RefreshRate = new Rational(60, 1),
                
                Width = 512,
                Height = 512
            };

            // Create a description of the multisampler
            SampleDescription sampleDescription = new SampleDescription()
            {
                Count = 1,
                Quality = 0
            };

            // Create a description of the swap chain
            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                ModeDescription = modeDescription,
                SampleDescription = sampleDescription,

                BufferCount = 1,
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput,

                IsWindowed = false
            };

            // Create a hardware accelarated rendering device
            renderingDevice = new Device(DriverType.Hardware, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport);

            // Create the shared texture
            Texture2DDescription colordesc = new Texture2DDescription();
            colordesc.BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource;
            colordesc.Format = Format.B8G8R8A8_UNorm;
            colordesc.Width = 512;
            colordesc.Height = 512;
            colordesc.MipLevels = 1;
            colordesc.SampleDescription = new SampleDescription(1, 0);
            colordesc.Usage = ResourceUsage.Default;
            colordesc.OptionFlags = ResourceOptionFlags.Shared;
            colordesc.CpuAccessFlags = CpuAccessFlags.None;
            colordesc.ArraySize = 1;

            SharedTexture = new Texture2D(renderingDevice, colordesc);

            // Create the render target view
            renderTargetView = new RenderTargetView(renderingDevice, SharedTexture);

            // Creat the depth/stencil buffer
            DepthStencilStateDescription depthStencilStateDescription = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
            };

            depthStencilState = DepthStencilState.FromDescription(renderingDevice, depthStencilStateDescription);

            Texture2DDescription depthStencilTextureDescription = new Texture2DDescription()
            {
                Width = 512,
                Height = 512,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D32_Float,
                SampleDescription = sampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            DepthStencilViewDescription depthStencilViewDescription = new DepthStencilViewDescription()
            {
                Format = depthStencilTextureDescription.Format,
                Dimension = depthStencilTextureDescription.SampleDescription.Count > 1 ? DepthStencilViewDimension.Texture2DMultisampled : DepthStencilViewDimension.Texture2D,
                MipSlice = 0
            };

            using (Texture2D depthStencilTexture = new Texture2D(renderingDevice, depthStencilTextureDescription))
            {
                depthStencilView = new DepthStencilView(renderingDevice, depthStencilTexture, depthStencilViewDescription);
            }

            // Setup the default output targets
            renderingDevice.ImmediateContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);

            // Setup the viewport
            Viewport viewPort = new Viewport()
            {
                X = 0,
                Y = 0,
                Width = 512,
                Height = 512,
                MinZ = 0,
                MaxZ = 1
            };

            renderingDevice.ImmediateContext.Rasterizer.SetViewports(viewPort);

            // Create the teappot
            teapotObject = new TeapotObject(teapotRenderer, renderingDevice);

            // Create the camera
            camera = new ThirdPersonCamera(teapotObject);
            
        }

        public void SetSourceCode(string sourceCode)
        {
            teapotObject.SetSourceCode(sourceCode);
        }

        public Texture2D SharedTexture
        {
            get;
            set;
        }

        public void Dispose()
        {
            // Dispose all resources
            depthStencilState.Dispose();

            depthStencilView.Dispose();
            renderTargetView.Resource.Dispose();
            renderTargetView.Dispose();

            renderingDevice.Dispose();
        }

        public void Render(int ellapsedTime)
        {
            // Clear the output targets
            renderingDevice.ImmediateContext.ClearRenderTargetView(renderTargetView, new Color4(1, 256, 0, 256));
            renderingDevice.ImmediateContext.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1, 0);

            // Update the camera
            camera.Update(ellapsedTime);

            // Render the teapot
            teapotObject.Render(camera, ellapsedTime);

            renderingDevice.ImmediateContext.Flush();
        }

    }
}
