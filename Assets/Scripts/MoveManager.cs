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

        public ChessMove(Tile fromTile, Tile toTile)
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.activePiece = fromTile.piece;
            this.passivePiece = toTile.piece;
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
        public List<Tile> GetAvailableMoves(Piece piece, Board board)
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

        public void DoMove(Tile fromTile, Tile toTile)
        {
            ChessMove newMove = new ChessMove(fromTile, toTile);
            moveHistory.Add(newMove);
            fromTile.piece.SetTile(toTile);
            if (newMove.passivePiece != null)
            {
                newMove.passivePiece.Capture();
            }
            ChangeThreatenedSpaces();
        }

        public void UndoMove(ChessMove move)
        {
            moveHistory.Remove(move);
            move.activePiece.SetTile(move.fromTile);
            if (move.passivePiece != null)
            {
                move.passivePiece.RestoreCaptured();
            }
            ChangeThreatenedSpaces();
        }
    }
}

