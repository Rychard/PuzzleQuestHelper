using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PuzzleQuestHelper.Logic
{
    public static class ColorExtensions
    {
        private static Color _red = Color.FromArgb(184, 52, 46);
        private static Color _yellow = Color.FromArgb(191, 152, 41);
        private static Color _blue = Color.FromArgb(42, 127, 211);
        private static Color _green = Color.FromArgb(56, 169, 34);
        private static Color _purple = Color.FromArgb(115, 36, 185);
        private static Color _glove = Color.FromArgb(87, 115, 107);
        private static Color _skull = Color.FromArgb(186, 167, 140);
        private static Color _skull5 = Color.FromArgb(147, 96, 84);
        //private static Color _crystal = Color.FromArgb(136, 139, 119);
        private static Color _crystal = Color.FromArgb(102, 89, 81);


        private static int maximumDiff = 0;
        public static TokenType ToTokenType(this Color color)
        {
            var distances = color.GetDistances();
            var closestColor = distances.OrderBy(obj => obj.Value).First();

            if (maximumDiff <= closestColor.Value)
            {
                maximumDiff = closestColor.Value;
            }

            if (closestColor.Value > 25)
            {
                //throw new Exception();
                Console.WriteLine("Unknown Color: #{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
                return TokenType.Unknown;
            }

            var closestColorName = closestColor.Key;
            var tokenType = GetTokenTypeByName(closestColorName);
            return tokenType;
        }

        private static TokenType GetTokenTypeByName(String name)
        {
            switch (name)
            {
                case "red":
                    return TokenType.Red;
                case "yellow":
                    return TokenType.Yellow;
                case "blue":
                    return TokenType.Blue;
                case "green":
                    return TokenType.Green;
                case "purple":
                    return TokenType.Purple;
                case "glove":
                    return TokenType.Glove;
                case "skull":
                    return TokenType.Skull;
                case "skull5":
                    return TokenType.Skull5;
                case "crystal":
                    return TokenType.Crystal;
                default:
                    return TokenType.Unknown;
            }
        }

        private static Dictionary<String, int> GetDistances(this Color color)
        {
            Dictionary<String, int> distances = new Dictionary<String, int>
            {
                { "red", GetDistance(color, _red) },
                { "yellow", GetDistance(color, _yellow) },
                { "blue", GetDistance(color, _blue) },
                { "green", GetDistance(color, _green) },
                { "purple", GetDistance(color, _purple) },
                { "glove", GetDistance(color, _glove) },
                { "skull", GetDistance(color, _skull) },
                { "skull5", GetDistance(color, _skull5) },
                { "crystal", GetDistance(color, _crystal) }
            };
            return distances;
        }

        private static int GetDistance(Color a, Color b)
        {
            var distance = Math.Abs(a.R - b.R) * 0.3f + Math.Abs(a.G - b.G) * 0.59f + Math.Abs(a.B - b.B) * 0.11f;
            return (int)distance;
        }

    }
}
