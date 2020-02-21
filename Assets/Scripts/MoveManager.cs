using System.Collections;
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

        public ChessMove(Tile fromTile, Tile toTile)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
            isCastleMove = false;
            castlePair = null;
            castlePairFrom = null;
            castlePairTo = null;
        }

        public ChessMove(Tile fromTile, Tile toTile, Tile castlePairFrom, Tile castlePairTo)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
            isCastleMove = true;
            this.castlePair = castlePairFrom.piece;
            this.castlePairFrom = castlePairFrom;
            this.castlePairTo = castlePairTo;
        }
    }

    public class MoveManager
    {
        private Board board;
        private List<Piece> pieces = new List<Piece>();
        private List<ChessMove> moveHistory = new List<ChessMove>();
        private Dictionary<Piece, List<Tile>> whiteThreatenedSpaces = new Dictionary<Piece, List<Tile>>();
        private Dictionary<Piece, List<Tile>> blackThreatenedSpaces = new Dictionary<Piece, List<Tile>>();

        public MoveManager(Board board)
        {
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
            ChangeThreatenedSpaces();
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
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].GetColor() == PieceColor.White)
                {
                    if (!whiteThreatenedSpaces.ContainsKey(pieces[i]))
                    {
                        whiteThreatenedSpaces.Add(pieces[i], pieces[i].GetThreathenedTiles(board));
                    } else
                    {
                        whiteThreatenedSpaces[pieces[i]] = pieces[i].GetThreathenedTiles(board);
                    }
                } else
                {
                    if (!blackThreatenedSpaces.ContainsKey(pieces[i]))
                    {
                        blackThreatenedSpaces.Add(pieces[i], pieces[i].GetThreathenedTiles(board));
                    }
                    else
                    {
                        blackThreatenedSpaces[pieces[i]] = pieces[i].GetThreathenedTiles(board);
                    }
                }
            }
            /*Dictionary<Piece, List<Tile>> curThreatenedTiles;
            if (movedPiece.GetColor() == PieceColor.White)
            {
                curThreatenedTiles = whiteThreatenedSpaces;
            } else
            {
                curThreatenedTiles = blackThreatenedSpaces;
            }
            if (!curThreatenedTiles.ContainsKey(movedPiece))
            {
                curThreatenedTiles.Add(movedPiece, movedPiece.GetThreathenedTiles(board));
            }
            else
            {
                curThreatenedTiles[movedPiece] = movedPiece.GetThreathenedTiles(board);
            }*/
        }
        public List<ChessMove> AvailableMoves(Piece piece)
        {
            return piece.AvailableMoves(board);
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

        public void DoMove(ChessMove move)
        {
            moveHistory.Add(move);
            move.fromTile.piece.SetTile(move.toTile);
            if (move.isCastleMove)
            {
                move.castlePair.SetTile(move.castlePairTo);
            } else if (move.passivePiece != null)
            {
                move.passivePiece.Capture();
            }
            ChangeThreatenedSpaces();
        }

        public void UndoMove(ChessMove move)
        {
            moveHistory.Remove(move);
            move.activePiece.SetTile(move.fromTile);
            if (move.isCastleMove)
            {
                move.castlePair.SetTile(move.castlePairFrom);
            }
            else if (move.passivePiece != null)
            {
                move.passivePiece.RestoreCaptured();
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
                    } else if (activePiece.GetPieceType() == PieceType.Rook && !activePiece.createdByPromotion)
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
    }
}