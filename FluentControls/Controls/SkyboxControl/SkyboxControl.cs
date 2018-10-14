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
        private static readonly float _skyboxSize = 90000;
        private static readonly Vector3 _cameraPosition = new Vector3(0, 0, 1000);
        private static readonly Vector3 _rotationAxisX = new Vector3(1, 0, 0);
        private static readonly Vector3 _rotationAxisY = new Vector3(0, 1, 0);
        private static readonly string _defaultImage = "ms-appx:///Pseudo3DToolkit/Assets/Skybox/Gridlines.png";

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

            // TODO: Use attached properties to get default values
            _cameraControl.Yaw = 0;
            _cameraControl.Pitch = 0;
            _cameraControl.PerspectiveDistance = 575;
            _cameraControl.Position = new Vector3(960, 540, 0);

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
            SpriteVisual skyboxTop = SetupSkyboxSide(SkyboxSide.Top);
            SpriteVisual skyboxLeft = SetupSkyboxSide(SkyboxSide.Left);
            SpriteVisual skyboxRight = SetupSkyboxSide(SkyboxSide.Right);
            SpriteVisual skyboxBottom = SetupSkyboxSide(SkyboxSide.Bottom);
            SpriteVisual skyboxFront = SetupSkyboxSide(SkyboxSide.Front);
            SpriteVisual skyboxBack = SetupSkyboxSide(SkyboxSide.Back);

            // World root
            SpriteVisual treeRoot = _compositor.CreateSpriteVisual();
            SpriteVisual worldRoot = _compositor.CreateSpriteVisual();
            treeRoot.Comment = "TreeRoot";
            worldRoot.Comment = "WorldRoot";

            ElementCompositionPreview.SetElementChildVisual(_cameraControl, treeRoot);
            treeRoot.Children.InsertAtTop(worldRoot);
            worldRoot.Children.InsertAtTop(_skyboxContainer);

            if (AutoRotate)
            {
                // Animate for fun.
                Rotate(360, 100000);
            }
        }

        private SpriteVisual SetupSkyboxSide(SkyboxSide side)
        {
            SpriteVisual visual = _compositor.CreateSpriteVisual();
            CompositionBorderMode borderMode = CompositionBorderMode.Hard;
            Vector2 size = new Vector2(_skyboxSize, _skyboxSize);
            Vector3 offset = Vector3.Zero;
            string image = _defaultImage;
            string comment = string.Empty;

            switch (side)
            {
                case SkyboxSide.Top:
                    offset = new Vector3(0, 0, _skyboxSize);
                    image = TopImage;
                    comment = "SkyboxTop";
                    visual.RotationAngleInDegrees = -90;
                    visual.RotationAxis = _rotationAxisX;
                    break;
                case SkyboxSide.Bottom:
                    offset = new Vector3(0, _skyboxSize, 0);
                    image = BottomImage;
                    comment = "SkyboxBottom";
                    visual.RotationAngleInDegrees = 90;
                    visual.RotationAxis = _rotationAxisX;
                    break;
                case SkyboxSide.Left:
                    offset = new Vector3(0, 0, _skyboxSize);
                    image = LeftImage;
                    comment = "SkyboxLeft";
                    visual.RotationAngleInDegrees = 90;
                    visual.RotationAxis = _rotationAxisY;
                    break;
                case SkyboxSide.Right:
                    offset = new Vector3(_skyboxSize, 0, 0);
                    image = RightImage;
                    comment = "SkyboxRight";
                    visual.RotationAngleInDegrees = -90;
                    visual.RotationAxis = _rotationAxisY;
                    break;
                case SkyboxSide.Front:
                    offset = new Vector3(0, 0, 0);
                    image = FrontImage;
                    comment = "SkyboxFront";
                    break;
                case SkyboxSide.Back:
                    offset = new Vector3(0, 0, _skyboxSize);
                    image = BackImage;
                    comment = "SkyboxBack";
                    break;
            }

            visual.BorderMode = borderMode;
            visual.Size = size;
            visual.Offset = offset;
            visual.Brush = _compositor.CreateSurfaceBrush(_surfaceFactory.CreateUriSurface(new Uri(image)).Surface);
            visual.Comment = comment;

            _skyboxContainer.Children.InsertAtTop(visual);
            return visual;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _cameraControl.ViewportSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
        }
    }
}
