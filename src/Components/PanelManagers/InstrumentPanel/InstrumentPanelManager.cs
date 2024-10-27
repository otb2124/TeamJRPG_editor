using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static TeamJRPG_editor.Instrument;

namespace TeamJRPG_editor
{
    public class InstrumentPanelManager
    {
        public Panel instrumentsPanel;
        public InstrumentType currentInstrumentType;
        public List<Instrument> instruments;

        public InstrumentPanelMenuStripManager instrumentPanelMenuStripManager;

        public InstrumentPanelManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.instrumentsPanel = new Panel
            {
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, Globals.ScreenSize.Height - 300),
                Size = new Size(Globals.ScreenSize.Width, 300),
                AutoScroll = true // Enable auto-scrolling
            };

            InitializeMenuStrip();
            InitializeInstruments();
        }

        private void InitializeMenuStrip()
        {
            instrumentPanelMenuStripManager = new InstrumentPanelMenuStripManager();
        }

        private void InitializeInstruments()
        {
            instruments = new List<Instrument>();
        }

        public void Invalidate()
        {
            instruments.Clear();
            instrumentsPanel.Controls.Clear();
            instrumentsPanel.Controls.Add(instrumentPanelMenuStripManager.menuStrip); // Ensure MenuStrip is not removed

            FillPanel();

            foreach (var instrument in instruments)
            {
                instrument.DrawPictureBox(); // Initialize and prepare PictureBox
                instrumentsPanel.Controls.Add(instrument.instrumentPictureBox); // Add to the panel
            }

            UpdateScrollableArea();
        }





        public void FillPanel()
        {

            Image image = null;

            switch (currentInstrumentType)
            {
                case InstrumentType.Tile:
                    image = Globals.assetSetter.GetSheet(AssetSetter.SheetCategory.tiles, 0);
                    int tileSize = 32;

                    // Calculate the number of tiles per row and column
                    int tilesPerRow = image.Width / tileSize;
                    int tilesPerColumn = image.Height / tileSize;
                    int totalTiles = tilesPerRow * tilesPerColumn;

                    for (int i = 0; i < totalTiles; i++)
                    {
                        instruments.Add(new Instrument(currentInstrumentType, i));
                    }
                    break;

                case InstrumentType.Companion:
                    image = Globals.assetSetter.GetSheet(AssetSetter.SheetCategory.character_bodies, 0);
                    instruments.Add(new Instrument(currentInstrumentType, 0));
                    break;


                default:
                    break;
            }

        }

        private void UpdateScrollableArea()
        {
            int totalHeight = 0;
            foreach (var instrument in instruments)
            {
                instrument.DrawPictureBox();
                totalHeight += instrument.instrumentPictureBox.Height;
            }

            if (totalHeight > instrumentsPanel.ClientSize.Height)
            {
                instrumentsPanel.AutoScroll = true;
                instrumentsPanel.AutoScrollMinSize = new Size(instrumentsPanel.ClientSize.Width, totalHeight);
            }
            else
            {
                instrumentsPanel.AutoScroll = false;
            }
        }
    }

    
}
