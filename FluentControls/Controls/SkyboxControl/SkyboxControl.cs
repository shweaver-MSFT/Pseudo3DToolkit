using Robmikh.CompositionSurfaceFactory;
using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Pseudo3DToolkit.Controls
{
    [TemplatePart(Name = CAMERACONTROL_NAME, Type = typeof(CameraControl))]
    public partial class SkyboxControl : ContentControl
    {
        // Control names
        private const string CAMERACONTROL_NAME = "MyCamera";

        // Template Parts
        private CameraControl _cameraControl;

        private Compositor _compositor;
        private SurfaceFactory _surfaceFactory;

        private readonly Vector3 _rotationAxisX = new Vector3(1, 0, 0);
        private readonly Vector3 _rotationAxisY = new Vector3(0, 1, 0);

        private float _skyboxSize = 90000;
        private ContainerVisual _skyboxContainer;
        private SpriteVisual _skyboxTop;
        private SpriteVisual _skyboxLeft;
        private SpriteVisual _skyboxRight;
        private SpriteVisual _skyboxBottom;
        private SpriteVisual _skyboxFront;
        private SpriteVisual _skyboxBack;

        public SkyboxControl()
        {
            InitializeComponent();
        }

        protected override void OnApplyTemplate()
        {
            SetupCameraControl();

            base.OnApplyTemplate();
        }

        private void SetupCameraControl()
        {
            if (_cameraControl != null)
            {
                SizeChanged -= OnSizeChanged;
            }

            _cameraControl = (CameraControl)GetTemplateChild(CAMERACONTROL_NAME);

            if (_cameraControl != null)
            {
                _compositor = _cameraControl.CompositionCamera.CameraVisual.Compositor;

                SizeChanged += OnSizeChanged;

                // World root
                var worldRoot = SetupWorldRoot();

                // Camera setup
                _cameraControl.ViewportSize = RenderSize.ToVector2();
                _cameraControl.Position = new Vector3(0, 0, 1000);
                _cameraControl.SetAsPerspective(RenderSize.ToVector2());
                _cameraControl.PerspectiveDistance = (float)(RenderSize.Height + RenderSize.Width) / 3;

                // ImageLoader
                _surfaceFactory = SurfaceFactory.GetSharedSurfaceFactoryForCompositor(_compositor);

                // Skybox
                var halfSkyboxSize = _skyboxSize / 2;
                var negativeHalfSkyboxSize = -_skyboxSize / 2;

                _skyboxContainer = _compositor.CreateContainerVisual();
                _skyboxContainer.CenterPoint = new Vector3(halfSkyboxSize, halfSkyboxSize, halfSkyboxSize);
                _skyboxContainer.AnchorPoint = new Vector2(halfSkyboxSize, halfSkyboxSize);
                _skyboxContainer.Offset = new Vector3(negativeHalfSkyboxSize, negativeHalfSkyboxSize, negativeHalfSkyboxSize);
                _skyboxContainer.RotationAxis = _rotationAxisY;
                _skyboxContainer.BorderMode = CompositionBorderMode.Hard;
                _skyboxContainer.Comment = "Skybox";

                SetupSkyboxSide(ref _skyboxTop, new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsTop.jpg"), "SkyboxTop");
                SetupSkyboxSide(ref _skyboxLeft, new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsLeft.jpg"), "SkyboxLeft");
                SetupSkyboxSide(ref _skyboxRight, new Vector3(_skyboxSize, 0, 0), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsRight.jpg"), "SkyboxRight");
                SetupSkyboxSide(ref _skyboxBottom, new Vector3(0, _skyboxSize, 0), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsBottom.jpg"), "SkyboxBottom");
                SetupSkyboxSide(ref _skyboxFront, new Vector3(0, 0, 0), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsFront.jpg"), "SkyboxFront");
                SetupSkyboxSide(ref _skyboxBack, new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Assets/Skybox/Clouds/CloudsBack.jpg"), "SkyboxBack");

                _skyboxTop.RotationAxis = _rotationAxisX;
                _skyboxLeft.RotationAxis = _rotationAxisY;
                _skyboxRight.RotationAxis = _rotationAxisY;
                _skyboxBottom.RotationAxis = _rotationAxisX;

                _skyboxTop.RotationAngleInDegrees = -90;
                _skyboxLeft.RotationAngleInDegrees = 90;
                _skyboxRight.RotationAngleInDegrees = -90;
                _skyboxBottom.RotationAngleInDegrees = 90;

                // Set up items in visual tree
                worldRoot.Children.InsertAtTop(_skyboxContainer);
            }
        }

        private void SetupSkyboxSide(ref SpriteVisual side, Vector3 offset, Uri image, string comment)
        {
            side = _compositor.CreateSpriteVisual();
            side.BorderMode = CompositionBorderMode.Hard;
            side.Size = new Vector2(_skyboxSize, _skyboxSize);
            side.Offset = offset;
            side.Brush = _compositor.CreateSurfaceBrush(_surfaceFactory.CreateUriSurface(image).Surface);
            side.Comment = comment;

            _skyboxContainer.Children.InsertAtTop(side);
        }

        private SpriteVisual SetupWorldRoot()
        {
            // World root
            SpriteVisual treeRoot = _compositor.CreateSpriteVisual();
            SpriteVisual worldRoot = _compositor.CreateSpriteVisual();
            worldRoot.Comment = "WorldRoot";

            ElementCompositionPreview.SetElementChildVisual(_cameraControl, treeRoot);
            treeRoot.Children.InsertAtTop(worldRoot);
            treeRoot.Comment = "TreeRoot";

            return worldRoot;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _cameraControl.ViewportSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
        }
    }
}
