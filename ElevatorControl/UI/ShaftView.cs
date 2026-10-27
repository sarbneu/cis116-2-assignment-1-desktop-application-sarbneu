using System;
using System.Drawing;
using System.Windows.Forms;

namespace ElevatorControl.UI
{
    public sealed class ShaftView : UserControl
    {
        public string ElevatorName { get; set; } = "Elevator";

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.WhiteSmoke);
            e.Graphics.DrawRectangle(Pens.DarkGray, 10, 10, Width - 20, Height - 20);
        }
    }
}
