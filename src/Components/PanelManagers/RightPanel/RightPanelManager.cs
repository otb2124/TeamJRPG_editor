using System;
using System.Drawing;
using System.Windows.Forms;
using static TeamJRPG_editor.AttributeTextBox;



namespace TeamJRPG_editor
{
    public class RightPanelManager
    {
        public Panel rightPanel;
        public Control[] table;

        public RightPanelManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Initialize the panel
            rightPanel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(Globals.ScreenSize.Width - 200, 20),
                Size = new Size(200, Globals.ScreenSize.Height - 300), // Adjust height for better fit
                AutoScroll = true
            };
        }


        public void DisplayInfo()
        {
            rightPanel.Controls.Clear();

            string[] labelTexts = new string[GetAttributesByInstrumentType().Length];
            for (int i = 0; i < labelTexts.Length; i++)
            {
                labelTexts[i] = GetAttributesByInstrumentType()[i].ToString();
            }

            int numRows = labelTexts.Length;
            const int labelWidth = 80;
            const int textFieldWidth = 100;
            const int rowHeight = 30;
            const int padding = 2; // Padding between rows

            // Create a new table of controls
            table = new Control[numRows * 2];

            for (int row = 0; row < numRows; row++)
            {
                int y = row * (rowHeight + padding);

                // Create and configure the label
                Label label = new Label
                {
                    Text = labelTexts[row],
                    Size = new Size(labelWidth, rowHeight),
                    Location = new Point(padding, y - 4), // Adjust location to fit within the panel
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // Create and configure the text field
                AttributeTextBox textBox = new AttributeTextBox(GetAttributesByInstrumentType()[row])
                {
                    Size = new Size(textFieldWidth, rowHeight),
                    Location = new Point(label.Right + padding, y) // Position to the right of the label
                };

                // Attach the KeyDown event handler
                textBox.KeyDown += TextBox_KeyDown;

                // Add the controls to the panel
                rightPanel.Controls.Add(label);
                rightPanel.Controls.Add(textBox);

                // Store the controls in the table array
                table[row * 2] = label;
                table[row * 2 + 1] = textBox;
            }
        }


        public void ClearInfoDisplay()
        {
            rightPanel.Controls.Clear();
        }


        public AttributeTextBox.Attribute[] GetAttributesByInstrumentType()
        {
            switch (Globals.canvasPanelManager.selectedInstrument.type)
            {
                case Instrument.InstrumentType.Tile:
                    return new AttributeTextBox.Attribute[] { AttributeTextBox.Attribute.mapPos, AttributeTextBox.Attribute.Type, AttributeTextBox.Attribute.id };
                case Instrument.InstrumentType.Companion:
                    return new AttributeTextBox.Attribute[] { AttributeTextBox.Attribute.mapPos, AttributeTextBox.Attribute.Type, AttributeTextBox.Attribute.id, AttributeTextBox.Attribute.name };
            }

            return null;
        }






        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AttributeTextBox textBox = sender as AttributeTextBox;
                if (textBox != null)
                {
                    SetInstrumentData(textBox);
                    Globals.canvasPanelManager.canvasPanel.Invalidate();
                }
            }
        }



        public void SetInstrumentData(AttributeTextBox box)
        {
            Instrument instr = Globals.canvasPanelManager.selectedInstrument;

            switch (box.attribute)
            {
                case AttributeTextBox.Attribute.mapPos:
                    string[] parts = box.Text.Split(',');
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0].Trim(), out int x) &&
                        int.TryParse(parts[1].Trim(), out int y))
                    {
                        Globals.canvasPanelManager.ReplaceInstrument(instr, new Point(x, y));
                        
                    }
                    break;
                case AttributeTextBox.Attribute.Type:
                    // skip
                    break;
                case AttributeTextBox.Attribute.id:
                    if(int.TryParse(box.Text, out int newId))
                    {
                        instr.id = newId;
                    }
                    break;



                case AttributeTextBox.Attribute.name:
                    ((Companion)instr).name = box.Text;
                    break;
                
            }
        }
    }




    public class AttributeTextBox : TextBox
    {

        public enum Attribute { mapPos, Type, id, name };
        public Attribute attribute;

        public AttributeTextBox(Attribute attribute)
        {
            this.attribute = attribute;
            this.Text = GetDisplayValue();
        }

        public string GetDisplayValue()
        {
            Instrument instr = Globals.canvasPanelManager.selectedInstrument;

            switch (attribute)
            {
                case Attribute.Type:
                    this.Enabled = false;
                    return instr.type.ToString();
                case Attribute.id:
                    return instr.id.ToString();
                case Attribute.mapPos:
                    return instr.mapPosition.X + ", " + instr.mapPosition.Y;
                case Attribute.name:
                    return ((Companion)instr).name;
            }

            return null;
        }

    }
}
