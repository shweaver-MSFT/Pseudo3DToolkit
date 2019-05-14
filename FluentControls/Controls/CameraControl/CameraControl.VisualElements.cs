using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class CameraControl : UserControl
    {
        public static readonly DependencyProperty VisualElementsProperty = DependencyProperty.Register(nameof(VisualElements), typeof(List<VisualElement>), typeof(CameraControl), new PropertyMetadata(new List<VisualElement>(), OnVisualElementsChanged));

        public List<VisualElement> VisualElements
        {
            get => (List<VisualElement>)GetValue(VisualElementsProperty);
            set => SetValue(VisualElementsProperty, value);
        }

        /// <summary>
        /// Update elements to use this Camera instance
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void OnVisualElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CameraControl camera)
            {
                var visualElements = (List<VisualElement>)e.NewValue;
                foreach(var ve in visualElements)
                {
                    ve.Camera = camera;
                }
            }
        }
    }
}
