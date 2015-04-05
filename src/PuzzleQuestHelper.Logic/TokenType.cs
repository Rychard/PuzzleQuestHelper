
using System;

namespace PuzzleQuestHelper.Logic
{
    [Flags]
    public enum TokenType
    {
        Unknown = 1,
        None = 1,
        Red = 2,
        Yellow = 4,
        Blue = 8,
        Green = 16,
        Purple = 32,
        Glove = 64,
        Skull = 128,
        Skull5 = 128,
        Crystal = 512, //Red | Yellow | Blue | Green | Purple,
    }
}
