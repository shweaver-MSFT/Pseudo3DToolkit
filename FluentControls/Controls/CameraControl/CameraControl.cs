using Pseudo3DToolkit.Composition;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class CameraControl : UserControl
    {
        private static readonly float _cameraRotationDistance = 1.5708f; // 90 degrees = Math.Pi / 2
        private static readonly float _stageWidth = 1920;
        private static readonly float _stageFloorDepth = 1080;
        private static readonly float _stageBackdropHeight = 1080;
        private static readonly Vector3 _cameraPosition = new Vector3(0, 0, 1000);
        private static readonly Vector3 _rotationAxisX = new Vector3(1, 0, 0);
        private static readonly Vector3 _rotationAxisY = new Vector3(0, 1, 0);

        private ContainerVisual _containerVisual;

        public CompositionCamera CompositionCamera { get; private set; }

        public bool UseAnimations
        {
            get => CompositionCamera.UseAnimations;
            set => CompositionCamera.UseAnimations = value;
        }

        public bool IsOrthographic
        {
            get => CompositionCamera.IsOrthographic;
            set => CompositionCamera.IsOrthographic = value;
        }

        public float PerspectiveDistance
        {
            get => CompositionCamera.PerspectiveDistance;
            set => CompositionCamera.PerspectiveDistance = value;
        }

        public Vector2 ViewportSize
        {
            get => CompositionCamera.ViewportSize;
            set => CompositionCamera.ViewportSize = value;
        }

        public Vector3 Position
        {
            get => CompositionCamera.Position;
            set => CompositionCamera.Position = value;
        }

        public float Yaw
        {
            get => CompositionCamera.Yaw;
            set => CompositionCamera.Yaw = value;
        }

        public float Pitch
        {
            get => CompositionCamera.Pitch;
            set => CompositionCamera.Pitch = value;
        }

        public float Roll
        {
            get => CompositionCamera.Roll;
            set => CompositionCamera.Roll = value;
        }

        public CameraControl()
        {
            Visual cameraVisual = ElementCompositionPreview.GetElementVisual(this);
            CompositionCamera = new CompositionCamera(cameraVisual);
        }

        public void SetAsPerspective(Vector2 zPlaneSize)
        {
            ViewportSize = zPlaneSize;
            CompositionCamera.IsOrthographic = false;
        }

        public void SetAsOrthographic()
        {
            CompositionCamera.IsOrthographic = true;
        }

        public void PanCameraAbove()
        {
            CompositionCamera.RotatePitch(_cameraRotationDistance);
            CompositionCamera.RotateYaw(0);
        }

        public void PanCameraBelow()
        {
            CompositionCamera.RotatePitch(-_cameraRotationDistance);
            CompositionCamera.RotateYaw(0);
        }

        public void PanCameraLeft()
        {
            CompositionCamera.RotateYaw(-_cameraRotationDistance);
            CompositionCamera.RotatePitch(0);
        }

        public void PanCameraRight()
        {
            CompositionCamera.RotateYaw(_cameraRotationDistance);
            CompositionCamera.RotatePitch(0);
        }

        protected override void OnApplyTemplate()
        {
            var compositor = CompositionCamera.CameraVisual.Compositor;

            // Setup container visual
            _containerVisual = compositor.CreateContainerVisual();
            _containerVisual.CenterPoint = Position;//new Vector3(_stageWidth / 2, _stageBackdropHeight / 2, _stageFloorDepth / 2);
            _containerVisual.AnchorPoint = new Vector2(_stageWidth / 2, _stageBackdropHeight / 2);
            _containerVisual.Offset = new Vector3(-1 * _stageWidth / 2, -1 * _stageBackdropHeight * 0.75f, -1 * _stageFloorDepth / 4);
            _containerVisual.RotationAxis = _rotationAxisY;
            _containerVisual.BorderMode = CompositionBorderMode.Hard;
            _containerVisual.Comment = "VisualElements";

            // World root
            SpriteVisual treeRoot = compositor.CreateSpriteVisual();
            SpriteVisual worldRoot = compositor.CreateSpriteVisual();
            treeRoot.Comment = "TreeRoot";
            worldRoot.Comment = "WorldRoot";

            ElementCompositionPreview.SetElementChildVisual(this, treeRoot);
            treeRoot.Children.InsertAtTop(worldRoot);
            worldRoot.Children.InsertAtTop(_containerVisual);

            foreach (var element in VisualElements)
            {
                AddVisualElement(element);
            }

            base.OnApplyTemplate();
        }

        public void AddVisualElement(VisualElement element)
        {
            var visual = element.GetSpriteVisual(CompositionCamera.CameraVisual.Compositor);
            _containerVisual.Children.InsertAtTop(visual);
        }
    }
}
