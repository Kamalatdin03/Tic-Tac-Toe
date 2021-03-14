using UnityEngine;

public class AIPlayer : MonoBehaviour, IPlayer
{
    [SerializeField] GameObject backDrop;
    [SerializeField] float waitSecond = 0.5f;
    [SerializeField] int depth = 3;

    PieceType piece;
    TicTacToeGame game;

    private void Start()
    {
        game = TicTacToeGame.Instance;
    }

    public void Move(PieceType pieceType)
    {
        backDrop.SetActive(true);
        piece = pieceType;

        Invoke(nameof(FindBestMove), waitSecond);
    }

    private void FindBestMove()
    {
        Piece[,] board = game.Board;
        int targetSum = game.TargetSum;

        AIMove bestMove = MinMax(board, piece == PieceType.Cross, targetSum, depth);

        backDrop.SetActive(false);
        TicTacToeGame.Instance.Board[bestMove.Y, bestMove.X].GetComponent<UnityEngine.UI.Button>().onClick?.Invoke();
    }

    private AIMove MinMax(Piece[,] board, bool isMax, int targetSum, int depth)
    {
        var bestMove = new AIMove();
        var availablePoints = board.GetAvailablePoints();

        if (board.GetWinner(isMax ? PieceType.Circle : PieceType.Cross, targetSum))
        {
            bestMove.Score = isMax ? -1 : 1;
            return bestMove;
        }
        else if (availablePoints.Length == 0 || depth == 0)
        {
            bestMove.Score = 0;
            return bestMove;
        }

        bestMove.Score = isMax ? int.MinValue : int.MaxValue;
        availablePoints.Shuffle();

        for (int i = 0; i < availablePoints.Length; i++)
        {
            Vector2Int pos = availablePoints[i].Position;

            board[pos.y, pos.x].PieceType = isMax ? PieceType.Cross : PieceType.Circle;

            AIMove boardState = MinMax(board, !isMax, targetSum, depth - 1);

            if (isMax)
            {
                if (boardState.Score > bestMove.Score)
                {
                    bestMove.Score = boardState.Score;
                    bestMove.Y = pos.y;
                    bestMove.X = pos.x;
                }
            }
            else
            {
                if (boardState.Score < bestMove.Score)
                {
                    bestMove.Score = boardState.Score;
                    bestMove.Y = pos.y;
                    bestMove.X = pos.x;
                }
            }

            board[pos.y, pos.x].PieceType = PieceType.Empty;
        }

        return bestMove;
    }

    sealed class AIMove
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Score { get; set; }
    }
}
