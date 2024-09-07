using System.Drawing;
using System.Windows.Forms;

namespace ViewModel.Utilities
{
    public static class ScreenWidthCalculator
    {
        public static double GetScreenWidth()
        {
            var screen = Screen.PrimaryScreen;

            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = g.DpiX;
                return screen.Bounds.Width * 96.0 / dpiX - 555;
            }
        }
    }
}