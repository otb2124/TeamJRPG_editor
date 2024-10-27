using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public class EditForm : Form
    {
        public enum EditFormType { Group, General, Map };
        public EditFormType type;

        public EditForm(EditFormType type)
        {
            this.type = type;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Add controls based on the form type
            switch (type)
            {
                case EditFormType.Group:
                    AddGroupControls();
                    break;
                case EditFormType.General:
                    AddGeneralControls();
                    break;
                case EditFormType.Map:
                    AddMapControls();
                    break;
            }

            // Set properties for the form
            this.Text = $"{this.type.ToString()} Edit";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            AdjustFormSize();
        }

        private void AddGroupControls()
        {
            var button1 = new Button
            {
                Text = "Button 1",
                Size = new Size(260, 40),
                Location = new Point(10, 10)
            };

            var button2 = new Button
            {
                Text = "Button 2",
                Size = new Size(260, 40),
                Location = new Point(10, 60)
            };

            this.Controls.Add(button1);
            this.Controls.Add(button2);
        }

        private void AddGeneralControls()
        {
            for (int i = 0; i < 3; i++)
            {
                var label = new Label
                {
                    Text = $"Label {i + 1}:",
                    Location = new Point(10, 10 + i * 30),
                    AutoSize = true
                };

                var textBox = new TextBox
                {
                    Location = new Point(70, 10 + i * 30),
                    Size = new Size(200, 20)
                };

                this.Controls.Add(label);
                this.Controls.Add(textBox);
            }
        }

        private void AddMapControls()
        {
            for (int i = 0; i < 2; i++)
            {
                var label = new Label
                {
                    Text = $"Label {i + 1}:",
                    Location = new Point(10, 10 + i * 30),
                    AutoSize = true
                };

                var textBox = new TextBox
                {
                    Location = new Point(70, 10 + i * 30),
                    Size = new Size(200, 20)
                };

                this.Controls.Add(label);
                this.Controls.Add(textBox);
            }
        }

        private void AdjustFormSize()
        {
            switch (type)
            {
                case EditFormType.Group:
                    this.Size = new Size(300, 150);
                    break;
                case EditFormType.General:
                    this.Size = new Size(300, 150);
                    break;
                case EditFormType.Map:
                    this.Size = new Size(300, 110);
                    break;
            }
        }
    }
}
