using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IdleCursor
{
    public partial class Overlay: Form
    {

        private Point? effectPos = null; // null = don't draw anything

        public Overlay()
        {
            //Basically initlize it to be invisible full screen to cover everything 
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; 
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.Lime;
            this.TransparencyKey = Color.Lime;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
        }

        protected override bool ShowWithoutActivation => true; //so user does not interact with the overlay, like it doesnt exist input wise


        public void ShowEffect(Point screenPos)
        {
            effectPos = screenPos;
            this.Show();
            this.Invalidate(); // Triggers OnPaint

            System.Windows.Forms.Timer clearTimer = new System.Windows.Forms.Timer();
            clearTimer.Interval = 2000;
            clearTimer.Tick += (s, e) =>
            {
                clearTimer.Stop();
                effectPos = null; // Clear the circle
                this.Invalidate(); // Redraw without circle
                this.Hide(); // Hide form again
            };
            clearTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (effectPos.HasValue)
            {
                int radius = 50;
                Point pos = effectPos.Value;

                using (Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawEllipse(pen, pos.X - radius / 2, pos.Y - radius / 2, radius, radius);
                }
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80000 | 0x20; // WS_EX_LAYERED | WS_EX_TRANSPARENT
                return cp;
            }
        }
    }

}




