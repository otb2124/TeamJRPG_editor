using System.Drawing;
using System.Windows.Forms;


namespace TeamJRPG_editor
{
    public static class Globals
    {

        public static Size ScreenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        public static Form1 form;
        public static CanvasPanelManager canvasPanelManager;
        public static InstrumentPanelManager instrumentPanelManager;
        public static RightPanelManager rightPanelManager;
        public static MainMenuStripManager menuStripManager;

        public static AssetSetter assetSetter;

        public static FormManager formManager;


        public enum ViewMode { tiles, entities };
        public static ViewMode currentViewMode;
    }
}
