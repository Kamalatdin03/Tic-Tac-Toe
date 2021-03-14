using UnityEngine;
using TMPro;

public class TicTacToeGame : MonoBehaviour
{
    #region Properties
    public static TicTacToeGame Instance;

    [SerializeField] GameObject p1, p2;
    [SerializeField] Piece piecePrefab;
    [SerializeField] GameObject backDrop;
    [SerializeField] SwitchToggle turnIndicator;
    [SerializeField] UIController uIController;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GridContainer gridContainer;

    Piece[,] board;
    TicTacToeOption option;
    PoolManager poolManager;
    Piece[] winCells, checkWinCellOne, checkWinCellTwo;

    bool isMoveCircle;
    int moveCount;

    public Piece[,] Board { get => board; }
    public int TargetSum => option.TargetSum;
    public bool IsMoveCross => isMoveCircle;

    #endregion

    #region Private Methods

    public TicTacToeGame()
    {
        Instance = this;
        option = new TicTacToeOption();
        option.Players = new IPlayer[2];
    }

    private void RemovePieces()
    {
        if (board != null)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    poolManager.DeSpawnPool("Cell", board[i, j].gameObject);
                }
            }
        }
    }

    private void SpawnPieces()
    {
        poolManager = PoolManager.Instance;

        RemovePieces();

        board = new Piece[option.BoardSize, option.BoardSize];

        for (int i = 0; i < option.BoardSize; i++)
        {
            for (int j = 0; j < option.BoardSize; j++)
            {
                board[i, j] = poolManager.SpawnPool<Piece>("Cell", gridContainer.transform);
                board[i, j].Position = new Vector2Int(j, i);
            }
        }

        gridContainer.CalculateChildernPosition(option.BoardSize, option.BoardSize);
    }

    private bool HasHorizontalVertical(Piece[,] board, PieceType cellType)
    {
        int horizontalSum, verticalSum;

        for (int i = 0; i < option.BoardSize; i++)
        {
            horizontalSum = verticalSum = 0;

            for (int j = 0; j < option.BoardSize; j++)
            {
                if (board[i, j].PieceType == cellType)
                {
                    checkWinCellOne[horizontalSum++] = this.board[i, j];
                    if (horizontalSum >= option.TargetSum)
                    {
                        winCells = checkWinCellOne;
                        return true;
                    }
                }
                else horizontalSum = 0;

                if (board[j, i].PieceType == cellType)
                {
                    checkWinCellTwo[verticalSum++] = this.board[j, i];
                    if (verticalSum >= option.TargetSum)
                    {
                        winCells = checkWinCellTwo;
                        return true;
                    }
                }
                else verticalSum = 0;
            }
        }

        return false;
    }

    private bool HasDioganal(Piece[,] board, PieceType cellType)
    {
        int sumDig45, sumDigN45;

        for (int i = option.BoardSize - 1; i >= 0; i--)
        {
            sumDig45 = 0;
            sumDigN45 = 0;
            int y = i;

            for (int j = 0; j < option.BoardSize - i; j++)
            {
                if (board[y, j].PieceType == cellType)
                {
                    checkWinCellOne[sumDig45++] = this.board[y, j];
                    if (sumDig45 >= option.TargetSum)
                    {
                        winCells = checkWinCellOne;
                        return true;
                    }
                }
                else sumDig45 = 0;

                if (board[j, y].PieceType == cellType)
                {
                    checkWinCellTwo[sumDigN45++] = this.board[j, y];
                    if (sumDigN45 >= option.TargetSum)
                    {
                        winCells = checkWinCellTwo;
                        return true;
                    }
                }
                else sumDigN45 = 0;

                y++;
            }
        }

        for (int i = option.BoardSize - 1; i >= 0; i--)
        {
            sumDig45 = 0;
            sumDigN45 = 0;
            int y = i;

            for (int j = option.BoardSize - 1; j >= i; j--)
            {
                if (board[j, y].PieceType == cellType)
                {
                    checkWinCellOne[sumDig45++] = this.board[j, y];
                    if (sumDig45 >= option.TargetSum)
                    {
                        winCells = checkWinCellOne;
                        return true;
                    }
                }
                else sumDig45 = 0;

                if (board[option.BoardSize - y - 1, option.BoardSize - j - 1].PieceType == cellType)
                {
                    checkWinCellTwo[sumDigN45++] = this.board[option.BoardSize - y - 1, option.BoardSize - j - 1];
                    if (sumDigN45 >= option.TargetSum)
                    {
                        winCells = checkWinCellTwo;
                        return true;
                    }
                }
                else sumDigN45 = 0;

                y++;
            }
        }

        return false;
    }

    private bool HasGameEnd(out PieceType type)
    {
        PieceType cellType = isMoveCircle ? PieceType.Cross : PieceType.Circle;
        type = PieceType.Empty;
        moveCount--;

        if (HasHorizontalVertical(board, cellType) || HasDioganal(board, cellType))
        {
            foreach (var item in winCells)
            {
                item.ActiveCell();
            }

            type = cellType;
            return true;
        }

        if (moveCount < 0)
        {
            return true;
        }

        return false;
    }

    private void GameOver(PieceType pieceType)
    {
        string text;
        backDrop.SetActive(true);

        if (pieceType == PieceType.Circle) text = "Win O";
        else if (pieceType == PieceType.Cross) text = "Win X";
        else text = "Draw";

        resultText.text = text;
        Invoke(nameof(AcitveResultPanel), 0.5f);

        SoundManager.Instance.PlaySound(pieceType == PieceType.Empty ? SoundType.Draw : SoundType.Win);
    }

    private void AcitveResultPanel() => uIController.ActivePanelByName("Result");

    private void ResetBoard()
    {
        backDrop.SetActive(false);

        winCells = new Piece[option.TargetSum];
        checkWinCellOne = new Piece[option.TargetSum];
        checkWinCellTwo = new Piece[option.TargetSum];

        isMoveCircle = option.FristMove == PieceType.Cross;
        moveCount = (int)Mathf.Pow(option.BoardSize, 2);

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j].ResetCell();
            }
        }

        SwapTurn();
    }

    #endregion

    #region Public Methods

    public void StartGame()
    {
        SpawnPieces();
        ResetBoard();

        uIController.ActivePanelByName("Main");
    }

    public void ReMatch()
    {
        ResetBoard();

        uIController.ActivePanelByName("Main");
    }

    public void SetPlayers(bool isSingleGame)
    {
        option.Players[0] = p1.GetComponent<IPlayer>();
        option.Players[1] = isSingleGame ? p2.GetComponent<IPlayer>() : option.Players[0];
    }

    public void SetBoardSize(int boardSize)
    {
        option.BoardSize = boardSize;
    }

    public void SetTargetSum(int targetSum)
    {
        option.TargetSum = targetSum;
    }

    public void SetFristMove(bool isCross)
    {
        option.FristMove = isCross ? PieceType.Cross : PieceType.Circle;
    }

    public void SwapTurn()
    {
        turnIndicator.IsOn = (isMoveCircle = !isMoveCircle);

        PieceType pieceType;
        if (!HasGameEnd(out pieceType))
        {
            option.Players[isMoveCircle ? 0 : 1].Move(isMoveCircle ? PieceType.Circle : PieceType.Cross);
            return;
        }

        GameOver(pieceType);
    }

    #endregion

    #region Classes

    sealed class TicTacToeOption
    {
        public int BoardSize { get; set; }
        public int TargetSum { get; set; }
        public PieceType FristMove { get; set; }
        public IPlayer[] Players { get; set; }
    }


    #endregion
}