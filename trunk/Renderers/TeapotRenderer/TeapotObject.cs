using System.Collections.Generic;
using System.Linq;
using System.Text;

using TeapotRenderer.Camera;

using SlimDX;
using Device = SlimDX.Direct3D11.Device;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using System.IO;

namespace TeapotRenderer
{
    class TeapotObject : ICameraTarget
    {
        Device renderingDevice;
        TeapotRenderer renderer;

        Effect effect;
        EffectMatrixVariable effectViewProjection;
        EffectVectorVariable effectCameraPositionWorld;
        EffectTechnique technique;
        EffectPass pass;
        InputLayout inputLayout;
        Buffer vertexBuffer;
        Buffer indexBuffer;

        int indexCount;

        public TeapotObject(TeapotRenderer renderer, Device renderingDevice)
        {
            this.renderingDevice = renderingDevice;
            this.renderer = renderer;

            // Default position
            Position = new Vector3(0, 20, 0);

            // Create shader
            SetSourceCode(" float4 getColor(float4 position, float4 normal, float4 camera) { return float4(0, 0, 0, 0); } ");

            // Read data from file
            Vector3[] vertices; short[] indices;

            new XLoader().LoadFile(renderer.Directory + "\\Resources\\Teapot.X" , out vertices, out indices);

            //
            DataStream vertexStream = new DataStream(vertices.Length * 12, true, true);
            vertexStream.WriteRange(vertices);
            vertexStream.Position = 0;

            DataStream indexStream = new DataStream(indices.Length * 2, true, true);
            indexStream.WriteRange(indices);
            indexStream.Position = 0;
            
            //
            BufferDescription vbufferDescription = new BufferDescription();
            vbufferDescription.BindFlags = BindFlags.VertexBuffer;
            vbufferDescription.CpuAccessFlags = CpuAccessFlags.None;
            vbufferDescription.OptionFlags = ResourceOptionFlags.None;
            vbufferDescription.SizeInBytes = (int)vertexStream.Length;
            vbufferDescription.Usage = ResourceUsage.Immutable;

            vertexBuffer = new Buffer(renderingDevice, vertexStream, vbufferDescription);

            //
            BufferDescription ibufferDescription = new BufferDescription();
            ibufferDescription.BindFlags = BindFlags.IndexBuffer;
            ibufferDescription.CpuAccessFlags = CpuAccessFlags.None;
            ibufferDescription.OptionFlags = ResourceOptionFlags.None;
            ibufferDescription.SizeInBytes = (int)indexStream.Length;
            ibufferDescription.Usage = ResourceUsage.Immutable;
            
            indexBuffer = new Buffer(renderingDevice, indexStream, ibufferDescription);
            indexCount = (int)indexStream.Length / 2;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public void Render(ThirdPersonCamera camera, int ellapsedTime)
        {
            if (pass == null)
                return;

            effectViewProjection.SetMatrix(camera.ViewMatrix * camera.ProjectionMatrix);
            effectCameraPositionWorld.Set(camera.Position);

            renderingDevice.ImmediateContext.InputAssembler.InputLayout = inputLayout;
            renderingDevice.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            
            renderingDevice.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, 24, 0));
            renderingDevice.ImmediateContext.InputAssembler.SetIndexBuffer(indexBuffer, Format.R16_UInt, 0);

            pass.Apply(renderingDevice.ImmediateContext);
            renderingDevice.ImmediateContext.DrawIndexed(indexCount, 0, 0);
        }

        public void SetSourceCode(string extendedSourceCode)
        {
            string shaderCode = File.ReadAllText(renderer.Directory + "\\Resources\\Teapot.fx");

            try
            {
                using (ShaderBytecode bytecode = ShaderBytecode.Compile(extendedSourceCode + shaderCode, null, "fx_5_0", ShaderFlags.None, EffectFlags.None))
                {
                    effect = new Effect(renderingDevice, bytecode);
                }
            }
            catch (CompilationException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return;
            }

            effectViewProjection = effect.GetVariableByName("matViewProjection").AsMatrix();
            effectCameraPositionWorld = effect.GetVariableByName("cameraPositionWorld").AsVector();

            technique = effect.GetTechniqueByIndex(0);
            pass = technique.GetPassByIndex(0);

            ShaderSignature signature = pass.Description.Signature;

            inputLayout = new InputLayout(renderingDevice, signature, new[] {
				new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32_Float, 0, 0),
				new InputElement("NORMAL", 0, SlimDX.DXGI.Format.R32G32B32_Float, 12, 0) 
			});
        }
    }
}
