using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    public class ChessGameManager : MonoBehaviour
    {
        public Board board;
        public PieceManager pieceManager;
        private MoveManager moveManager;
        public ChessArtSet[] artSets;
        public int newArtSet;
        private int currentArtSet;

        public void Awake()
        {
            currentArtSet = newArtSet;
            CreateGame();
        }

        private void Update()
        {
            ChangeArt();
        }
        public void CreateGame()
        {
            board.Initalize(artSets[currentArtSet]);
            moveManager = new MoveManager(board);
            pieceManager.Initalize(board, artSets[currentArtSet], moveManager);
        }

        public void ChangeArt()
        {
            if (currentArtSet != newArtSet)
            {
                currentArtSet = newArtSet;
                board.ChangeArtSet(artSets[currentArtSet]);
                pieceManager.ChangeArtSet(artSets[currentArtSet]);
            }
        }
    }
}

