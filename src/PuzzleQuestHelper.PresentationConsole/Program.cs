using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using PuzzleQuestHelper.Logic;

namespace PuzzleQuestHelper.PresentationConsole
{
    public class Program
    {
        private const String PROCESS_NAME = "PuzzleQuest2";

        public static void Main(string[] args)
        {
            GameBoard gameBoard = new GameBoard(8, 8);
            var mapper = new GameBoardBitmapMapper(gameBoard, null);

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    Bitmap bmp = GetGameWindow();
                    mapper.SetBitmap(bmp);

                    String tokenCounts = GetTokenCounts(gameBoard);
                    String strBoard = WriteBoard(gameBoard);
                    String results = PerformCalculations(gameBoard);

                    Console.Clear();
                    Console.WriteLine(tokenCounts);
                    Console.WriteLine(strBoard);
                    Console.WriteLine(results);
                    System.Threading.Thread.Sleep(1000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static string PerformCalculations(GameBoard gameBoard)
        {
            StringBuilder sb = new StringBuilder();

           
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var moves = gameBoard.GetPossibleMoves();

            foreach (var move in moves)
            {
                var swap = move.SwapOperation;
                var tokens = move.MatchingTokens;
                var chains = move.ChainReactions;

                if (swap == null) { continue; }

                var sourceType = gameBoard.GetTokenType(swap.FromRow, swap.FromColumn);
                var targetType = gameBoard.GetTokenType(swap.ToRow, swap.ToColumn);

                var direction = GameBoardToken.GetDirection(swap.FromColumn, swap.FromRow, swap.ToColumn, swap.ToRow);

                var sourceTypeName = Enum.GetName(typeof(TokenType), sourceType);
                var targetTypeName = Enum.GetName(typeof(TokenType), targetType);

                String template = "Swap ({0}, {1}) ({2}) {3} (with {4}) = {5} tokens.";
                if (chains > 0)
                {
                    template = "Swap ({0}, {1}) ({2}) {3} (with {4}) = {5} tokens. (+{6} chains).";
                }
                String line = String.Format(template, swap.FromColumn, swap.FromRow, sourceTypeName, direction, targetTypeName, tokens, chains);
                sb.AppendLine(line);
            }
            var milliseconds = sw.ElapsedMilliseconds;
            sb.AppendLine();
            var timeTaken = String.Format("[{0} {1}] Processing completed in {2}ms.", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), milliseconds);
            sb.AppendLine(timeTaken);

            return sb.ToString();
        }

        private static String GetTokenCounts(GameBoard gameBoard)
        {
            StringBuilder sb = new StringBuilder();
            var counts = gameBoard.GetTokenCounts();
            foreach (var count in counts)
            {
                sb.AppendLine(String.Format("{0}: {1}", count.Key, count.Value));
            }
            return sb.ToString();
        }

        private static String WriteBoard(GameBoard gameBoard)
        {
            int rows = gameBoard.Rows;
            int columns = gameBoard.Columns;
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var tokenType = gameBoard.GetTokenType(row, column);
                    var displayName = Enum.GetName(typeof (TokenType), tokenType) ?? "????";
                    sb.Append(displayName.PadLeft(8));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static Bitmap GetGameWindow()
        {
            // Only works when playing in windowed mode, at a resolution of 1600 x 900.
            var boardRect = new Rectangle(443, 114, 718, 723);
            var bmp = WindowHelper.GetGameWindow(PROCESS_NAME, boardRect);
            return bmp;
        }
    }
}
