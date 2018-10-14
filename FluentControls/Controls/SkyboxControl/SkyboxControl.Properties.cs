using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class SkyboxControl : ContentControl
    {
        public static readonly DependencyProperty AutoRotateProperty = DependencyProperty.Register(nameof(AutoRotate), typeof(bool), typeof(SkyboxControl), new PropertyMetadata(false, OnAutoRotateChanged));

        public static readonly DependencyProperty NavigationControlsVisibilityProperty = DependencyProperty.Register(nameof(NavigationControlsVisibility), typeof(Visibility), typeof(SkyboxControl), new PropertyMetadata(Visibility.Collapsed, OnNavigationControlsVisibilityChanged));

        public static readonly DependencyProperty TopImageProperty = DependencyProperty.Register(nameof(TopImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnTopImageChanged));

        public static readonly DependencyProperty BottomImageProperty = DependencyProperty.Register(nameof(BottomImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnBottomImageChanged));

        public static readonly DependencyProperty LeftImageProperty = DependencyProperty.Register(nameof(LeftImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnLeftImageChanged));

        public static readonly DependencyProperty RightImageProperty = DependencyProperty.Register(nameof(RightImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnRightImageChanged));

        public static readonly DependencyProperty FrontImageProperty = DependencyProperty.Register(nameof(FrontImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnFrontImageChanged));

        public static readonly DependencyProperty BackImageProperty = DependencyProperty.Register(nameof(BackImage), typeof(string), typeof(SkyboxControl), new PropertyMetadata(_defaultImage, OnBackImageChanged));

        public bool AutoRotate
        {
            get { return (bool)GetValue(AutoRotateProperty); }
            set { SetValue(AutoRotateProperty, value); }
        }

        public Visibility NavigationControlsVisibility
        {
            get { return (Visibility)GetValue(NavigationControlsVisibilityProperty); }
            set { SetValue(NavigationControlsVisibilityProperty, value); }
        }

        public string TopImage
        {
            get { return (string)GetValue(TopImageProperty); }
            set { SetValue(TopImageProperty, value); }
        }

        public string BottomImage
        {
            get { return (string)GetValue(BottomImageProperty); }
            set { SetValue(BottomImageProperty, value); }
        }

        public string LeftImage
        {
            get { return (string)GetValue(LeftImageProperty); }
            set { SetValue(LeftImageProperty, value); }
        }

        public string RightImage
        {
            get { return (string)GetValue(RightImageProperty); }
            set { SetValue(RightImageProperty, value); }
        }

        public string FrontImage
        {
            get { return (string)GetValue(FrontImageProperty); }
            set { SetValue(FrontImageProperty, value); }
        }

        public string BackImage
        {
            get { return (string)GetValue(BackImageProperty); }
            set { SetValue(BackImageProperty, value); }
        }

        private static void OnAutoRotateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnNavigationControlsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnTopImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnBottomImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnLeftImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnRightImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnFrontImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }

        private static void OnBackImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SkyboxControl skybox)
            {

            }
        }
    }
}
