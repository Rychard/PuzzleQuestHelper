using System.Drawing;

namespace PuzzleQuestHelper.Logic
{
    public class GameBoardBitmapMapper
    {
        private readonly GameBoard _board;
        private Bitmap _bmp;

        public GameBoardBitmapMapper(GameBoard board, Bitmap bmp)
        {
            _board = board;
            SetBitmap(bmp);
        }

        public void SetBitmap(Bitmap bmp)
        {
            _bmp = bmp;
            UpdateGameBoard();
        }

        private void UpdateGameBoard()
        {
            int rows = _board.Rows;
            int columns = _board.Columns;

            BitmapDivider divider = new BitmapDivider(_bmp, rows, columns);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var bmpRegion = divider.GetRegion(row, column);
                    var color = bmpRegion.GetAverageColor();

                    var tokenType = color.ToTokenType();
                    _board.SetTokenType(row, column, tokenType);
                }
            }
        }
    }
}
