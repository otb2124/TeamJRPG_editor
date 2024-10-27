using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TeamJRPG_editor.Instrument;

namespace TeamJRPG_editor
{
    public class InstrumentPanelMenuStripManager
    {

        public MenuStrip menuStrip;

        public InstrumentPanelMenuStripManager()
        {
            Initialize();
        }


        public void Initialize()
        {
            menuStrip = new MenuStrip
            {
                Dock = DockStyle.Top
            };

            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("Tiles");
            fileMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.Tile));

            ToolStripMenuItem editMenuItem = new ToolStripMenuItem("Entities");
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.Companion));
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.NPC));
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.Mob));
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.DObject));
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.IObject));
            editMenuItem.DropDownItems.Add(new InstrumentToolStripMenuItem(Instrument.InstrumentType.Event));

            menuStrip.Items.Add(fileMenuItem);
            menuStrip.Items.Add(editMenuItem);
        }
    }




    public class InstrumentToolStripMenuItem : ToolStripMenuItem
    {
        public InstrumentType type;

        public InstrumentToolStripMenuItem(InstrumentType type)
        {
            this.type = type;
            this.Text = type.ToString();
            this.Click += OnInstrumentMenuClick;
        }

        private void OnInstrumentMenuClick(object sender, EventArgs e)
        {

            Globals.ViewMode newMode = Globals.currentViewMode;


            if (type == InstrumentType.Tile)
            {
                newMode = Globals.ViewMode.tiles;
            }
            else
            {
                newMode = Globals.ViewMode.entities;
            }



            if (newMode != Globals.currentViewMode)
            {
                Globals.formManager.SetViewMode(newMode);
            }


            Globals.instrumentPanelManager.currentInstrumentType = type;
            Globals.instrumentPanelManager.Invalidate();
        }
    }
}
