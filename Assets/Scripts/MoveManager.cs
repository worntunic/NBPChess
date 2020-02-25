﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NBPChess
{
    public struct ChessMove
    {
        public Tile fromTile;
        public Piece activePiece;
        public Tile toTile;
        public Piece passivePiece;
        public bool isCastleMove;
        public Piece castlePair;
        public Tile castlePairFrom;
        public Tile castlePairTo;
        public Tile passivePieceFromTile;
        public bool isPromotionMove;
        public bool isCompletePromotionMove;
        public PieceType promotionType;
        public Piece promotedPiece;

        public ChessMove(Tile fromTile, Tile toTile)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
            passivePieceFromTile = toTile;
            isCastleMove = false;
            castlePair = null;
            castlePairFrom = null;
            castlePairTo = null;
            isPromotionMove = false;
            isCompletePromotionMove = false;
            promotionType = PieceType.Pawn;
            promotedPiece = null;
        }

        public ChessMove(Tile fromTile, Tile toTile, Piece passivePiece, bool isPromotionMove = false)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = passivePiece;
            this.passivePieceFromTile = passivePiece.GetTile();
            isCastleMove = false;
            castlePair = null;
            castlePairFrom = null;
            castlePairTo = null;
            this.isPromotionMove = isPromotionMove;
            isCompletePromotionMove = false;
            promotionType = PieceType.Pawn;
            promotedPiece = null;
        }

        public ChessMove(Tile fromTile, Tile toTile, Tile castlePairFrom, Tile castlePairTo)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
            this.passivePieceFromTile = toTile;
            isCastleMove = true;
            this.castlePair = castlePairFrom.piece;
            this.castlePairFrom = castlePairFrom;
            this.castlePairTo = castlePairTo;
            isPromotionMove = false;
            isCompletePromotionMove = false;
            promotionType = PieceType.Pawn;
            promotedPiece = null;
        }
        public ChessMove(Tile fromTile, Tile toTile, bool promotionMove)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
            passivePieceFromTile = toTile;
            isCastleMove = false;
            castlePair = null;
            castlePairFrom = null;
            castlePairTo = null;
            isPromotionMove = false;
            isCompletePromotionMove = false;
            promotionType = PieceType.Pawn;
            promotedPiece = null;
        }
        public override string ToString()
        {
            if (isCastleMove)
            {
                return "Castle move: King to " + toTile;
            } else
            {
                string retStr = activePiece.GetPieceType() + " moves from " + fromTile + " to " + toTile;
                if(passivePiece != null)
                {
                    retStr += " and captures " + passivePiece.GetPieceType();
                }
                return retStr;
            }
        }
    }

    public class MoveManager
    {
        private ChessGameManager gameManager;
        private PieceManager pieceManager;
        private Board board;
        private List<Piece> pieces = new List<Piece>();
        private List<ChessMove> moveHistory = new List<ChessMove>();
        private Dictionary<Piece, List<Tile>> whiteThreatenedSpaces = new Dictionary<Piece, List<Tile>>();
        private Dictionary<Piece, List<Tile>> blackThreatenedSpaces = new Dictionary<Piece, List<Tile>>();
        public delegate void PawnPromotionStarted(PieceColor color);
        public event PawnPromotionStarted onPawnPromotionStarted;
        private bool waitingForPawnPromotion = false;
        private ChessMove incompletePromotionMove;

        public bool CanPieceMove(PieceColor color)
        {
            bool canItMove = (gameManager.GetGameState() == GameState.WhiteMove && color == PieceColor.White)
                || (gameManager.GetGameState() == GameState.BlackMove && color == PieceColor.Black);
            return canItMove;
        }

        public MoveManager(ChessGameManager gameManager, Board board)
        {
            this.gameManager = gameManager;
            this.board = board;
        }
        public void RegisterAllPieces(List<Piece> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                RegisterPiece(pieces[i]);
            }
            ChangeThreatenedSpaces();
        }
        public void RegisterPiece(Piece piece)
        {
            pieces.Add(piece);
            //ChangeThreatenedSpaces();
        } 
        public void RemovePiece(Piece piece)
        {
            pieces.Remove(piece);
            if (piece.GetColor() == PieceColor.White)
            {
                whiteThreatenedSpaces.Remove(piece);
            } else
            {
                blackThreatenedSpaces.Remove(piece);
            }
        }
        private void ChangeThreatenedSpaces()
        {
            GetThreatenedSpaces(pieces, whiteThreatenedSpaces, blackThreatenedSpaces);
        }
        private void GetThreatenedSpaces(List<Piece> pieces, Dictionary<Piece, List<Tile>> whiteThreatened, Dictionary<Piece, List<Tile>> blackThreatened)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].GetColor() == PieceColor.White)
                {
                    if (!whiteThreatened.ContainsKey(pieces[i]))
                    {
                        whiteThreatened.Add(pieces[i], pieces[i].GetThreathenedTiles(board));
                    }
                    else
                    {
                        whiteThreatened[pieces[i]] = pieces[i].GetThreathenedTiles(board);
                    }
                }
                else
                {
                    if (!blackThreatened.ContainsKey(pieces[i]))
                    {
                        blackThreatened.Add(pieces[i], pieces[i].GetThreathenedTiles(board));
                    }
                    else
                    {
                        blackThreatened[pieces[i]] = pieces[i].GetThreathenedTiles(board);
                    }
                }
            }
        }
        public List<ChessMove> AvailableMoves(Piece piece)
        {
            List<ChessMove> moves = FilterKingInCheckMoves(piece.GetColor(), piece.AvailableMoves(board));
            return moves;
        }

        public List<Tile> GetThreatenedSpaces(PieceColor defendingColor)
        {
            Dictionary<Piece, List<Tile>> curThreathened;
            if (defendingColor == PieceColor.Black)
            {
                curThreathened = whiteThreatenedSpaces;
            } else
            {
                curThreathened = blackThreatenedSpaces;
            }
            List<Tile> threatenedTiles = new List<Tile>();
            foreach (KeyValuePair<Piece, List<Tile>> threatPair in curThreathened)
            {
                threatenedTiles = threatenedTiles.Union(threatPair.Value).ToList();
            }
            return threatenedTiles;
        }

        public void DoMove(ChessMove move, bool simulate = false)
        {
            if (!simulate && !CanPieceMove(move.activePiece.GetColor()))
            {
                return;
            }
            bool moveCanBeMade = true;
            if (!simulate)
            {
                if (move.isPromotionMove)
                {
                    if (!move.isCompletePromotionMove)
                    {
                        waitingForPawnPromotion = true;
                        incompletePromotionMove = move;
                        moveCanBeMade = false;
                        if (onPawnPromotionStarted != null)
                        {
                            onPawnPromotionStarted(move.activePiece.GetColor());
                        }
                    } else
                    {
                        move.promotedPiece = gameManager.CreateNewPiece(move.promotionType, move.activePiece.GetColor(), move.toTile.row, move.toTile.col);
                        move.promotedPiece.SetTile(move.toTile, !simulate);
                        move.activePiece.DisablePiece();
                    }
                }
            }
            if (moveCanBeMade)
            {
                if (!simulate)
                {
                    moveHistory.Add(move);
                }
                if (!move.isPromotionMove)
                {
                    move.activePiece.SetTile(move.toTile, !simulate);
                }

                if (move.isCastleMove)
                {
                    move.castlePair.SetTile(move.castlePairTo, !simulate);
                }
                else if (move.passivePiece != null)
                {
                    move.passivePiece.Capture(!simulate);
                }
                if (!simulate)
                {
                    gameManager.ChangeGameState();
                }
                ChangeThreatenedSpaces();
            }
        }

        public void UndoMove(ChessMove move, bool simulate = false)
        {
            if (!simulate && !CanPieceMove(move.activePiece.GetColor()))
            {
                return;
            }
            if (!simulate && move.isPromotionMove)
            {
                move.activePiece.EnablePiece();
                move.promotedPiece.DisablePiece();
            }
            if (!simulate)
            {
                moveHistory.Remove(move);
            }
            move.activePiece.SetTile(move.fromTile, !simulate);
            if (move.isCastleMove)
            {
                move.castlePair.SetTile(move.castlePairFrom, !simulate);
            }
            else if (move.passivePiece != null)
            {
                move.passivePiece.SetTile(move.passivePieceFromTile, !simulate);
                move.passivePiece.RestoreCaptured(!simulate);
            }
            if (!simulate)
            {
                gameManager.ChangeGameState();
            }
            ChangeThreatenedSpaces();
        }

        public List<ChessMove> GetCastlingMoves(PieceColor color, out bool queenSideAvailable, out bool kingSideAvailable)
        {
            Tile queenSideRookTile, kingSideRookTile, kingTile;
            Tile kingQueenSideDst, kingKingSideDst, rookQueenSideDst, rookKingSideDst;
            queenSideAvailable = true;
            kingSideAvailable = true;
            List<Tile> threatenedSpaces = GetThreatenedSpaces(color);
            List<ChessMove> castleMoves = new List<ChessMove>();

            if (color == PieceColor.White)
            {
                kingTile = board.GetTile(Row._1, Column.E);

            } else
            {
                kingTile = board.GetTile(Row._8, Column.E);
            }
            kingQueenSideDst = board.GetTile(kingTile.row, Column.C);
            rookQueenSideDst = board.GetTile(kingTile.row, Column.D);
            kingKingSideDst = board.GetTile(kingTile.row, Column.G);
            rookKingSideDst = board.GetTile(kingTile.row, Column.F);
            queenSideRookTile = board.GetTile(kingTile.row, Column.A);
            kingSideRookTile = board.GetTile(kingTile.row, Column.H);

            for (int i = 1; i < 7; i++)
            {
                Tile currentTile = board.GetTile((int)kingTile.row, i);
                if (((i != 4) && currentTile.IsTileOccupied())
                    || threatenedSpaces.Contains(currentTile)) { 
                    if (i == 4)
                    {
                        queenSideAvailable = false;
                        kingSideAvailable = false;
                        return castleMoves;
                    } else if (i < 4)
                    {
                        queenSideAvailable = false;                        
                    } else
                    {
                        kingSideAvailable = false;
                    }
                }
            }

            for (int i = 0; i < moveHistory.Count && (queenSideAvailable || kingSideAvailable); i++)
            {
                Piece activePiece = moveHistory[i].activePiece;
                if (activePiece.GetColor() == color)
                {
                    //Were they moved
                    if (activePiece.GetPieceType() == PieceType.King)
                    {
                        queenSideAvailable = false;
                        kingSideAvailable = false;
                    } else if (activePiece.GetPieceType() == PieceType.Rook)
                    {
                        if (activePiece.initialTile == queenSideRookTile)
                        {
                            queenSideAvailable = false;
                        } else if (activePiece.initialTile == kingSideRookTile)
                        {
                            kingSideAvailable = false;
                        }
                    }
                }
            }
            if (kingSideAvailable)
            {


                ChessMove kingChessMove = new ChessMove(kingTile, kingKingSideDst, kingSideRookTile, rookKingSideDst);
                castleMoves.Add(kingChessMove);
            }
            if (queenSideAvailable)
            {

                ChessMove kingChessMove = new ChessMove(kingTile, kingQueenSideDst, queenSideRookTile, rookQueenSideDst);
                castleMoves.Add(kingChessMove);
            }
            return castleMoves;
        }

        private bool IsKingInCheck(PieceColor color)
        {
            List<Tile> threathenedSpaces = GetThreatenedSpaces(color);
            for (int i = 0; i < threathenedSpaces.Count; i++)
            {
                if (threathenedSpaces[i].IsTileOccupied() 
                    && threathenedSpaces[i].GetPieceColor() == color
                    && threathenedSpaces[i].piece.GetPieceType() == PieceType.King)
                {
                    return true;
                }
            }
            return false;
        }

        private List<ChessMove> FilterKingInCheckMoves(PieceColor color, List<ChessMove> moves)
        {
            List<ChessMove> filteredMoves = new List<ChessMove>();
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i], true);
                if (!IsKingInCheck(color))
                {
                    filteredMoves.Add(moves[i]);
                }
                UndoMove(moves[i], true);
            }
            return filteredMoves;
        }

        public void GetMoveCount(out int moveCountWhite, out int moveCountBlack)
        {
            moveCountWhite = 0;
            moveCountBlack = 0;
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].GetColor() == PieceColor.White)
                {
                    moveCountWhite += AvailableMoves(pieces[i]).Count;
                }
                else 
                {
                    moveCountBlack += AvailableMoves(pieces[i]).Count;
                }
            }
        }

        public ChessMove GetLastMove()
        {
            return moveHistory[moveHistory.Count - 1];
        }

        public void PromotionPawnTypeSelected(PieceType type)
        {
            if (waitingForPawnPromotion)
            {
                waitingForPawnPromotion = false;
                incompletePromotionMove.isCompletePromotionMove = true;
                incompletePromotionMove.promotionType = type;
                DoMove(incompletePromotionMove);
            }
        }
    }
}