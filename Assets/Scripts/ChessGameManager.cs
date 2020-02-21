using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    public enum GameState
    {
        WhiteMove, BlackMove, WhiteWin, BlackWin, Tie
    }

    public class ChessGameManager : MonoBehaviour
    {
        public Board board;
        public PieceManager pieceManager;
        private MoveManager moveManager;
        public ChessArtSet[] artSets;
        public int newArtSet;
        private int currentArtSet;
        private GameState gameState;

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
            gameState = GameState.WhiteMove;
            board.Initalize(artSets[currentArtSet]);
            moveManager = new MoveManager(this, board);
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

        public void ChangeGameState()
        {
            int whiteMoveCount, blackMoveCount;
            moveManager.GetMoveCount(out whiteMoveCount, out blackMoveCount);
            if (whiteMoveCount == 0 && blackMoveCount == 0)
            {
                gameState = GameState.Tie;
                TieOccurred();
            } else if (whiteMoveCount == 0)
            {
                gameState = GameState.BlackWin;
                PlayerWon(PieceColor.Black);
            } else if (blackMoveCount == 0)
            {
                gameState = GameState.WhiteWin;
                PlayerWon(PieceColor.White);
            } else if (gameState == GameState.WhiteMove)
            {
                gameState = GameState.BlackMove;
            } else
            {
                gameState = GameState.WhiteMove;
            }
        }

        private void TieOccurred()
        {
            Debug.Log("Tie!");
        }

        private void PlayerWon(PieceColor winnerColor)
        {
            Debug.Log(winnerColor + " won!");
        }

        public GameState GetGameState()
        {
            return gameState;
        }
    }
}

