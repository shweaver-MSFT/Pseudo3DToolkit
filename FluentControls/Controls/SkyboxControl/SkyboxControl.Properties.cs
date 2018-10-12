using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    public partial class SkyboxControl : ContentControl
    {
        public static readonly DependencyProperty AutoRotateProperty = DependencyProperty.Register(nameof(AutoRotate), typeof(bool), typeof(SkyboxControl), new PropertyMetadata(false, OnAutoRotateChanged));

        public static readonly DependencyProperty ShowNavigationControlsProperty = DependencyProperty.Register(nameof(ShowNavigationControls), typeof(bool), typeof(SkyboxControl), new PropertyMetadata(false, OnShowNavigationControlsChanged));

        public bool AutoRotate
        {
            get { return (bool)GetValue(AutoRotateProperty); }
            set { SetValue(AutoRotateProperty, value); }
        }

        public bool ShowNavigationControls
        {
            get { return (bool)GetValue(ShowNavigationControlsProperty); }
            set { SetValue(ShowNavigationControlsProperty, value); }
        }

        private static void OnAutoRotateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnShowNavigationControlsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }
    }
}
