using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace TeapotRenderer.Camera
{
    class ThirdPersonCamera
    {
        ICameraTarget target;

        public ThirdPersonCamera(ICameraTarget target)
        {
            this.target = target;

            // Initialize values for the projection matrix
            fieldOfView = (float)Math.PI / 4.0f;

            nearDistance = 0.5f;
            farDistance = 100000.0f;

            updateProjectionMatrix();

            // Initialize values for the viewing matrix
            minZoom = 1.0f;
            maxZoom = 5000.0f;

            zoom = 100.0f;

            rotationXZ = 0.0f;

            minRotationY = -(float)Math.PI / 2.0f + 0.1f;
            maxRotationY = +(float)Math.PI / 2.0f - 0.1f;
            rotationY = -(float)Math.PI / 4.0f;

            updateViewMatrix();
        }

        public void Update(int elapsedTime)
        {
            LookAt = target.Position;

            rotationXZ += elapsedTime / 50000.0f;
        }

        #region View

        //
        public Vector3 Position
        {
            get
            {
                Vector3 viewingDirection3 = new Vector3(1, 0, 0);

                Vector4 viewingDirection4 = Vector3.Transform(viewingDirection3, Matrix.RotationYawPitchRoll(RotationXZ, 0, RotationY));

                viewingDirection3 = new Vector3(viewingDirection4.X, viewingDirection4.Y, viewingDirection4.Z);

                viewingDirection3 *= Zoom;

                Vector3 position = LookAt - viewingDirection3;

                return position;
            }

        }

        //
        public Vector3 LookAt
        {
            get { return lookAt; }
            set
            {
                lookAt = value;
                updateViewMatrix();
            }
        }

        private Vector3 lookAt;

        //
        public float RotationXZ
        {
            get { return rotationXZ; }

            set
            {
                rotationXZ = value;
                updateViewMatrix();
            }
        }

        private float rotationXZ;

        //
        public float minRotationY { get; set; }
        public float maxRotationY { get; set; }

        public float RotationY
        {
            get { return rotationY; }

            set
            {
                rotationY = Math.Min(Math.Max(value, minRotationY), maxRotationY);
                updateViewMatrix();
            }
        }

        private float rotationY;

        //
        public float minZoom { get; set; }
        public float maxZoom { get; set; }

        public float Zoom
        {
            get { return zoom; }

            set
            {
                zoom = Math.Min(Math.Max(value, minZoom), maxZoom);
                updateViewMatrix();
            }
        }

        private float zoom;

        //
        public Matrix ViewMatrix { get; private set; }

        private void updateViewMatrix()
        {
            ViewMatrix = Matrix.LookAtLH(Position, LookAt, Vector3.UnitY);
        }

        #endregion

        #region Projection

        //
        public float FieldOfView
        {
            get { return fieldOfView; }

            set
            {
                fieldOfView = value;
                updateProjectionMatrix();
            }
        }

        private float fieldOfView;

        //
        public float aspectRatio
        {
            get
            {
                return 256.0f / 256.0f; // (float)window.ClientSize.Width / (float)window.ClientSize.Height;
            }
        }

        //
        public float NearDistance
        {
            get { return nearDistance; }

            set
            {
                nearDistance = value;
                updateProjectionMatrix();
            }
        }

        private float nearDistance;

        //
        public float FarDistance
        {
            get { return farDistance; }

            set
            {
                farDistance = value;
                updateProjectionMatrix();
            }
        }

        private float farDistance;

        //
        public Matrix ProjectionMatrix { get; private set; }

        private void updateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.PerspectiveFovLH(FieldOfView, aspectRatio, NearDistance, FarDistance);
        }

        #endregion

    }
}
