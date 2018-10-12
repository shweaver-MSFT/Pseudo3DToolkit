using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Windows.UI.Composition;

namespace Pseudo3DToolkit.Controls
{
    /// <summary>
    /// A standalone CompositionCamera
    /// </summary>
    public sealed class CompositionCamera : INotifyPropertyChanged
    {
        public Visual CameraVisual { get; private set; }

        private bool _useAnimations;
        public bool UseAnimations
        {
            get => _useAnimations;
            set => Set(ref _useAnimations, value);
        }

        private bool _isOrthographic;
        public bool IsOrthographic
        {
            get => _isOrthographic;
            set => Set(ref _isOrthographic, value);
        }

        private float _perspectiveDistance;
        public float PerspectiveDistance
        {
            get => _perspectiveDistance;
            set => Set(ref _perspectiveDistance, value);
        }

        private Vector2 _viewportSize;
        public Vector2 ViewportSize
        {
            get => _viewportSize;
            set => Set(ref _viewportSize, value);
        }

        private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
            set => Set(ref _position, value);
        }

        private float _yaw;
        public float Yaw
        {
            get => _yaw;
            set => Set(ref _yaw, value);
        }

        private float _pitch;
        public float Pitch
        {
            get => _pitch;
            set => Set(ref _pitch, value);
        }

        private float _roll;
        public float Roll
        {
            get => _roll;
            set => Set(ref _roll, value);
        }

        public CompositionCamera(Visual cameraVisual)
        {
            CameraVisual = cameraVisual;

            // Set defaults
            UseAnimations = false;
            IsOrthographic = true;
            ViewportSize = new Vector2(1, 1);
            PerspectiveDistance = (ViewportSize.X + ViewportSize.Y) / 2;
            Position = new Vector3(0, 0, -PerspectiveDistance);
            Yaw = 0;
            Pitch = 0;
            Roll = 0;

            PropertyChanged += CompositionCamera_PropertyChanged;

            AnimateVisualMatrix();
        }

