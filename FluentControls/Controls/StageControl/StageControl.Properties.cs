using Windows.UI.Xaml;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class StageControl
    {
        public static readonly DependencyProperty NavigationControlsVisibilityProperty = DependencyProperty.Register(nameof(NavigationControlsVisibility), typeof(Visibility), typeof(StageControl), new PropertyMetadata(Visibility.Collapsed, OnNavigationControlsVisibilityChanged));

        public Visibility NavigationControlsVisibility
        {
            get { return (Visibility)GetValue(NavigationControlsVisibilityProperty); }
            set { SetValue(NavigationControlsVisibilityProperty, value); }
        }

        private static void OnNavigationControlsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }
    }
}
