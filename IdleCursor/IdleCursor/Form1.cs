namespace IdleCursor;

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public partial class Form1 : Form
{

    bool AFK = false;
    Overlay overlay = new Overlay();
    public Form1()
    {
        InitializeComponent();
        this.Hide();
        this.ShowInTaskbar = false;
        this.BackColor = Color.Lime;
        this.TransparencyKey = Color.Lime;
        this.FormBorderStyle = FormBorderStyle.None;
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

            overlay.ShowEffect(new Point(cursorPos.X, cursorPos.Y)); // ✅ screen coordinates

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
            
            if (AFKTime > 45)
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
