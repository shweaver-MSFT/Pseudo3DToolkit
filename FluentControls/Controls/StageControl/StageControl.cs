using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Pseudo3DToolkit.Controls
{
    public sealed partial class StageControl : ContentControl
    {
        public StageControl()
        {
            DefaultStyleKey = typeof(StageControl);
        }

        protected override void OnApplyTemplate()
        {


            base.OnApplyTemplate();
        }
    }
}
