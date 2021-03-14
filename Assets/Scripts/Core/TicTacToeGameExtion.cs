using System.Collections.Generic;

public static class TicTacToeGameExtion
{
    private static System.Random random = new System.Random();

    public static Piece[] GetAvailablePoints(this Piece[,] board)
    {
        List<Piece> availablePoints = new List<Piece>();
        int boardSize = board.GetLength(0);

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j].PieceType == PieceType.Empty)
                {
                    availablePoints.Add(board[i, j]);
                }
            }
        }

        return availablePoints.ToArray();
    }

    public static void Shuffle(this Piece[] arr)
    {
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            int rand = random.Next(0, arr.Length - i);
            var temp =  arr[rand];
            arr[rand] = arr[i];
            arr[i] = temp;
            
        }
    }

    public static bool GetWinner(this Piece[,] board, PieceType pieceType, int targetSum)
    {
        if (HasHorizontalVertical(board, pieceType, targetSum) || HasDioganal(board, pieceType, targetSum))
        {
            return true;
        }

        return false;
    }

    private static bool HasHorizontalVertical(Piece[,] board, PieceType cellType, int targetSum)
    {
        int horizontalSum, verticalSum, boardSize = board.GetLength(0);

        for (int i = 0; i < boardSize; i++)
        {
            horizontalSum = verticalSum = 0;

            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j].PieceType == cellType)
                {
                    horizontalSum++;
                    if (horizontalSum >= targetSum)
                    {
                        return true;
                    }
                }
                else horizontalSum = 0;

                if (board[j, i].PieceType == cellType)
                {
                    verticalSum++;
                    if (verticalSum >= targetSum)
                    {
                        return true;
                    }
                }
                else verticalSum = 0;
            }
        }

        return false;
    }

    private static bool HasDioganal(Piece[,] board, PieceType cellType, int targetSum)
    {
        int sumDig45, sumDigN45, boardSize = board.GetLength(0);

        for (int i = boardSize - 1; i >= 0; i--)
        {
            sumDig45 = 0;
            sumDigN45 = 0;
            int y = i;

            for (int j = 0; j < boardSize - i; j++)
            {
                if (board[y, j].PieceType == cellType)
                {
                    sumDig45++;
                    if (sumDig45 >= targetSum)
                    {
                        return true;
                    }
                }
                else sumDig45 = 0;

                if (board[j, y].PieceType == cellType)
                {
                    sumDigN45++;
                    if (sumDigN45 >= targetSum)
                    {
                        return true;
                    }
                }
                else sumDigN45 = 0;

                y++;
            }
        }

        for (int i = boardSize - 1; i >= 0; i--)
        {
            sumDig45 = 0;
            sumDigN45 = 0;
            int y = i;

            for (int j = boardSize - 1; j >= i; j--)
            {
                if (board[j, y].PieceType == cellType)
                {
                    sumDig45++;
                    if (sumDig45 >= targetSum)
                    {
                        return true;
                    }
                }
                else sumDig45 = 0;

                if (board[boardSize - y - 1, boardSize - j - 1].PieceType == cellType)
                {
                    sumDigN45++;
                    if (sumDigN45 >= targetSum)
                    {
                        return true;
                    }
                }
                else sumDigN45 = 0;

                y++;
            }
        }

        return false;
    }
}
