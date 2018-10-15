using Robmikh.CompositionSurfaceFactory;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Pseudo3DToolkit.Controls
{
    [TemplatePart(Name = CAMERACONTROL_NAME, Type = typeof(CameraControl))]
    [TemplatePart(Name = CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    public sealed partial class StageControl : ContentControl
    {
        private enum StageSide
        {
            Backdrop,
            Floor
        }

        // Template part names
        private const string CAMERACONTROL_NAME = "MyCamera";
        private const string CONTENTPRESENTER_NAME = "MyContent";

        // Defaults
        private static readonly float _stageWidth = 1920;
        private static readonly float _stageFloorDepth = 1080;
        private static readonly float _stageBackdropHeight = 1080;
        private static readonly Vector3 _cameraPosition = new Vector3(0, 0, 1000);
        private static readonly Vector3 _rotationAxisX = new Vector3(1, 0, 0);
        private static readonly Vector3 _rotationAxisY = new Vector3(0, 1, 0);
        private static readonly string _defaultImage = "ms-appx:///Pseudo3DToolkit/Assets/Stage/Gridlines.png";

        // Template parts
        private CameraControl _cameraControl;
        private ContentPresenter _contentPresenter;

        // Other fields
        private Compositor _compositor;
        private SurfaceFactory _surfaceFactory;
        private ContainerVisual _stageContainer;

        public StageControl()
        {
            DefaultStyleKey = typeof(StageControl);
        }

        protected override void OnApplyTemplate()
        {
            SetupContentPresenter();
            SetupCameraControl();

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
            _cameraControl.SetAsPerspective(RenderSize.ToVector2());
            _cameraControl.Yaw = 0;
            _cameraControl.Pitch = 0;
            _cameraControl.PerspectiveDistance = 575;
            _cameraControl.Position = new Vector3(_stageWidth / 2, _stageBackdropHeight / 2, -1 * _stageFloorDepth / 2);

            // ImageLoader
            _compositor = _cameraControl.CompositionCamera.CameraVisual.Compositor;
            _surfaceFactory = SurfaceFactory.GetSharedSurfaceFactoryForCompositor(_compositor);

            // Stage container
            _stageContainer = _compositor.CreateContainerVisual();
            _stageContainer.CenterPoint = new Vector3(_stageWidth / 2, _stageBackdropHeight / 2, _stageFloorDepth / 2);
            _stageContainer.AnchorPoint = new Vector2(_stageWidth / 2, _stageBackdropHeight / 2);
            _stageContainer.Offset = new Vector3(-1 * _stageWidth / 2, -1 * _stageBackdropHeight * 0.75f, -1 * _stageFloorDepth / 4);
            _stageContainer.RotationAxis = _rotationAxisY;
            _stageContainer.BorderMode = CompositionBorderMode.Hard;
            _stageContainer.Comment = "Stage";

            // Backdrop + Floor
            SetupStageSide(StageSide.Backdrop);
            SetupStageSide(StageSide.Floor);

            // World root
            SpriteVisual treeRoot = _compositor.CreateSpriteVisual();
            SpriteVisual worldRoot = _compositor.CreateSpriteVisual();
            treeRoot.Comment = "TreeRoot";
            worldRoot.Comment = "WorldRoot";

            ElementCompositionPreview.SetElementChildVisual(_cameraControl, treeRoot);
            treeRoot.Children.InsertAtTop(worldRoot);
            worldRoot.Children.InsertAtTop(_stageContainer);
        }

        private SpriteVisual SetupStageSide(StageSide side)
        {
            SpriteVisual visual = _compositor.CreateSpriteVisual();
            Vector3 offset = Vector3.Zero;
            Vector2 size = Vector2.Zero;
            CompositionBrush brush = null;
            string comment = string.Empty;

            switch(side)
            {
                case StageSide.Backdrop:
                    offset = new Vector3(_stageWidth / 2, _stageBackdropHeight / 2, -1 * _stageFloorDepth);
                    size = new Vector2(_stageWidth, _stageBackdropHeight);
                    brush = _compositor.CreateSurfaceBrush(_surfaceFactory.CreateUriSurface(new Uri(BackgropImage), new Size(_stageWidth, _stageBackdropHeight)).Surface);
                    comment = "Backdrop";
                    break;

                case StageSide.Floor:
                    offset = new Vector3(_stageWidth / 2, _stageBackdropHeight * 1.5f, -1 * _stageFloorDepth);
                    size = new Vector2(_stageWidth, _stageFloorDepth);
                    brush = _compositor.CreateSurfaceBrush(_surfaceFactory.CreateUriSurface(new Uri(FloorImage), new Size(_stageWidth, _stageFloorDepth)).Surface);
                    comment = "Floor";
                    visual.RotationAngleInDegrees = 90f;
                    visual.RotationAxis = _rotationAxisX;
                    break;
            }

            visual.Offset = offset;
            visual.Size = size;
            visual.Brush = brush;
            visual.Comment = comment;

            _stageContainer.Children.InsertAtTop(visual);
            return visual;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _cameraControl.ViewportSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
        }
    }
}
