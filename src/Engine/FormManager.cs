using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public class FormManager
    {


        



        public FormManager() 
        {
            Initialize();
        }


        public void Initialize()
        {
            SetInitPreferences();

            Globals.assetSetter = new AssetSetter();

            Globals.instrumentPanelManager = new InstrumentPanelManager();
            Globals.menuStripManager = new MainMenuStripManager();
            Globals.canvasPanelManager = new CanvasPanelManager();
            Globals.rightPanelManager = new RightPanelManager();
            
        }


        public void SetInitPreferences()
        {
            Globals.currentViewMode = Globals.ViewMode.tiles;
        }




        public void OnLoad()
        {
            Globals.instrumentPanelManager.Invalidate();
        }





        public void SetViewMode(Globals.ViewMode viewMode)
        {

                Globals.currentViewMode = viewMode;
                Globals.canvasPanelManager.canvasPanel.Invalidate();
                Globals.canvasPanelManager.selectedInstrument = null;
                Globals.instrumentPanelManager.Invalidate();
                Globals.rightPanelManager.ClearInfoDisplay();
            
        }
    }
}
