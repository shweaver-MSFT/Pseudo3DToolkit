using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Pseudo3DToolkit.Controls
{
    public sealed class VisualElement : UserControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(nameof(Camera), typeof(CameraControl), typeof(VisualElement), new PropertyMetadata(null, OnCameraChanged));

        public static readonly DependencyProperty PositionXProperty = DependencyProperty.Register(nameof(PositionX), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnPositionXChanged));

        public static readonly DependencyProperty PositionYProperty = DependencyProperty.Register(nameof(PositionY), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnPositionYChanged));

        public static readonly DependencyProperty PositionZProperty = DependencyProperty.Register(nameof(PositionZ), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnPositionZChanged));

        public new static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof(Height), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnHeightChanged));

        public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof(Width), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnWidthChanged));

        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register(nameof(Comment), typeof(string), typeof(VisualElement), new PropertyMetadata("", OnCommentChanged));

        public static readonly DependencyProperty CompositionBrushProperty = DependencyProperty.Register(nameof(CompositionBrush), typeof(CompositionBrush), typeof(VisualElement), new PropertyMetadata(null, OnCompositionBrushChanged));

        public static readonly DependencyProperty RotationAngleInDegreesProperty = DependencyProperty.Register(nameof(RotationAngleInDegrees), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnRotationAngleInDegreesChanged));

        public static readonly DependencyProperty RotationAxisXProperty = DependencyProperty.Register(nameof(RotationAxisX), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnRotationAxisXChanged));

        public static readonly DependencyProperty RotationAxisYProperty = DependencyProperty.Register(nameof(RotationAxisY), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnRotationAxisYChanged));

        public static readonly DependencyProperty RotationAxisZProperty = DependencyProperty.Register(nameof(RotationAxisZ), typeof(float), typeof(VisualElement), new PropertyMetadata(0f, OnRotationAxisZChanged));
        #endregion

        #region Properties
        public CameraControl Camera
        {
            get => (CameraControl)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        public float PositionX
        {
            get => (float)GetValue(PositionXProperty);
            set => SetValue(PositionXProperty, value);
        }

        public float PositionY
        {
            get => (float)GetValue(PositionYProperty);
            set => SetValue(PositionYProperty, value);
        }

        public float PositionZ
        {
            get => (float)GetValue(PositionZProperty);
            set => SetValue(PositionZProperty, value);
        }

        public new float Width
        {
            get => (float)GetValue(PositionZProperty);
            set => SetValue(PositionZProperty, value);
        }

        public new float Height
        {
            get => (float)GetValue(PositionZProperty);
            set => SetValue(PositionZProperty, value);
        }

        public string Comment
        {
            get => (string)GetValue(CommentProperty);
            set => SetValue(CommentProperty, value);
        }

        public CompositionBrush CompositionBrush
        {
            get => (CompositionBrush)GetValue(CompositionBrushProperty);
            set => SetValue(CompositionBrushProperty, value);
        }

        public float RotationAngleInDegrees
        {
            get => (float)GetValue(RotationAngleInDegreesProperty);
            set => SetValue(RotationAngleInDegreesProperty, value);
        }

        public float RotationAxisX
        {
            get => (float)GetValue(RotationAxisXProperty);
            set => SetValue(RotationAxisXProperty, value);
        }

        public float RotationAxisY
        {
            get => (float)GetValue(RotationAxisYProperty);
            set => SetValue(RotationAxisYProperty, value);
        }

        public float RotationAxisZ
        {
            get => (float)GetValue(RotationAxisZProperty);
            set => SetValue(RotationAxisZProperty, value);
        }
        #endregion

        #region OnDependencyPropertyChanged
        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var camera = (CameraControl)e.NewValue;
                visualElement.CreateVisual(camera.CompositionCamera.CameraVisual.Compositor, camera);
            }
        }

        private static void OnPositionXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)e.NewValue;
                var y = (float)d.GetValue(PositionYProperty);
                var z = (float)d.GetValue(PositionZProperty);
                visualElement._visual.Offset = new Vector3(x, y, z);
            }
        }

        private static void OnPositionYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)d.GetValue(PositionXProperty);
                var y = (float)e.NewValue;
                var z = (float)d.GetValue(PositionZProperty);
                visualElement._visual.Offset = new Vector3(x, y, z);
            }
        }

        private static void OnPositionZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)d.GetValue(PositionXProperty);
                var y = (float)d.GetValue(PositionYProperty);
                var z = (float)e.NewValue;
                visualElement._visual.Offset = new Vector3(x, y, z);
            }
        }

        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var width = (float)d.GetValue(WidthProperty);
                var height = (float)e.NewValue;
                visualElement._visual.Size = new Vector2(width, height);
            }
        }

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var width = (float)e.NewValue;
                var height = (float)d.GetValue(HeightProperty);
                visualElement._visual.Size = new Vector2(width, height);
            }
        }

        private static void OnCommentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var comment = (string)e.NewValue;
                visualElement._visual.Comment = comment;
            }
        }

        private static void OnCompositionBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var brush = (CompositionBrush)e.NewValue;
                visualElement._visual.Brush = brush;
            }
        }

        private static void OnRotationAngleInDegreesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var angle = (float)e.NewValue;
                visualElement._visual.RotationAngleInDegrees = angle;
            }
        }

        private static void OnRotationAxisXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)e.NewValue;
                var y = (float)d.GetValue(RotationAxisYProperty);
                var z = (float)d.GetValue(RotationAxisZProperty);
                visualElement._visual.RotationAxis = new Vector3(x, y, z);
            }
        }

        private static void OnRotationAxisYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)d.GetValue(RotationAxisXProperty);
                var y = (float)e.NewValue;
                var z = (float)d.GetValue(RotationAxisZProperty);
                visualElement._visual.RotationAxis = new Vector3(x, y, z);
            }
        }

        private static void OnRotationAxisZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualElement visualElement)
            {
                var x = (float)d.GetValue(RotationAxisXProperty);
                var y = (float)d.GetValue(RotationAxisYProperty);
                var z = (float)e.NewValue;
                visualElement._visual.RotationAxis = new Vector3(x, y, z);
            }
        }
        #endregion

        public VisualElement()
        {
            
        }

        public SpriteVisual GetSpriteVisual(Compositor compositor)
        {
            var visual = compositor.CreateSpriteVisual();
            visual.Comment = Comment;
            visual.Brush = CompositionBrush;
            visual.RotationAngleInDegrees = RotationAngleInDegrees;
            visual.Size = new Vector2(Width, Height);
            visual.Offset = new Vector3(PositionX, PositionY, PositionZ);
            visual.RotationAxis = new Vector3(RotationAxisX, RotationAxisY, RotationAxisZ);

            return visual;
        }
    }
}
