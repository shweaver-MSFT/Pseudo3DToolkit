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
        private const int _defaultAnimationDuration = 1000;

        public Visual CameraVisual { get; }

        // This boolean determines whether to use composition expression animations
        // to drive camera movement, or without.
        // Using expression animations is nice because the camera can then be driven
        // by animating things like the "cameraAnimationViewportSize" field in 
        // CameraVisual.Properties
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
            set
            {
                Set(ref _isOrthographic, value);
                UpdateVisualMatrix();
                AnimateVisualMatrix();
            }
        }

        public Vector2 ViewportSize
        {
            get
            {
                var status = CameraVisual.Properties.TryGetVector2("cameraAnimationViewportSize", out Vector2 value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : new Vector2(0, 0);
            }
            set
            {
                CameraVisual.Properties.InsertVector2("cameraAnimationViewportSize", value);
                UpdateVisualMatrix();
            }
        }

        public float PerspectiveDistance
        {
            get
            {
                var status = CameraVisual.Properties.TryGetScalar("cameraAnimationPerspectiveDistance", out float value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : 1;
            }
            set
            {
                CameraVisual.Properties.InsertScalar("cameraAnimationPerspectiveDistance", (value <= 0) ? 1 : value);
                UpdateVisualMatrix();
            }
        }

        public Vector3 Position
        {
            get
            {
                var status = CameraVisual.Properties.TryGetVector3("cameraAnimationPosition", out Vector3 value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : new Vector3(0, 0, 0);
            }
            set
            {
                CameraVisual.Properties.InsertVector3("cameraAnimationPosition", value);
                UpdateVisualMatrix();
            }
        }
        public float Yaw
        {
            get
            {
                var status = CameraVisual.Properties.TryGetScalar("cameraAnimationYaw", out float value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : 1;
            }
            set
            {
                CameraVisual.Properties.InsertScalar("cameraAnimationYaw", value);
                UpdateVisualMatrix();
            }
        }

        public float Pitch
        {
            get
            {
                var status = CameraVisual.Properties.TryGetScalar("cameraAnimationPitch", out float value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : 1;
            }
            set
            {
                CameraVisual.Properties.InsertScalar("cameraAnimationPitch", value);
                UpdateVisualMatrix();
            }
        }

        public float Roll
        {
            get
            {
                var status = CameraVisual.Properties.TryGetScalar("cameraAnimationRoll", out float value);
                return status.Equals(CompositionGetValueStatus.Succeeded) ? value : 1;
            }
            set
            {
                CameraVisual.Properties.InsertScalar("cameraAnimationRoll", value);
                UpdateVisualMatrix();
            }
        }

        public CompositionCamera(Visual cameraVisual)
        {
            CameraVisual = cameraVisual;

            // Set defaults
            UseAnimations = true;
            ViewportSize = new Vector2(1, 1);
            PerspectiveDistance = (ViewportSize.X + ViewportSize.Y) / 2;
            Yaw = 0;
            Pitch = 0;
            Roll = 0;
            Position = new Vector3(0, 0, -PerspectiveDistance);
            IsOrthographic = true;

            AnimateVisualMatrix();
        }

        public void TranslateX(float value, float duration = _defaultAnimationDuration)
        {
            Position = new Vector3(value, Position.Y, Position.Z);
            Vector3KeyFrameAnimation animateCameraPosition = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPosition.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPosition.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPosition);
        }

        public void TranslateY(float value, float duration = _defaultAnimationDuration)
        {
            Position = new Vector3(Position.X, value, Position.Z);
            Vector3KeyFrameAnimation animateCameraPosition = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPosition.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPosition.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPosition);
        }

        public void Zoom(float value, float duration = _defaultAnimationDuration)
        {
            Position = new Vector3(Position.X, Position.Y, value);
            Vector3KeyFrameAnimation animateCameraPositionZoom = CameraVisual.Compositor.CreateVector3KeyFrameAnimation();
            animateCameraPositionZoom.Duration = TimeSpan.FromMilliseconds(duration);
            animateCameraPositionZoom.InsertKeyFrame(1f, Position);
            CameraVisual.Properties.StartAnimation("cameraAnimationPosition", animateCameraPositionZoom);
        }

        public void RotatePitch(float value, float duration = _defaultAnimationDuration)
        {
            ScalarKeyFrameAnimation rotateAnimation = CameraVisual.Compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, value);
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            CameraVisual.Properties.StartAnimation("cameraAnimationPitch", rotateAnimation);
        }

        public void RotateYaw(float value, float duration = _defaultAnimationDuration)
        {
            ScalarKeyFrameAnimation rotateAnimation = CameraVisual.Compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, value);
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            CameraVisual.Properties.StartAnimation("cameraAnimationYaw", rotateAnimation);
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
