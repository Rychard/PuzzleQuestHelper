using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleQuestHelper.Logic
{
    public class BitmapDivider
    {
        private readonly Bitmap _bmp;
        private readonly int _rows;
        private readonly int _columns;

        public BitmapDivider(Bitmap bmp, int rows, int columns)
        {
            _bmp = bmp;
            _rows = rows;
            _columns = columns;
        }

        public Bitmap GetRegion(int row, int column)
        {
            if (_bmp == null) { return null; }

            int regionWidth = (_bmp.Height / _rows);
            int regionHeight = (_bmp.Width / _columns);

            Bitmap bmpRegion = new Bitmap(regionWidth, regionHeight);

            int columnOffset = column * regionWidth;
            int rowOffset = row * regionHeight;
            Rectangle sourceRectangle = new Rectangle(columnOffset, rowOffset, regionWidth, regionHeight);

            Rectangle destinationRectangle = new Rectangle(0, 0, regionWidth, regionHeight);

            using (Graphics g = Graphics.FromImage(bmpRegion))
            {
                g.DrawImage(_bmp, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }   

            return bmpRegion;
        }
    }
}
