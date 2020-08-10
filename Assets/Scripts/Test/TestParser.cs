using NBPChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParser : MonoBehaviour
{
    public ChessGameManager gameManager;
    public string move;
    public PieceColor pieceColor;

    [ContextMenu("Play Move")]
    public void PlayMove()
    {
        gameManager.AddMove(move, pieceColor);
        if (pieceColor == PieceColor.White)
        {
            pieceColor = PieceColor.Black;
        }
        else
        {
            pieceColor = PieceColor.White;
        }
    }
}
