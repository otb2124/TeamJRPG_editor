using System.Drawing;
using System.Drawing.Imaging;

namespace TeamJRPG_editor
{
    public class Tile : Instrument
    {
        public Tile(Point mapPosition, int id) : base(InstrumentType.Tile, id)
        {
            this.mapPosition = mapPosition;
        }

    }
}
