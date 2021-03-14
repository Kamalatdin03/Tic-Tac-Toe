using UnityEngine;

public class HumanPlayer : MonoBehaviour, IPlayer
{
    [SerializeField] GameObject backDrop;

    public void Move(PieceType pieceType)
    {
        backDrop.SetActive(false);
    }
}
