using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System;
using Newtonsoft.Json;

namespace TeamJRPG_editor
{

    [Serializable]
    public class Instrument
    {
        public PictureBox instrumentPictureBox;
        public Image image;
        public Point mapPosition;

        public int id;
        public enum InstrumentType { Tile, Companion, NPC, Mob, DObject, IObject, Event };
        public InstrumentType type;

        private const int InstrumentSize = 40;

        public Instrument(InstrumentType type, int id)
        {
            this.type = type;
            this.id = id;

            Initialize();
        }

        public void Initialize()
        {
            switch (type)
            {
                case InstrumentType.Tile:
                    image = Globals.assetSetter.GetSheet(AssetSetter.SheetCategory.tiles, 0);
                    break;
                case InstrumentType.Companion:
                    image = Globals.assetSetter.GetSheet(AssetSetter.SheetCategory.character_bodies, 0);
                    break;
            }
        }

        public void DrawPictureBox()
        {
            instrumentPictureBox = new PictureBox
            {
                Size = new Size(InstrumentSize, InstrumentSize),
                Location = GetInstrumentPanelLocation(),
                BackColor = Color.Transparent,
                Tag = this
            };

            if (image == null)
            {
                instrumentPictureBox.BackColor = Color.Red;
            }

            instrumentPictureBox.Paint += new PaintEventHandler(Instrument_Paint);
            instrumentPictureBox.MouseDown += new MouseEventHandler(Instrument_MouseDown);
        }

        public Point GetInstrumentPanelLocation()
        {
            int instrumentsPerRow = Globals.instrumentPanelManager.instrumentsPanel.ClientSize.Width / InstrumentSize;
            int x = (id % instrumentsPerRow) * InstrumentSize;
            int y = (id / instrumentsPerRow) * InstrumentSize;
            return new Point(x, y + 20);
        }

        public Rectangle GetSrcRect()
        {
            int tileSize = 32;
            int rowItemCount = image.Width / tileSize;
            int x = (id % rowItemCount) * tileSize;
            int y = (id / rowItemCount) * tileSize;
            return new Rectangle(x, y, tileSize, tileSize);
        }

        private void Instrument_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null && image != null)
            {
                Rectangle srcRect = GetSrcRect();
                Rectangle destRect = new Rectangle(0, 0, pictureBox.Width, pictureBox.Height);
                e.Graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        private void Instrument_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox instrument = sender as PictureBox;
            if (instrument != null)
            {
                instrument.DoDragDrop(instrument, DragDropEffects.Copy);
            }
        }

        public void Draw(Graphics g, Point position, int cellSize, float transparency)
        {
            if (image != null)
            {
                Rectangle srcRect = GetSrcRect();
                Image resizedImage = ResizeImage(image, srcRect, cellSize);
                Rectangle destRect = new Rectangle(position, new Size(cellSize, cellSize));

                if (transparency < 1.0f)
                {
                    ColorMatrix colorMatrix = new ColorMatrix
                    {
                        Matrix33 = transparency
                    };
                    ImageAttributes imageAttributes = new ImageAttributes();
                    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(resizedImage, destRect, 0, 0, resizedImage.Width, resizedImage.Height, GraphicsUnit.Pixel, imageAttributes);
                }
                else
                {
                    g.DrawImage(resizedImage, destRect);
                }
            }
        }

        private Image ResizeImage(Image sourceImage, Rectangle srcRect, int newSize)
        {
            srcRect = Rectangle.Intersect(srcRect, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
            Bitmap resizedImage = new Bitmap(newSize, newSize);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourceImage, new Rectangle(0, 0, newSize, newSize), srcRect, GraphicsUnit.Pixel);
            }
            return resizedImage;
        }
    }
}