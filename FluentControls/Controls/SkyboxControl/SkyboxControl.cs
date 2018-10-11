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
    [TemplatePart(Name = CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    public partial class SkyboxControl : ContentControl
    {
        // Template part names
        private const string CAMERACONTROL_NAME = "MyCamera";
        private const string CONTENTPRESENTER_NAME = "MyContent";

        // Defaults
        private readonly float _skyboxSize = 90000;
        private readonly Vector3 _cameraPosition = new Vector3(0, 0, 1000);
        private readonly Vector3 _rotationAxisX = new Vector3(1, 0, 0);
        private readonly Vector3 _rotationAxisY = new Vector3(0, 1, 0);

        // Template parts
        private CameraControl _cameraControl;
        private ContentPresenter _contentPresenter;

        // Other fields
        private Compositor _compositor;
        private SurfaceFactory _surfaceFactory;
        private ContainerVisual _skyboxContainer;

        public SkyboxControl()
        {
            DefaultStyleKey = typeof(SkyboxControl);
        }

        public void Rotate(float value, float duration = 0)
        {
            ScalarKeyFrameAnimation rotateAnimation = _compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, value, _compositor.CreateLinearEasingFunction());
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            rotateAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            _skyboxContainer.StartAnimation("RotationAngleInDegrees", rotateAnimation);
        }

        protected override void OnApplyTemplate()
        {
            SetupCameraControl();
            SetupContentPresenter();

            base.OnApplyTemplate();
        }

        private void SetupContentPresenter()
        {
            if (_contentPresenter != null)
            {

            }

            _contentPresenter = (ContentPresenter)GetTemplateChild(CONTENTPRESENTER_NAME);

            if (_contentPresenter != null)
            {

            }
        }

        private void SetupCameraControl()
        {
            // Unregister
            if (_cameraControl != null)
            {
                SizeChanged -= OnSizeChanged;
            }

            // Get template part
            _cameraControl = (CameraControl)GetTemplateChild(CAMERACONTROL_NAME);

            // Bail if missing
            if (_cameraControl == null) return;
            
            SizeChanged += OnSizeChanged;

            // Camera setup
            _cameraControl.ViewportSize = RenderSize.ToVector2();
            _cameraControl.Position = _cameraPosition;
            _cameraControl.SetAsPerspective(RenderSize.ToVector2());
            _cameraControl.PerspectiveDistance = (float)(RenderSize.Height + RenderSize.Width) / 3;

            // ImageLoader
            _compositor = _cameraControl.CompositionCamera.CameraVisual.Compositor;
            _surfaceFactory = SurfaceFactory.GetSharedSurfaceFactoryForCompositor(_compositor);

            // Skybox container
            var halfSkyboxSize = _skyboxSize / 2;
            var negativeHalfSkyboxSize = -_skyboxSize / 2;
            _skyboxContainer = _compositor.CreateContainerVisual();
            _skyboxContainer.CenterPoint = new Vector3(halfSkyboxSize, halfSkyboxSize, halfSkyboxSize);
            _skyboxContainer.AnchorPoint = new Vector2(halfSkyboxSize, halfSkyboxSize);
            _skyboxContainer.Offset = new Vector3(negativeHalfSkyboxSize, negativeHalfSkyboxSize, negativeHalfSkyboxSize);
            _skyboxContainer.RotationAxis = _rotationAxisY;
            _skyboxContainer.BorderMode = CompositionBorderMode.Hard;
            _skyboxContainer.Comment = "Skybox";

            // Skybox sides
            SpriteVisual skyboxTop = SetupSkyboxSide(new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsTop.jpg"), "SkyboxTop");
            SpriteVisual skyboxLeft = SetupSkyboxSide(new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsLeft.jpg"), "SkyboxLeft");
            SpriteVisual skyboxRight = SetupSkyboxSide(new Vector3(_skyboxSize, 0, 0), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsRight.jpg"), "SkyboxRight");
            SpriteVisual skyboxBottom = SetupSkyboxSide(new Vector3(0, _skyboxSize, 0), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsBottom.jpg"), "SkyboxBottom");
            SpriteVisual skyboxFront = SetupSkyboxSide(new Vector3(0, 0, 0), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsFront.jpg"), "SkyboxFront");
            SpriteVisual skyboxBack = SetupSkyboxSide(new Vector3(0, 0, _skyboxSize), new Uri("ms-appx:///Pseudo3DToolkit/Assets/Skybox/Clouds/CloudsBack.jpg"), "SkyboxBack");

            skyboxTop.RotationAxis = _rotationAxisX;
            skyboxLeft.RotationAxis = _rotationAxisY;
            skyboxRight.RotationAxis = _rotationAxisY;
            skyboxBottom.RotationAxis = _rotationAxisX;

            skyboxTop.RotationAngleInDegrees = -90;
            skyboxLeft.RotationAngleInDegrees = 90;
            skyboxRight.RotationAngleInDegrees = -90;
            skyboxBottom.RotationAngleInDegrees = 90;

            // World root
            SpriteVisual treeRoot = _compositor.CreateSpriteVisual();
            SpriteVisual worldRoot = _compositor.CreateSpriteVisual();
            treeRoot.Comment = "TreeRoot";
            worldRoot.Comment = "WorldRoot";

            ElementCompositionPreview.SetElementChildVisual(_cameraControl, treeRoot);
            treeRoot.Children.InsertAtTop(worldRoot);
            worldRoot.Children.InsertAtTop(_skyboxContainer);

            // TODO: Use attached properties to get default values
            _cameraControl.Yaw = 0;
            _cameraControl.Pitch = 0;
            _cameraControl.PerspectiveDistance = 575;
            _cameraControl.Position = new Vector3(960, 540, 0);

            if (AutoRotate)
            {
                // Animate for fun.
                Rotate(360, 100000);
            }
        }

        private SpriteVisual SetupSkyboxSide(Vector3 offset, Uri image, string comment)
        {
            SpriteVisual side = _compositor.CreateSpriteVisual();
            side.BorderMode = CompositionBorderMode.Hard;
            side.Size = new Vector2(_skyboxSize, _skyboxSize);
            side.Offset = offset;
            side.Brush = _compositor.CreateSurfaceBrush(_surfaceFactory.CreateUriSurface(image).Surface);
            side.Comment = comment;

            _skyboxContainer.Children.InsertAtTop(side);
            return side;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _cameraControl.ViewportSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
        }
    }
}
