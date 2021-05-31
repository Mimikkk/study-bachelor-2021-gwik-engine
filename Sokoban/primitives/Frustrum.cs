using Silk.NET.Maths;
using Sokoban.utilities;
using static Sokoban.utilities.FieldExtensions;

namespace Sokoban.primitives
{
    internal abstract class Frustum
    {
        protected Frustum(float fieldOfView, float aspectRatio, float nearDistance, float farDistance)
        {
            Fov = fieldOfView;
            AspectRatio = aspectRatio;
            NearDistance = nearDistance;
            FarDistance = farDistance;
        }

        private void RecalculateProjection()
        {
            ShouldRecalculateProjection = false;
            _projection = Matrix4X4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio,
                NearDistance, FarDistance);
        }

        public Matrix4X4<float> Projection
        {
            get => OperationIfConditionAndGet(_projection, ShouldRecalculateProjection, RecalculateProjection);
            set => _projection = value;
        }
        public float Fov
        {
            get => _fov;
            set => SetAndOperation(() => _fov = value, () => ShouldRecalculateProjection = true);
        }
        public float AspectRatio
        {
            get => _aspectRatio;
            set => SetAndOperation(() => _aspectRatio = value, () => ShouldRecalculateProjection = true);
        }
        public float NearDistance
        {
            get => _nearDistance;
            set => SetAndOperation(() => _nearDistance = value, () => ShouldRecalculateProjection = true);
        }
        public float FarDistance
        {
            get => _farDistance;
            set => SetAndOperation(() => _farDistance = value, () => ShouldRecalculateProjection = true);
        }

        private bool ShouldRecalculateProjection { get; set; }
        private Matrix4X4<float> _projection;
        private float _fov;
        private float _aspectRatio;
        private float _nearDistance;
        private float _farDistance;
    }
}