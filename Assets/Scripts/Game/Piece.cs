using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType
{
    Empty, Circle, Cross
}

public class Piece : MonoBehaviour, IPoolableObject
{
    #region Properties

    [SerializeField] PieceType pieceType;
    [SerializeField] Sprite ticSprite, tacSprite;
    [SerializeField] float animationDuration;
    [SerializeField] Color crossColor, circleColor;

    Image backgroundImage;
    Image iconImage;
    Color fristColor;
    TicTacToeGame game;

    public PieceType PieceType { get => pieceType; set => pieceType = value; }
    public Vector2Int Position { get; set; }
    public Button Button => GetComponent<Button>();

    #endregion

    #region Methods

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        iconImage = transform.GetChild(0).GetComponent<Image>();

        fristColor = backgroundImage.color;
        ResetCell();
    }

    public void ActiveCell()
    {
        Ease ease = Ease.InOutCirc;
        Color childColor = iconImage.color;
        Color imageColor = backgroundImage.color;

        backgroundImage.DOColor(childColor, animationDuration).SetEase(ease);
        iconImage.DOColor(imageColor, animationDuration).SetEase(ease);
    }

    public void OnClick()
    {
        if (pieceType != PieceType.Empty) return;

        bool cType = game.IsMoveCross;
        pieceType = cType ? PieceType.Circle : PieceType.Cross;

        iconImage.gameObject.SetActive(true);
        iconImage.sprite = cType ? tacSprite : ticSprite;
        iconImage.color = cType ? crossColor : circleColor;

        game.SwapTurn();
        SoundManager.Instance.PlaySound(cType ? SoundType.Tic : SoundType.Tac);
    }

    public void ResetCell()
    {
        if (fristColor != Color.clear) backgroundImage.color = fristColor;

        pieceType = PieceType.Empty;
        iconImage.gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        game = TicTacToeGame.Instance;

        ResetCell();
    }

    public void OnDeSpawn()
    {
        
    }

    #endregion
}
