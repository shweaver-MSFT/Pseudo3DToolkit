﻿using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class CameraControl : UserControl
    {
        public CompositionCamera CompositionCamera { get; private set; }

        public bool UseAnimations
        {
            get => CompositionCamera.UseAnimations;
            set => CompositionCamera.UseAnimations = value;
        }

        public bool IsOrthographic
        {
            get => CompositionCamera.IsOrthographic;
            set => CompositionCamera.IsOrthographic = value;
        }

        public float PerspectiveDistance
        {
            get => CompositionCamera.PerspectiveDistance;
            set => CompositionCamera.PerspectiveDistance = value;
        }

        public Vector2 ViewportSize
        {
            get => CompositionCamera.ViewportSize;
            set => CompositionCamera.ViewportSize = value;
        }

        public Vector3 Position
        {
            get => CompositionCamera.Position;
            set => CompositionCamera.Position = value;
        }

        public float Yaw
        {
            get => CompositionCamera.Yaw;
            set => CompositionCamera.Yaw = value;
        }

        public float Pitch
        {
            get => CompositionCamera.Pitch;
            set => CompositionCamera.Pitch = value;
        }

        public float Roll
        {
            get => CompositionCamera.Roll;
            set => CompositionCamera.Roll = value;
        }

        public CameraControl()
        {
            Visual cameraVisual = ElementCompositionPreview.GetElementVisual(this);
            CompositionCamera = new CompositionCamera(cameraVisual);
        }

        public void SetAsPerspective(Vector2 zPlaneSize)
        {
            ViewportSize = zPlaneSize;
            CompositionCamera.IsOrthographic = false;
        }

        public void SetAsOrthographic()
        {
            CompositionCamera.IsOrthographic = true;
        }
    }
}
