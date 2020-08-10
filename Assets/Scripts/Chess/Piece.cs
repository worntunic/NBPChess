using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    public enum PieceColor
    {
        Black, White
    }
    public enum PieceType
    {
        Pawn, Rook, Knight, Bishop, Queen, King
    }
    public static class PieceFactory
    {

        public static Piece CreatePieceByType(PieceType pieceType, PieceColor pieceColor, Tile tile, MoveManager moveManager)
        {
            switch (pieceType)
            {
                case (PieceType.Pawn):
                    return new Pawn(pieceColor, tile, moveManager);
                case (PieceType.Rook):
                    return new Rook(pieceColor, tile, moveManager);
                case (PieceType.Knight):
                    return new Knight(pieceColor, tile, moveManager);
                case (PieceType.Bishop):
                    return new Bishop(pieceColor, tile, moveManager);
                case (PieceType.Queen):
                    return new Queen(pieceColor, tile, moveManager);
                case (PieceType.King):
                    return new King(pieceColor, tile, moveManager);
            }
            throw new System.Exception("Can't create piece because there is no defined class for it:" + pieceType.ToString());
        }
    }

    public abstract class Piece
    {
        public delegate void PieceMoved(Tile oldTile, Tile newTile);
        public event PieceMoved pieceMoved;
        public delegate void PieceCaptureStateChanged(bool captured);
        public event PieceCaptureStateChanged pieceCapturedStateChanged;
        public delegate void PieceEnabledStateChanged(bool enabled);
        public event PieceEnabledStateChanged pieceEnabledStateChanged;
        public abstract PieceType GetPieceType();
        protected PieceColor pieceColor;
        public Tile initialTile { get; private set; }
        protected Tile currentTile;
        protected bool captured = false;
        protected MoveManager moveManager;
        protected bool pieceEnabled = true;

        public Piece(PieceColor color, Tile tile, MoveManager moveManager)
        {
            this.pieceColor = color;
            this.moveManager = moveManager;
            initialTile = tile;
            SetTile(tile);
            moveManager.RegisterPiece(this);
        }
        public Tile GetTile()
        {
            return currentTile;
        }

        public void SetTile(Tile newTile, bool triggerEvent = true)
        {
            Tile oldTile = currentTile;
            if (currentTile != null)
            {
                currentTile.RemoveCurrentPiece();
            }
            currentTile = newTile;
            currentTile.SetCurrentPiece(this);
            if (triggerEvent && pieceMoved != null)
            {
                pieceMoved(oldTile, newTile);
            }
        }



        public abstract List<ChessMove> AvailableMoves(Board board);
        public abstract List<Tile> GetThreathenedTiles(Board board);
        public PieceColor GetColor()
        {
            return pieceColor;
        }

        protected bool IsTileOnBoard(int row, int column)
        {
            return (row >= 0 && row < 8 && column >= 0 && column < 8);
        }

        protected bool IsRelTileOnBoard(int rowOffset, int colOffset, Board board, out Tile newTile)
        {
            int newRow, newCol;
            if (pieceColor == PieceColor.White)
            {
                newRow = (int)currentTile.row + rowOffset;
                newCol = (int)currentTile.col + colOffset;
            }
            else
            {
                newRow = (int)currentTile.row - rowOffset;
                newCol = (int)currentTile.col - colOffset;
            }
            bool isTileOnBoard = IsTileOnBoard(newRow, newCol);
            if (isTileOnBoard)
            {
                newTile = board.GetTile(newRow, newCol);
            }
            else
            {
                newTile = null;
            }
            return isTileOnBoard;
        }

        protected bool IsRelTileEmpty(int rowOffset, int colOffset, Board board, out Tile newTile)
        {
            bool isTileOnBoard = IsRelTileOnBoard(rowOffset, colOffset, board, out newTile);
            if (isTileOnBoard)
            {
                return !newTile.IsTileOccupied();
            }
            return false;
        }

        protected bool IsRelTileEnemy(int rowOffset, int colOffset, Board board, out Tile newTile)
        {
            bool isTileOnBoard = IsRelTileOnBoard(rowOffset, colOffset, board, out newTile);
            if (isTileOnBoard)
            {
                return newTile.IsTileOccupied() && newTile.piece.GetColor() != this.pieceColor;
            }
            return false;
        }

        protected bool IsRelTileEmptyOrEnemy(int rowOffset, int colOffset, Board board, out Tile newTile)
        {
            bool isTileOnBoard = IsRelTileOnBoard(rowOffset, colOffset, board, out newTile);
            if (isTileOnBoard)
            {
                return !newTile.IsTileOccupied() || newTile.GetPieceColor() != pieceColor;
            }
            return false;
        }

        protected Row GetRowColorRelative(int distanceFromRelativeBottom)
        {
            if (pieceColor == PieceColor.White)
            {
                return (Row)distanceFromRelativeBottom;
            }
            else
            {
                return (Row)(7 - distanceFromRelativeBottom);
            }
        }

        protected Column GetColumnColorRelative(int distanceFromRelativeLeft)
        {
            if (pieceColor == PieceColor.White)
            {
                return (Column)distanceFromRelativeLeft;
            }
            else
            {
                return (Column)(7 - distanceFromRelativeLeft);
            }
        }

        public void Capture(bool triggerEvents = true)
        {
            captured = true;
            currentTile.RemovePiece(this);
            if (pieceCapturedStateChanged != null && triggerEvents)
            {
                pieceCapturedStateChanged(captured);
            }
            moveManager.RemovePiece(this);
        }

        public void RestoreCaptured(bool triggerEvents = true)
        {
            captured = false;
            currentTile.SetCurrentPiece(this);
            if (pieceCapturedStateChanged != null && triggerEvents)
            {
                pieceCapturedStateChanged(captured);
            }
            moveManager.RegisterPiece(this);
        }

        protected ChessMove CreateMoveTo(Tile toTile)
        {
            return new ChessMove(currentTile, toTile);
        }

        public void DisablePiece()
        {
            pieceEnabled = false;
            currentTile.RemovePiece(this);
            if (pieceEnabledStateChanged != null)
            {
                pieceEnabledStateChanged(pieceEnabled);
            }
            moveManager.RemovePiece(this);
        }

        public void EnablePiece()
        {
            pieceEnabled = true;
            currentTile.SetCurrentPiece(this);
            if (pieceEnabledStateChanged != null)
            {
                pieceEnabledStateChanged(pieceEnabled);
            }
            moveManager.RegisterPiece(this);
        }
    }
}
