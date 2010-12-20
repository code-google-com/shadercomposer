using System;
using System.Windows.Forms;
using DXGI = SlimDX.DXGI;
using D3D9 = SlimDX.Direct3D9;

using SlimDX.Direct3D10;
using SlimDX;

namespace TerrainRenderer
{
    class XLoader : IDisposable
    {
        public XLoader()
        {
            CreateNullDevice();
        }

        #region IDisposable
        ~XLoader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposeManagedObjects)
        {
            if (disposeManagedObjects)
            {
                device9.Dispose();
                form.Dispose();
            }
        }
        #endregion

        public void LoadFile(string filename, out Vector3[] vertexBuffer, out int[] indexBuffer)
        {
            using (var mesh9 = D3D9.Mesh.FromFile(device9, filename, D3D9.MeshFlags.SystemMemory))
            {
                // Read vertex buffer
                vertexBuffer = new Vector3[mesh9.VertexCount * 2];

                DataStream vertexStream = mesh9.VertexBuffer.Lock(0, mesh9.VertexCount * mesh9.BytesPerVertex, D3D9.LockFlags.ReadOnly);

                vertexStream.ReadRange(vertexBuffer, 0, mesh9.VertexCount * 2);
                
                vertexStream.Close();

                // Read index buffer
                indexBuffer = new int[mesh9.IndexBuffer.Description.SizeInBytes / 4];

                DataStream indexStream = mesh9.IndexBuffer.Lock(0, mesh9.IndexBuffer.Description.SizeInBytes, D3D9.LockFlags.ReadOnly);

                indexStream.ReadRange(indexBuffer, 0, mesh9.IndexBuffer.Description.SizeInBytes / 4);

                indexStream.Close();
            }
        }

        #region Implementation Details
    
        private void CreateNullDevice()
        {
            form = new Form();
            using (var direct3D = new D3D9.Direct3D())
                device9 = new D3D9.Device(direct3D, 0, D3D9.DeviceType.Reference, form.Handle, D3D9.CreateFlags.SoftwareVertexProcessing, new D3D9.PresentParameters
                {
                    BackBufferCount = 1,
                    BackBufferFormat = D3D9.Format.X8R8G8B8,
                    BackBufferHeight = 512,
                    BackBufferWidth = 512,
                    SwapEffect = D3D9.SwapEffect.Discard,
                    Windowed = true
                });
        }

        private Form form;
        private D3D9.Device device9;

        #endregion
    }
}