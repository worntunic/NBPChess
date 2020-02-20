using System.Collections;
using System.Collections.Generic;
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
        private List<Piece> pieces = new List<Piece>();
        private List<ChessMove> moveHistory = new List<ChessMove>();

        public void RegisterPiece(Piece piece)
        {
            pieces.Add(piece);
        } 
        public void RemovePiece(Piece piece)
        {
            pieces.Remove(piece);
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
        }

        public List<Tile> GetAvailableMoves(Piece piece, Board board)
        {
            return piece.AvailableMoves(board);
        }

        public void UndoMove(ChessMove move)
        {
            moveHistory.Remove(move);
            move.activePiece.SetTile(move.fromTile);
            if (move.passivePiece != null)
            {
                move.passivePiece.RestoreCaptured();
            }
        }
    }
}

