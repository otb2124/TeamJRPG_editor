using System.Drawing;

namespace TeamJRPG_editor
{
    public class Companion : Instrument
    {

        public string name;

        public Companion(Point mapPosition, int id) : base(InstrumentType.Companion, id)
        {
            this.mapPosition = mapPosition;
        }
    }
}
