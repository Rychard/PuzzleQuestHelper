namespace PuzzleQuestHelper.Logic
{
    public class SwapOperationResult
    {
        public SwapOperation SwapOperation { get; set; }
        public int MatchingTokens { get; set; }
        public int ChainReactions { get; set; }

        private SwapOperationResult()
        {
            
        }

        public SwapOperationResult(SwapOperation swapOperation, int matchingTokens, int chainReactions)
        {
            SwapOperation = swapOperation;
            MatchingTokens = matchingTokens;
            ChainReactions = chainReactions;
        }
    }
}
