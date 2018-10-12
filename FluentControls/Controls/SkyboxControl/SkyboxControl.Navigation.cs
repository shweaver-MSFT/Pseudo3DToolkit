using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    [TemplatePart(Name = TEMPLATEPART_BUTTON_CENTER, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_LEFT, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_RIGHT, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_ABOVE, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATEPART_BUTTON_BELOW, Type = typeof(Button))]
    public partial class SkyboxControl : ContentControl
    {
        const string TEMPLATEPART_BUTTON_CENTER= "ButtonCenter";
        const string TEMPLATEPART_BUTTON_LEFT = "ButtonLeft";
        const string TEMPLATEPART_BUTTON_RIGHT= "ButtonRight";
        const string TEMPLATEPART_BUTTON_ABOVE= "ButtonAbove";
        const string TEMPLATEPART_BUTTON_BELOW = "ButtonBelow";

        Button _centerButton;
        Button _leftButton;
        Button _rightButton;
        Button _aboveButton;
        Button _belowButton;

        private void SetupNavigationControls()
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
        }

        private void OnCenterButton_Click(object sender, RoutedEventArgs e)
        {
            ResetCamera();
            _centerButton.Focus(FocusState.Programmatic);
        }

        private void OnCenterButton_Focus(object sender, RoutedEventArgs e)
        {
            _cameraControl.PerspectiveDistance = 575;
            ResetCamera();
        }

        private void OnLeftButton_Focus(object sender, RoutedEventArgs e)
        {
            _cameraControl.PanCameraLeft();
        }

        private void OnRightButton_Focus(object sender, RoutedEventArgs e)
        {
            _cameraControl.PanCameraRight();
        }

        private void OnAboveButton_Focus(object sender, RoutedEventArgs e)
        {
            _cameraControl.PanCameraAbove();
        }

        private void OnBelowButton_Focus(object sender, RoutedEventArgs e)
        {
            _cameraControl.PanCameraBelow();
        }

        private void ResetCamera()
        {
            _cameraControl.CompositionCamera.RotateYaw(0);
            _cameraControl.CompositionCamera.RotatePitch(0);
            _cameraControl.CompositionCamera.TranslateX(960);
            _cameraControl.CompositionCamera.TranslateY(540);
        }
    }
}