        private void CompositionCamera_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ViewportSize):
                    CameraVisual.Properties.InsertVector2("cameraAnimationViewportSize", ViewportSize);
                    break;
                case nameof(PerspectiveDistance):
                    CameraVisual.Properties.InsertScalar("cameraAnimationPerspectiveDistance", PerspectiveDistance);
                    break;
                case nameof(Position):
                    CameraVisual.Properties.InsertVector3("cameraAnimationPosition", Position);
                    break;
                case nameof(Yaw):
                    CameraVisual.Properties.InsertScalar("cameraAnimationYaw", Yaw);
                    break;
                case nameof(Pitch):
                    CameraVisual.Properties.InsertScalar("cameraAnimationPitch", Pitch);
                    break;
                case nameof(Roll):
                    CameraVisual.Properties.InsertScalar("cameraAnimationRoll", Roll);
                    break;
            }

            UpdateVisualMatrix();

            if (e.PropertyName == nameof(IsOrthographic))
            {
                AnimateVisualMatrix();
            }
        }

        private void AnimateVisualMatrix()
        {
            if (!_useAnimations)
            {
                return;
            }

            var perspectiveMatrix =
                "Matrix4x4.CreateTranslation(Vector3(0, 0, properties.cameraAnimationPerspectiveDistance)) * " +
                "Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, -1 / properties.cameraAnimationPerspectiveDistance, 0, 0, 0, 1) * " +
                /* This function is not supported in the expression language for some reason
                 * "Matrix4x4.CreatePerspective(1,1 properties.cameraAnimationPerspectiveDistance, properties.cameraAnimationPerspectiveDistance * 100f) * " + */
                "Matrix4x4.CreateTranslation(Vector3(properties.cameraAnimationViewportSize.X / 2, properties.cameraAnimationViewportSize.Y / 2, 0))";

            if (IsOrthographic)
            {
                perspectiveMatrix = "Matrix4x4(1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1)";
            }

            var expressionAnimation = CameraVisual.Compositor.CreateExpressionAnimation();
            expressionAnimation.Expression =
                // Translation
                "Matrix4x4.CreateTranslation(-properties.cameraAnimationPosition) * " +

                // Rotation
                "Matrix4x4.CreateFromAxisAngle(Vector3(0,1,0), properties.cameraAnimationYaw) * " +
                "Matrix4x4.CreateFromAxisAngle(Vector3(1,0,0), properties.cameraAnimationPitch) * " +
                "Matrix4x4.CreateFromAxisAngle(Vector3(0,0,1), properties.cameraAnimationRoll) * " +

                // Perspective
                perspectiveMatrix;

            expressionAnimation.SetReferenceParameter("properties", CameraVisual.Properties);
            CameraVisual.StartAnimation("transformMatrix", expressionAnimation);
        }

        private void UpdateVisualMatrix()
        {
            if (_useAnimations)
            {
                return;
            }

            if (PerspectiveDistance <= 0 || ViewportSize.X <= 0 || ViewportSize.Y <= 0)
            {
                return;
            }

            // translate
            Matrix4x4 translate = Matrix4x4.CreateTranslation(-Position);

            // rotate
            Matrix4x4 rotate = Matrix4x4.CreateFromYawPitchRoll(Yaw, 0, 0) *
                Matrix4x4.CreateFromYawPitchRoll(0, Pitch, 0) *
                Matrix4x4.CreateFromYawPitchRoll(0, 0, Roll);

            // Three different ways of making a perspective matrix
            Matrix4x4 perspectiveStandard =
                Matrix4x4.CreatePerspective(1, 1, PerspectiveDistance, PerspectiveDistance * 100f) *
                Matrix4x4.CreateTranslation(new Vector3(ViewportSize / 2, 0));

            Matrix4x4 perspectiveOffCenter =
                Matrix4x4.CreatePerspectiveOffCenter(
                    -1f, 1f,
                    -1f, 1f,
                    PerspectiveDistance, PerspectiveDistance * 100) *
                Matrix4x4.CreateTranslation(new Vector3(0, 0, -PerspectiveDistance)) *
                Matrix4x4.CreateTranslation(new Vector3(ViewportSize / 2, 0));// *

            Matrix4x4 perspectiveChris =
                Matrix4x4.CreateTranslation(new Vector3(0, 0, PerspectiveDistance)) *
                new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, -1 / PerspectiveDistance, 0, 0, 0, 1) *
                Matrix4x4.CreateTranslation(new Vector3(ViewportSize / 2, 0));

            //CameraVisual.TransformMatrix = perspectiveOffCenter;
            //CameraVisual.TransformMatrix = perspectiveStandard;
            //CameraVisual.TransformMatrix = perspectiveChris;

            CameraVisual.TransformMatrix = translate * rotate * (IsOrthographic ? Matrix4x4.Identity : perspectiveChris);
        }

        public void TranslateX(float value, float duration = 1)
        {
            Position = new Vector3(value, Position.Y, Position.Z);
            Vector3KeyFrameAnimation animateCameraPosition = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPosition.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPosition.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPosition);
        }

        public void TranslateY(float value, float duration = 1)
        {
            Position = new Vector3(Position.X, value, Position.Z);
            Vector3KeyFrameAnimation animateCameraPosition = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPosition.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPosition.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPosition);
        }

        public void Zoom(float value, float duration = 1)
        {
            Position = new Vector3(Position.X, Position.Y, value);
            Vector3KeyFrameAnimation animateCameraPositionZoom = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPositionZoom.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPositionZoom.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPositionZoom);
        }

        public void RotatePitch(float value, float duration = 1)
        {
            ScalarKeyFrameAnimation rotateAnimation = CameraVisual.Compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, value);
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            CameraVisual.Properties.StartAnimation("cameraAnimationPitch", rotateAnimation);
        }

        public void RotateYaw(float value, float duration = 1)
        {
            ScalarKeyFrameAnimation rotateAnimation = CameraVisual.Compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, value);
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            CameraVisual.Properties.StartAnimation("cameraAnimationYaw", rotateAnimation);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
