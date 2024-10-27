using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public class CanvasPanelManager
    {
        public Panel canvasPanel;
        private const int CellSize = 50; // Size of each grid cell
        private const int VirtualWidth = 5000; // Virtual canvas width
        private const int VirtualHeight = 5000; // Virtual canvas height
        private Size Size;

        public List<Instrument> entities;
        public Tile[,] tiles; // 2D array to store Tile objects

        public Instrument selectedInstrument; // Field to track the selected entity

        public CanvasPanelManager()
        {
            Size = new Size(Globals.ScreenSize.Width - 200, Globals.ScreenSize.Height - 300 - 20); // Viewport size in pixels
            Initialize();
        }

        public void Initialize()
        {
            // Initialize canvas panel
            this.canvasPanel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 20),
                Size = Size,
                AllowDrop = true,
                AutoScroll = true // Enable auto scrolling
            };
            this.canvasPanel.Paint += CanvasPanel_Paint;
            this.canvasPanel.DragEnter += CanvasPanel_DragEnter;
            this.canvasPanel.DragDrop += CanvasPanel_DragDrop;
            this.canvasPanel.Scroll += CanvasPanel_Scroll;
            this.canvasPanel.MouseClick += CanvasPanel_MouseClick; // Add MouseClick event handler

            // Set the minimum size for the auto scroll area
            this.canvasPanel.AutoScrollMinSize = new Size(VirtualWidth, VirtualHeight);

            // Set focus to the panel to ensure it can receive key events
            this.canvasPanel.Focus();

            InitializeTiles();
            InitializeEntities();
        }

        private void InitializeEntities()
        {
            entities = new List<Instrument>();
        }

        private void InitializeTiles()
        {
            // Initialize the entity array with null values
            int rows = VirtualHeight / CellSize;
            int cols = VirtualWidth / CellSize;
            tiles = new Tile[cols, rows];

            Random rand = new Random();
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (rand.NextDouble() < 0.5) // 50% chance to place a entity
                    {
                        int id = rand.Next(1, 10);

                        // Create a Tile and add to the array
                        var tile = new Tile(new Point(x * CellSize, y * CellSize), id);
                        tiles[x, y] = tile;
                        tiles[x, y].mapPosition = new Point(x, y);

                        // Optionally, initialize or load image data for each Tile
                        // entity.images = LoadImage(id);
                    }
                }
            }
        }



        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int scrollX = -canvasPanel.AutoScrollPosition.X;
            int scrollY = -canvasPanel.AutoScrollPosition.Y;
            int visibleWidth = canvasPanel.ClientSize.Width;
            int visibleHeight = canvasPanel.ClientSize.Height;

            DrawTiles(g, scrollX, scrollY, visibleWidth, visibleHeight);
            DrawEntities(g, scrollX, scrollY, visibleWidth, visibleHeight);
            DrawGrid(g, scrollX, scrollY, visibleWidth, visibleHeight);
        }

        private void DrawTiles(Graphics g, int scrollX, int scrollY, int visibleWidth, int visibleHeight)
        {
            float tileTransparency = Globals.currentViewMode == Globals.ViewMode.entities ? 0.5f : 1.0f;

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    DrawInstrument(tiles[x, y], g, scrollX, scrollY, visibleWidth, visibleHeight, tileTransparency);
                }
            }
        }



        private void DrawEntities(Graphics g, int scrollX, int scrollY, int visibleWidth, int visibleHeight)
        {
            float tileTransparency = Globals.currentViewMode == Globals.ViewMode.tiles ? 0.5f : 1.0f;

            for (int i = 0; i < entities.Count; i++)
            {
                DrawInstrument(entities[i], g, scrollX, scrollY, visibleWidth, visibleHeight, tileTransparency);
            }
        }

        public void DrawInstrument(Instrument instrument, Graphics g, int scrollX, int scrollY, int visibleWidth, int visibleHeight, float transparency)
        {

            if (instrument != null)
            {
                Point tilePosition = new Point((instrument.mapPosition.X * CellSize) - scrollX, (instrument.mapPosition.Y * CellSize) - scrollY);
                instrument.Draw(g, tilePosition, CellSize, transparency);

                if (instrument == selectedInstrument)
                {
                    using (SolidBrush selectionBrush = new SolidBrush(Color.FromArgb(128, Color.Blue)))
                    {
                        Rectangle selectionRect = new Rectangle(tilePosition, new Size(CellSize, CellSize));
                        g.FillRectangle(selectionBrush, selectionRect);
                    }
                }
            }
        }



        private void DrawGrid(Graphics g, int scrollX, int scrollY, int visibleWidth, int visibleHeight)
        {
            Pen pen = new Pen(Color.Gray, 1);
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);

            int startX = (scrollX / CellSize) * CellSize;
            int startY = (scrollY / CellSize) * CellSize;

            for (int x = startX; x <= VirtualWidth; x += CellSize)
            {
                g.DrawLine(pen, x - scrollX, 0, x - scrollX, visibleHeight);
                if (x - scrollX >= 0 && x - scrollX <= visibleWidth)
                {
                    g.DrawString((x / CellSize).ToString(), font, brush, new PointF(x - scrollX, 0));
                }
            }

            for (int y = startY; y <= VirtualHeight; y += CellSize)
            {
                g.DrawLine(pen, 0, y - scrollY, visibleWidth, y - scrollY);
                if (y - scrollY >= 0 && y - scrollY <= visibleHeight)
                {
                    g.DrawString((y / CellSize).ToString(), font, brush, new PointF(0, y - scrollY));
                }
            }
        }



        private void CanvasPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void CanvasPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                PictureBox originalInstrument = (PictureBox)e.Data.GetData(typeof(PictureBox));
                Instrument originalInstrumentData = (Instrument)originalInstrument.Tag;

                if (originalInstrumentData != null && originalInstrumentData.image != null)
                {
                    Point dropPoint = canvasPanel.PointToClient(new Point(e.X, e.Y));
                    int scrollX = canvasPanel.AutoScrollPosition.X;
                    int scrollY = canvasPanel.AutoScrollPosition.Y;
                    dropPoint.Offset(-scrollX, -scrollY);
                    dropPoint = SnapToGrid(dropPoint);

                    int xIndex = dropPoint.X / CellSize;
                    int yIndex = dropPoint.Y / CellSize;


                    if (originalInstrumentData.type == Instrument.InstrumentType.Tile)
                    {
                        // Check bounds before creating a new entity
                        if (xIndex >= 0 && xIndex < tiles.GetLength(0) && yIndex >= 0 && yIndex < tiles.GetLength(1))
                        {
                            // Create a new Tile and store it
                            var newTile = new Tile(new Point(dropPoint.X, dropPoint.Y), originalInstrumentData.id)
                            {
                                image = originalInstrumentData.image
                            };
                            tiles[xIndex, yIndex] = newTile;
                            tiles[xIndex, yIndex].mapPosition = new Point(xIndex, yIndex);
                            ReplacePictureBox(dropPoint);
                        }
                    }
                    else
                    {
                        entities.Add(new Companion(new Point(xIndex, yIndex), originalInstrumentData.id));
                        //ReplacePictureBox(dropPoint);
                    }

                    

                }
            }

            canvasPanel.Invalidate();
        }


        public void ReplacePictureBox(Point dropPoint)
        {
            // Remove existing PictureBox if present
            PictureBox existingPictureBox = canvasPanel.Controls.OfType<PictureBox>()
                .FirstOrDefault(pb => pb.Bounds.Contains(dropPoint));

            if (existingPictureBox != null)
            {
                canvasPanel.Controls.Remove(existingPictureBox);
                existingPictureBox.Dispose();
            }
        }

        private void CanvasPanel_Scroll(object sender, ScrollEventArgs e)
        {
            // Revalidate and redraw the panel
            canvasPanel.Invalidate();
        }



        private void CanvasPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Convert the mouse click position to grid coordinates
            int scrollX = -canvasPanel.AutoScrollPosition.X;
            int scrollY = -canvasPanel.AutoScrollPosition.Y;
            int x = (e.X + scrollX) / CellSize;
            int y = (e.Y + scrollY) / CellSize;

            if (e.Button == MouseButtons.Left)
            {
                if (Globals.currentViewMode == Globals.ViewMode.tiles)
                {
                    // Ensure coordinates are within bounds
                    if (x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1))
                    {
                        Tile clickedTile = tiles[x, y];
                        if (clickedTile != null)
                        {
                            SelectInstrument(clickedTile);
                        }
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (entity != null && entity.mapPosition == new Point(x, y))
                        {
                            SelectInstrument(entity);
                            break; // Exit loop once we find and handle the entity
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Globals.currentViewMode == Globals.ViewMode.tiles)
                {
                    // Ensure coordinates are within bounds
                    if (x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1))
                    {
                        Tile clickedTile = tiles[x, y];
                        if (clickedTile != null)
                        {
                            // Delete the tile
                            tiles[x, y] = null;
                            ClearInstrument();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        if (entities[i] != null && entities[i].mapPosition == new Point(x, y))
                        {
                            // Delete the entity
                            entities.RemoveAt(i);
                            ClearInstrument();
                            break;
                        }
                    }
                }
            }
        }


        public void SelectInstrument(Instrument instrument)
        {
            if (selectedInstrument == instrument)
            {
                // Deselect if the same entity is clicked again
                selectedInstrument = null;
                Globals.rightPanelManager.ClearInfoDisplay();
            }
            else
            {
                // Select the clicked entity
                selectedInstrument = instrument;
                Globals.rightPanelManager.DisplayInfo();
            }

            // Repaint the panel to update selection
            canvasPanel.Invalidate();
        }


        public void ClearInstrument()
        {
            selectedInstrument = null;
            Globals.rightPanelManager.ClearInfoDisplay();
            canvasPanel.Invalidate();
        }



        public void ReplaceInstrument(Instrument instrument, Point newPoint)
        {
            if (instrument.type == Instrument.InstrumentType.Tile)
            {
                ReplaceTile((Tile)instrument, newPoint);
            }
            else
            {
                ReplaceEntity(instrument, newPoint);
            }
        }

        public void ReplaceTile(Tile tile, Point newPoint)
        {
            Globals.canvasPanelManager.tiles[tile.mapPosition.X, tile.mapPosition.Y] = null;
            tile.mapPosition = newPoint;
            Globals.canvasPanelManager.tiles[tile.mapPosition.X, tile.mapPosition.Y] = tile;
        }


        public void ReplaceEntity(Instrument entity, Point newPoint)
        {
            entity.mapPosition = newPoint;
        }





        // Method to snap a point to the nearest grid cell
        private Point SnapToGrid(Point point)
        {
            int x = (point.X / CellSize) * CellSize;
            int y = (point.Y / CellSize) * CellSize;
            return new Point(x, y);
        }

    }
}
