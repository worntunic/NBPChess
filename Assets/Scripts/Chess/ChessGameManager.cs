using NBPChess.Lobby.UI;
using NBPChess.UI;
using NBPChess.Web;
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
        public WebUIController webController;
        public Board board;
        public PieceManager pieceManager;
        public PawnPromotionUI pawnPromotionUI;
        public MoveHistoryUI moveHistoryUI;
        public EndGameController endGameController;
        private MoveManager moveManager;
        public ChessArtSet[] artSets;
        public int newArtSet;
        private int currentArtSet;
        private GameState gameState;
        public bool localGame = false;
        public PieceColor localPlayerColor = PieceColor.White;
        private int onlineGameID;
        public bool createOnAwake = false;
        public PlayerInfoPanel blackPlayerPanel, whitePlayerPanel;
        private PlayerData localPlayerData, oppPlayerData;

        public void Awake()
        {
            currentArtSet = newArtSet;
            if (createOnAwake)
            {
                CreateGame();
            }
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
            pawnPromotionUI.Initialize(artSets[currentArtSet], moveManager);
            pieceManager.Initalize(board, artSets[currentArtSet], moveManager);
            moveHistoryUI.Initialize(moveManager);
            endGameController.CloseVictoryScreen();

            moveManager.afterMovePlayed += MovePlayed;
        }

        public void ChangeArt()
        {
            if (currentArtSet != newArtSet)
            {
                currentArtSet = newArtSet;
                board.ChangeArtSet(artSets[currentArtSet]);
                pieceManager.ChangeArtSet(artSets[currentArtSet]);
                pawnPromotionUI.ChangeArtSet(artSets[currentArtSet]);
            }
        }

        public void MovePlayed(ChessMove move, string anMove, bool newMove)
        {
            ChangeGameState();
            if (!localGame && newMove)
            {
                webController.PlayMove(onlineGameID, anMove, gameState);
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
            }
            else if (whiteMoveCount == 0)
            {
                gameState = GameState.BlackWin;
                PlayerWon(PieceColor.Black);
            }
            else if (blackMoveCount == 0)
            {
                gameState = GameState.WhiteWin;
                PlayerWon(PieceColor.White);
            }
            else if (gameState == GameState.WhiteMove)
            {
                gameState = GameState.BlackMove;
            }
            else
            {
                gameState = GameState.WhiteMove;
            }
        }

        public void UpdatePlayerInfo(GameInfoWithMoves gameInfo)
        {
            bool whiteMove = GetGameState() == GameState.WhiteMove;
            whitePlayerPanel.Setup(gameInfo.gamedata.wplayername, gameInfo.gamedata.wtimeleft, gameInfo.gamedata.wplayerrank, whiteMove);
            blackPlayerPanel.Setup(gameInfo.gamedata.bplayername, gameInfo.gamedata.btimeleft, gameInfo.gamedata.bplayerrank, !whiteMove);
        }

        private void TieOccurred()
        {
            endGameController.OpenTieScreen();
        }

        private void PlayerWon(PieceColor winnerColor)
        {
            if (!localGame)
            {
                endGameController.OpenVictoryScreen(localPlayerColor == winnerColor);
            } else
            {
                endGameController.OpenVictoryScreen(true);
            }
            //endGameController.OpenVictoryScreen(winnerColor);
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public bool CanPieceMove(PieceColor color, bool newMove)
        {
            bool canPieceMove = true;
            canPieceMove = (color == PieceColor.White && GetGameState() == GameState.WhiteMove)
            || (color == PieceColor.Black && GetGameState() == GameState.BlackMove);
            if (!localGame && newMove)
            {
                canPieceMove = canPieceMove && color == localPlayerColor;
            }
            return canPieceMove;
        }

        public Piece CreateNewPiece(PieceType type, PieceColor color, Row row, Column col)
        {
            return pieceManager.CreatePiece(color, row, col, type);
        }

        public void LoadOnlineGame(GameInfoWithMoves gameInfo, PlayerData localPlayerData)
        {
            localGame = false;
            onlineGameID = gameInfo.id;
            //Determine local color
            if (localPlayerData.id == gameInfo.gamedata.wplayer)
            {
                localPlayerColor = PieceColor.White;
            } else
            {
                localPlayerColor = PieceColor.Black;
            }

            //Get opponent info (TODO)
            //Insert moves
            CreateGame();
            PieceColor[] colors = new PieceColor[] { PieceColor.White, PieceColor.Black };
            for (int i = 0; i < gameInfo.moves.Count; ++i)
            {
                AddMove(gameInfo.moves[i], colors[i % 2]);
            }
            if (!CanPieceMove(localPlayerColor, false))
            {
                webController.WaitForGameState(onlineGameID);
            }
            UpdatePlayerInfo(gameInfo);
        }

        public void AddMove(string algebraicMove, PieceColor player)
        {
            ChessMove move = AlgebraicNotation.ToChessMove(algebraicMove, moveManager, player);
            moveManager.DoMove(move, false, false);
        }

        public void UpdateGameForOpponent(GameInfoWithMoves gameInfo)
        {
            ChessMove move = AlgebraicNotation.ToChessMove(gameInfo.moves[gameInfo.moves.Count - 1], moveManager, GetOpponentColor());
            moveManager.DoMove(move, false, false);
            UpdatePlayerInfo(gameInfo);
        }

        private PieceColor GetOpponentColor()
        {
            if (localPlayerColor == PieceColor.White)
            {
                return PieceColor.Black;
            } else
            {
                return PieceColor.White;
            }
        }
    }
}

