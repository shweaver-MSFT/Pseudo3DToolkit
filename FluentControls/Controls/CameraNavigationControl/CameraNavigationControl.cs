using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    [TemplatePart(Name = TEMPLATEPART_BUTTON_CENTER, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_LEFT, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_RIGHT, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_ABOVE, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_BELOW, Type = typeof(Button))]
    public class CameraNavigationControl : Control
    {
        private const string TEMPLATEPART_BUTTON_CENTER = "ButtonCenter";
        private const string TEMPLATEPART_BUTTON_LEFT = "ButtonLeft";
        private const string TEMPLATEPART_BUTTON_RIGHT = "ButtonRight";
        private const string TEMPLATEPART_BUTTON_ABOVE = "ButtonAbove";
        private const string TEMPLATEPART_BUTTON_BELOW = "ButtonBelow";

        private Button _centerButton;
        private Button _leftButton;
        private Button _rightButton;
        private Button _aboveButton;
        private Button _belowButton;

        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(nameof(Camera), typeof(CameraControl), typeof(CameraNavigationControl), new PropertyMetadata(null, OnCameraChanged));

        public CameraControl Camera
        {
            get => (CameraControl)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CameraNavigationControl cameraNav)
            {
                cameraNav?.ResetCamera();
            }
        }

        public CameraNavigationControl()
        {
            DefaultStyleKey = typeof(CameraNavigationControl);
        }

        protected override void OnApplyTemplate()
        {
            if (_centerButton != null)
            {
                _centerButton.Click -= OnCenterButton_Click;
                _centerButton.GotFocus -= OnCenterButton_Focus;
            }

            if (_leftButton != null)
            {
                _leftButton.Click -= OnCenterButton_Click;
                _leftButton.GotFocus -= OnLeftButton_Focus;
            }

            if (_rightButton != null)
            {
                _rightButton.Click -= OnCenterButton_Click;
                _rightButton.GotFocus -= OnRightButton_Focus;
            }

            if (_aboveButton != null)
            {
                _aboveButton.Click -= OnCenterButton_Click;
                _aboveButton.GotFocus -= OnAboveButton_Focus;
            }

            if (_belowButton != null)
            {
                _belowButton.Click -= OnCenterButton_Click;
                _belowButton.GotFocus -= OnBelowButton_Focus;
            }

            _centerButton = (Button)GetTemplateChild(TEMPLATEPART_BUTTON_CENTER);
            _leftButton = (Button)GetTemplateChild(TEMPLATEPART_BUTTON_LEFT);
            _rightButton = (Button)GetTemplateChild(TEMPLATEPART_BUTTON_RIGHT);
            _aboveButton = (Button)GetTemplateChild(TEMPLATEPART_BUTTON_ABOVE);
            _belowButton = (Button)GetTemplateChild(TEMPLATEPART_BUTTON_BELOW);

            if (_centerButton != null)
            {
                _centerButton.Click += OnCenterButton_Click;
                _centerButton.GotFocus += OnCenterButton_Focus;
            }

            if (_leftButton != null)
            {
                _leftButton.Click += OnCenterButton_Click;
                _leftButton.GotFocus += OnLeftButton_Focus;
            }

            if (_rightButton != null)
            {
                _rightButton.Click += OnCenterButton_Click;
                _rightButton.GotFocus += OnRightButton_Focus;
            }

            if (_aboveButton != null)
            {
                _aboveButton.Click += OnCenterButton_Click;
                _aboveButton.GotFocus += OnAboveButton_Focus;
            }

            if (_belowButton != null)
            {
                _belowButton.Click += OnCenterButton_Click;
                _belowButton.GotFocus += OnBelowButton_Focus;
            }

            base.OnApplyTemplate();
        }

        private void OnCenterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            ResetCamera();
            _centerButton.Focus(FocusState.Programmatic);
        }

        private void OnCenterButton_Focus(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            Camera.PerspectiveDistance = 575;
            ResetCamera();
        }

        private void OnLeftButton_Focus(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            Camera.PanCameraLeft();
        }

        private void OnRightButton_Focus(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            Camera.PanCameraRight();
        }

        private void OnAboveButton_Focus(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            Camera.PanCameraAbove();
        }

        private void OnBelowButton_Focus(object sender, RoutedEventArgs e)
        {
            if (Camera == null) return;
            Camera.PanCameraBelow();
        }

        private void ResetCamera()
        {
            if (Camera == null) return;
            Camera.CompositionCamera.RotateYaw(0, 1000);
            Camera.CompositionCamera.RotatePitch(0, 1000);
            Camera.CompositionCamera.TranslateX(960, 1000);
            Camera.CompositionCamera.TranslateY(540, 1000);
        }
    }
}
