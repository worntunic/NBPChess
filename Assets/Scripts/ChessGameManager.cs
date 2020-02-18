using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    public class ChessGameManager : MonoBehaviour
    {
        public Board board;
        public PieceManager pieceManager;
        public ChessArtSet artSet;

        public void Awake()
        {
            CreateGame();
        }

        public void CreateGame()
        {
            board.Initalize(artSet);
            pieceManager.Initalize(board, artSet);
        }
    }
}

