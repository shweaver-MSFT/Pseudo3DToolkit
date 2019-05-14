using Windows.UI.Xaml;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class StageControl
    {
        public static readonly DependencyProperty NavigationControlsVisibilityProperty = DependencyProperty.Register(nameof(NavigationControlsVisibility), typeof(Visibility), typeof(StageControl), new PropertyMetadata(Visibility.Collapsed, OnNavigationControlsVisibilityChanged));

        public static readonly DependencyProperty BackdropImageProperty = DependencyProperty.Register(nameof(BackdropImage), typeof(string), typeof(StageControl), new PropertyMetadata(null, OnBackdropImageChanged));

        public static readonly DependencyProperty FloorImageProperty = DependencyProperty.Register(nameof(FloorImage), typeof(string), typeof(StageControl), new PropertyMetadata(null, OnFloorImageChanged));

        public Visibility NavigationControlsVisibility
        {
            get { return (Visibility)GetValue(NavigationControlsVisibilityProperty); }
            set { SetValue(NavigationControlsVisibilityProperty, value); }
        }

        public string BackdropImage
        {
            get { return (string)GetValue(BackdropImageProperty); }
            set { SetValue(BackdropImageProperty, value); }
        }

        public string FloorImage
        {
            get { return (string)GetValue(FloorImageProperty); }
            set { SetValue(FloorImageProperty, value); }
        }

        private static void OnNavigationControlsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StageControl stage)
            {

            }
        }

        private static void OnBackdropImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StageControl stage)
            {

            }
        }

        private static void OnFloorImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StageControl stage)
            {

            }
        }
    }
}
