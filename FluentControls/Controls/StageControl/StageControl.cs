using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    [TemplatePart(Name = CAMERACONTROL_NAME, Type = typeof(CameraControl))]
    [TemplatePart(Name = CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    public sealed partial class StageControl : ContentControl
    {
        // Template part names
        private const string CAMERACONTROL_NAME = "MyCamera";
        private const string CONTENTPRESENTER_NAME = "MyContent";

        // Template parts
        private CameraControl _cameraControl;
        private ContentPresenter _contentPresenter;

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

            }

            // Get template part
            _cameraControl = (CameraControl)GetTemplateChild(CAMERACONTROL_NAME);

            // Bail if missing
            if (_cameraControl == null) return;

            // Camera setup

        }
    }
}
