namespace IdleCursor;

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public partial class Form1 : Form
{

    bool AFK = false;
    
    public Form1()
    {
        InitializeComponent();


    }

    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    [DllImport("user32.dll")]
    static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

 


    private void Form1_Load(object sender, EventArgs e)
    {
       
    }

    private void CursorEffect()
    {
        POINT cursorPos;
        if (GetCursorPos(out cursorPos))
        {

            using (Graphics g = this.CreateGraphics())
            {
                Point formPos = this.PointToClient(new Point(cursorPos.X, cursorPos.Y));

                int radius = 50;
                g.DrawEllipse(Pens.Red, formPos.X - radius / 2, formPos.Y - radius / 2, radius, radius);
            }

            Timer clearTimer = new Timer();
            clearTimer.Interval = 2000;
            clearTimer.Tick += (s, e) =>
            {
                clearTimer.Stop();
                this.Invalidate(); // Triggers a repaint → clears drawing
            };
            clearTimer.Start();
        }


    }

    // This runs every every second to check if user is idle for > Y time
    private void timer1_Tick(object sender, EventArgs e)
    {
        uint SystemUpTime = (uint)Environment.TickCount;
        LASTINPUTINFO LastInputInfo = new LASTINPUTINFO();
        LastInputInfo.cbSize = (uint)Marshal.SizeOf(LastInputInfo); //windows wants to know the size of the struct so take it -_- 
         
        if (GetLastInputInfo(ref LastInputInfo))
        {
            uint AFKTime = ((uint)SystemUpTime - LastInputInfo.dwTime) / 1000;
            
            if (AFKTime > 5)
            {

                Console.WriteLine("User is AFK");
                AFK = true; 
            }
            else
            {
                if (AFK)
                {
                    CursorEffect(); //user is back so activate the effect
                    AFK = false;
                }
            }
        }

    }
}
