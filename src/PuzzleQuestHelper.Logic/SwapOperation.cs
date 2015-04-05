using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleQuestHelper.Logic
{
    public class SwapOperation
    {
        public int FromRow { get; set; }
        public int FromColumn { get; set; }
        public int ToRow { get; set; }
        public int ToColumn { get; set; }

        public SwapOperation()
        {
            
        }

        public SwapOperation(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            FromRow = fromRow;
            FromColumn = fromColumn;
            ToRow = toRow;
            ToColumn = toColumn;
        }

    }
}
