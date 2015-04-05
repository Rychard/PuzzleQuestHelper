using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleQuestHelper.Logic
{
    public class GameBoardToken
    {
        public GameBoard GameBoard { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public TokenType TokenType { get; set; }

        public Boolean CanSwapDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return (Row > 0);
                case Direction.Down:
                    return (Row < GameBoard.Rows - 1);
                case Direction.Left:
                    return (Column > 0);
                case Direction.Right:
                    return (Column < GameBoard.Columns - 1);
                default:
                    return false;
            }
        }

        public SwapOperation GetSwapOperation(Direction direction)
        {
            if (!CanSwapDirection(direction)) { return null; }
            
            int toRow = Row;
            int toCol = Column;

            switch (direction)
            {
                case Direction.Up:
                    toRow--;
                    break;
                case Direction.Down:
                    toRow++;
                    break;
                case Direction.Left:
                    toCol--;
                    break;
                case Direction.Right:
                    toCol++;
                    break;
                default:
                    return null;
            }

            SwapOperation swapOperation = new SwapOperation(Row, Column, toRow, toCol);
            return swapOperation;
        }

        public Boolean HasMatchingNeighbors
        {
            get { return GetMatchingNeighbors().Any(); }
        }

        private GameBoardToken[] _matchingNeighbors; 
        public IEnumerable<GameBoardToken> GetMatchingNeighbors()
        {
            // Only compute neighbors once.
            if (_matchingNeighbors != null)
            {
                foreach (var matchingNeighbor in _matchingNeighbors)
                {
                    yield return matchingNeighbor;
                }
            }

            List<GameBoardToken> matchingNeighbors = new List<GameBoardToken>(4);

            var neighborUp = GetNeighboringToken(Direction.Up);
            if (TokenType == neighborUp.TokenType) { yield return neighborUp; matchingNeighbors.Add(neighborUp); }
            
            var neighborDown = GetNeighboringToken(Direction.Down);
            if (TokenType == neighborDown.TokenType) { yield return neighborDown; matchingNeighbors.Add(neighborDown); }
            
            var neighborLeft = GetNeighboringToken(Direction.Left);
            if (TokenType == neighborLeft.TokenType) { yield return neighborLeft; matchingNeighbors.Add(neighborLeft); }

            var neighborRight = GetNeighboringToken(Direction.Right);
            if (TokenType == neighborRight.TokenType) { yield return neighborRight; matchingNeighbors.Add(neighborRight); }

            _matchingNeighbors = matchingNeighbors.ToArray();
        }

        public GameBoardToken GetNeighboringToken(Direction direction)
        {
            if (!CanSwapDirection(direction)) { return null; }

            int row = Row;
            int column = Column;

            switch (direction)
            {
                case Direction.Up:
                    row--;
                    break;
                case Direction.Down:
                    row++;
                    break;
                case Direction.Left:
                    column--;
                    break;
                case Direction.Right:
                    column++;
                    break;
                default:
                    return null;
            }

            var neighbor = GameBoard.GetToken(row, column);
            return neighbor;
        }

        public static Direction GetDirection(int col, int row, int col2, int row2)
        {
            // We subtract the values of the current instance from the "neighbor"
            // The order is important.
            // Negative values indicate Direction.Left or Direction.Up.
            // Positive values indicate Direction.Right or Direction.Down.
            var rowDifference = row2 - row;
            var columnDifference = col2 - col;

            // If the difference between rows or columns is greater than 1 in either direction, it's not a neighbor.
            if (rowDifference < -1 || rowDifference > 1) { return Direction.Unknown; }
            if (columnDifference < -1 || columnDifference > 1) { return Direction.Unknown; }

            // If the "neighbor" is positioned diagonally from this instance, it's not actually a neighbor.
            if (rowDifference != 0 && columnDifference != 0) { return Direction.Unknown; }

            if (rowDifference < 0) { return Direction.Up; }
            if (rowDifference > 0) { return Direction.Down; }

            if (columnDifference < 0) { return Direction.Left; }
            if (columnDifference > 0) { return Direction.Right; }

            // If we're here, then the "neighbor" must be the current instance.
            return Direction.Unknown;
        }

        public Direction GetDirectionToNeighbor(GameBoardToken neighbor)
        {
            return GetDirection(Column, Row, neighbor.Column, neighbor.Row);
        }

        public int GetMatchCount(Direction direction)
        {
            // If the current instance is not a valid game token (it was removed or otherwise invalid), then don't even try to continue;
            if (TokenType.HasFlag(TokenType.None) || TokenType.HasFlag(TokenType.Unknown))
            {
                return 0; 
            }
            
            GameBoardToken currentToken = this;
            int elementsInMatchSet = 0;
            
            while (currentToken != null && (this.TokenType.HasFlag(currentToken.TokenType)))
            {
                elementsInMatchSet++;
                currentToken = currentToken.GetNeighboringToken(direction);
            }
            return elementsInMatchSet;
        }
    }
}
