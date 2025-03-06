using Avalonia.Controls.Skia;
using Avalonia.Input;
using Avalonia.Skia;
using System;

namespace ECAD_Library.Controls
{
    public partial class CanvasControl : SKCanvasControl
    {
        public ulong leftclicktime;
        public bool IsPointerWithin = false;
        public bool IsRightBtnPressed = false;
        public CanvasControl()
        {

            Focusable = true;
            Focus();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
        }

        protected override void OnDraw(SKCanvasEventArgs e)
        {
            base.OnDraw(e);
        }
        protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);
            IsPointerWithin = true;
        }
        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            IsPointerWithin = false;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            var point = e.GetPosition(this);
            var properties = e.GetCurrentPoint(this).Properties;
            if (e.ClickCount == 1)
            {
                IsRightBtnPressed = properties.IsRightButtonPressed;

            }
            else
            {

            }
            InvalidateVisual();
        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var point = e.GetPosition(this);
            var properties = e.GetCurrentPoint(this).Properties;
            ulong timenow = (ulong)DateTime.Now.Ticks;
            var time = timenow - leftclicktime;

            InvalidateVisual();
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            var point = e.GetPosition(this);

            InvalidateVisual();
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            var pointer = e.GetPosition(this).ToSKPoint();

            InvalidateVisual();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            InvalidateVisual();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            InvalidateVisual();
        }
    }
}
