using System;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public class MainMenuStripManager
    {


        public MenuStrip menuStrip;

        public MainMenuStripManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            menuStrip = new MenuStrip();

            var fileMenu = CreateMenuItem("File", new ToolStripMenuItem[]
            {
                CreateMenuItem("Load", LoadMenuItem_Click),
                CreateMenuItem("Save", SaveMenuItem_Click)
            });

            var viewMenu = CreateMenuItem("View", new ToolStripMenuItem[]
            {
                CreateMenuItem("Tiles", TilesMenuItem_Click),
                CreateMenuItem("Entities", EntitiesMenuItem_Click)
            });

            var editMenu = CreateMenuItem("Edit", new ToolStripMenuItem[]
            {
                CreateMenuItem("Group", EditMenu_Click),
                CreateMenuItem("General", EditMenu_Click),
                CreateMenuItem("Map", EditMenu_Click)
            });

            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, viewMenu, editMenu });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(800, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
        }

        private ToolStripMenuItem CreateMenuItem(string text, EventHandler onClick)
        {
            var menuItem = new ToolStripMenuItem
            {
                Name = text.ToLower() + "MenuItem",
                Size = new System.Drawing.Size(140, 26),
                Text = text
            };
            menuItem.Click += onClick;
            return menuItem;
        }

        private ToolStripMenuItem CreateMenuItem(string text, ToolStripMenuItem[] subItems)
        {
            var menuItem = new ToolStripMenuItem
            {
                Name = text.ToLower() + "Menu",
                Size = new System.Drawing.Size(55, 24),
                Text = text,
                DropDownItems = { }
            };
            menuItem.DropDownItems.AddRange(subItems);
            return menuItem;
        }

        // Event handler methods
        private void LoadMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Load menu item clicked.");
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Save menu item clicked.");
        }

        private void TilesMenuItem_Click(object sender, EventArgs e)
        {
            Globals.formManager.SetViewMode(Globals.ViewMode.tiles);
        }

        private void EntitiesMenuItem_Click(object sender, EventArgs e)
        {
            Globals.formManager.SetViewMode(Globals.ViewMode.entities);
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            EditForm editForm = null;
            var menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                switch (menuItem.Text)
                {
                    case "Group":
                        editForm = new EditForm(EditForm.EditFormType.Group);
                        break;
                    case "General":
                        editForm = new EditForm(EditForm.EditFormType.General);
                        break;
                    case "Map":
                        editForm = new EditForm(EditForm.EditFormType.Map);
                        break;
                }

                editForm?.ShowDialog();
            }
        }
    }
}
