using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class CameraControl : ItemsControl
    {
        //public static readonly DependencyProperty VisualElementsProperty = DependencyProperty.Register(nameof(VisualElements), typeof(List<VisualElement>), typeof(CameraControl), new PropertyMetadata(null, OnVisualElementsChanged));

        //public List<VisualElement> VisualElements
        //{
        //    get => (List<VisualElement>)GetValue(VisualElementsProperty);
        //    set => SetValue(VisualElementsProperty, value);
        //}

        //public static void OnVisualElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is CameraControl camera)
        //    {
        //        var visualElements = (List<VisualElement>)e.NewValue;

        //    }
        //}

        protected override void OnItemsChanged(object e)
        {




            base.OnItemsChanged(e);
        }
    }
}
