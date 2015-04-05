
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleQuestHelper.Logic
{
    public class GameBoard
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly List<GameBoardToken> _tokens;

        public int Rows { get { return _rows; } }
        public int Columns { get { return _columns; } }

        public Dictionary<TokenType, int> GetTokenCounts()
        {
            Dictionary<TokenType, int> count = new Dictionary<TokenType, int>();
            foreach (var token in _tokens)
            {
                if (count.ContainsKey(token.TokenType))
                {
                    count[token.TokenType]++; 
                }
                else
                {
                    count.Add(token.TokenType, 1);
                }
            }
            return count;
        }

        public GameBoard(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _tokens = new List<GameBoardToken>();
            Reset();
        }

        public void Reset()
        {
            _tokens.Clear();

            int tokenCount = (_rows * _columns);
            for (int i = 0; i < tokenCount; i++)
            {
                int row = i / _rows;
                int col = i % _columns;

                var piece = new GameBoardToken
                {
                    GameBoard = this,
                    Row = row,
                    Column = col,
                    TokenType = TokenType.Unknown,
                };

                _tokens.Add(piece);
            }
        }

        public TokenType GetTokenType(int row, int column)
        {
            //var offset = row * _columns + column;
            //return _tokens[offset].TokenType;

            var token = GetToken(row, column);
            return token.TokenType;
        }

        public void SetTokenType(int row, int column, TokenType type)
        {
            //var offset = row * _columns + column;
            //_tokens[offset].TokenType = type;

            var token = GetToken(row, column);
            token.TokenType = type;
        }

        public GameBoardToken GetToken(int row, int column)
        {
            var token = _tokens.Single(obj => obj.Row == row && obj.Column == column);
            return token;
        }

        public GameBoard PreviewMove(SwapOperation swap)
        {
            var copy = GetCopy();
            var from = copy.GetTokenType(swap.FromRow, swap.FromColumn);
            var to = copy.GetTokenType(swap.ToRow, swap.ToColumn);

            copy.SetTokenType(swap.FromRow, swap.FromColumn, to);
            copy.SetTokenType(swap.ToRow, swap.ToColumn, from);

            return copy;
        }

        public IEnumerable<SwapOperationResult> GetPossibleMoves()
        {
            foreach (var token in _tokens)
            {
                // We only need to test swapping in two directions: Right, and Down.
                // Testing any other direction will be performing duplicate work.
                var swapOperationRight = token.GetSwapOperation(Direction.Right);

                if (swapOperationRight != null)
                {
                    var swapOperationRightPreview = PreviewMove(swapOperationRight);
                    foreach (var swapOperationResult in PreviewResults(swapOperationRightPreview, swapOperationRight))
                    {
                        yield return swapOperationResult;
                    }
                }
                
                var swapOperationDown = token.GetSwapOperation(Direction.Down);
                if (swapOperationDown != null)
                {
                    var swapOperationDownPreview = PreviewMove(swapOperationDown);
                    foreach (var swapOperationResult in PreviewResults(swapOperationDownPreview, swapOperationDown))
                    {
                        yield return swapOperationResult;
                    }    
                }
            }
        }

        private IEnumerable<SwapOperationResult> PreviewResults(GameBoard previewBoard, SwapOperation swapOperation, int level = 0)
        {
            foreach (var previewBoardToken in previewBoard._tokens)
            {
                // This should recursively determine the the (deterministic) results of a swap operation.
                // It's not 100%, since we don't know what the new tokens that appear will be.

                var matchCountHorizontal = previewBoardToken.GetMatchCount(Direction.Right);
                if (matchCountHorizontal >= 3 && level < 10)
                {
                    int recurseResultCount = 0;
                    var previewBoardCopy = previewBoardToken.GameBoard.GetCopy();
                    for (int i = 0; i < matchCountHorizontal; i++)
                    {
                        var tokenCopy = previewBoardCopy.GetToken(previewBoardToken.Row, previewBoardToken.Column + i);
                        previewBoardCopy.RemoveToken(tokenCopy);
                    }
                    var recurseResults = PreviewResults(previewBoardCopy, swapOperation, level + 1);

                    foreach (var recurseResult in recurseResults)
                    {
                        yield return recurseResult;
                        matchCountHorizontal += recurseResult.MatchingTokens;
                        recurseResultCount++;
                    }
                    yield return new SwapOperationResult(swapOperation, matchCountHorizontal, recurseResultCount);
                }

                var matchCountVertical = previewBoardToken.GetMatchCount(Direction.Down);
                if (matchCountVertical >= 3 && level < 10)
                {
                    int recurseResultCount = 0;
                    
                    var previewBoardCopy = previewBoardToken.GameBoard.GetCopy();
                    for (int i = 0; i < matchCountVertical; i++)
                    {
                        var tokenCopy = previewBoardCopy.GetToken(previewBoardToken.Row + i, previewBoardToken.Column);
                        previewBoardCopy.RemoveToken(tokenCopy);
                    }
                    var recurseResults = PreviewResults(previewBoardCopy, swapOperation, level + 1);

                    foreach (var recurseResult in recurseResults)
                    {
                        yield return recurseResult;
                        matchCountVertical += recurseResult.MatchingTokens;
                        recurseResultCount++;
                    }
                    yield return new SwapOperationResult(swapOperation, matchCountVertical, recurseResultCount);
                }
            }
        }

        public GameBoard GetCopy()
        {
            GameBoard copy = new GameBoard(_rows, _columns);
            foreach (var token in _tokens)
            {
                copy.SetTokenType(token.Row, token.Column, token.TokenType);
            }
            return copy;
        }

        public void RemoveToken(GameBoardToken token)
        {
            // Removing a token causes all tokens above it to drop into the available space.
            // Iterate over all tokens above this one, and place them into a list.
            // After identifying all appropriate tokens, iterate over each token in the list and swap them with the previous token.
            // When finished, the token to be removed is now the top-most token in the column.
            // Set that token to be "None" to indicate a blank space.
            List<GameBoardToken> tokensToShift = new List<GameBoardToken>();
            var currentToken = token;
            do
            {
                currentToken = currentToken.GetNeighboringToken(Direction.Up);
                if (currentToken != null)
                {
                    tokensToShift.Add(currentToken);
                }
            } while (currentToken != null);

            token.Row = 0;
            token.TokenType = TokenType.None;

            foreach (var shiftToken in tokensToShift)
            {
                shiftToken.Row++;
            }
        }
    }
}
