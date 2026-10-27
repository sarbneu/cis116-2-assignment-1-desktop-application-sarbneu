using System;
using System.Windows.Forms;

namespace ElevatorControl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Elevator Control";
            this.Size = new System.Drawing.Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
        }
    }
}
