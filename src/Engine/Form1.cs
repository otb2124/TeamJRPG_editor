using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Globals.formManager = new FormManager();
            Initialize();
        }

        public void Initialize()
        {

            // 
            // Form1
            // 
            this.ClientSize = Globals.ScreenSize;
            this.Controls.Add(Globals.canvasPanelManager.canvasPanel);
            this.Controls.Add(Globals.instrumentPanelManager.instrumentsPanel);
            this.Controls.Add(Globals.menuStripManager.menuStrip);
            this.Controls.Add(Globals.rightPanelManager.rightPanel); // Add the new panel to the form
            this.MainMenuStrip = Globals.menuStripManager.menuStrip;
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            this.Load += new System.EventHandler(this.Form1_Load);

            // Ensure the form can handle key events
            this.KeyPreview = true; // Allow the form to capture key events before controls
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            Globals.menuStripManager.menuStrip.ResumeLayout(false);
            Globals.menuStripManager.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            Globals.form = this;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.formManager.OnLoad();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Forward key events to CanvasPanelManager for viewport movement
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                //canvasPanelManager.HandleKeyDown(e);
                e.Handled = true; // Mark the event as handled
            }
        }
    }
}
