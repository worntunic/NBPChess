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
    public static class PieceFactory {

        public static Piece CreatePieceByType(PieceType pieceType, PieceColor pieceColor, Tile tile)
        {
            switch(pieceType) {
                case (PieceType.Pawn):
                    return new Pawn(pieceColor, tile);
                case (PieceType.Rook):
                    return new Rook(pieceColor, tile);
                case (PieceType.Knight):
                    return new Knight(pieceColor, tile);
                case (PieceType.Bishop):
                    return new Bishop(pieceColor, tile);
                case (PieceType.Queen):
                    return new Queen(pieceColor, tile);
                case (PieceType.King):
                    return new King(pieceColor, tile);
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
        public abstract PieceType GetPieceType();
		protected PieceColor pieceColor;
        protected Tile currentTile;
        protected bool captured = false;

        public Piece(PieceColor color, Tile tile)
        {
            this.pieceColor = color;
            SetTile(tile);
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

		public abstract List<Tile> AvailableMoves(Board board);

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
            } else
            {
                newRow = (int)currentTile.row - rowOffset;
                newCol = (int)currentTile.col - colOffset;
            }
            bool isTileOnBoard = IsTileOnBoard(newRow, newCol);
            if (isTileOnBoard)
            {
                newTile = board.GetTile(newRow, newCol);
            } else
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

        public void Capture()
        {
            captured = true;
            pieceCapturedStateChanged(captured);
        }

        public void RestoreCaptured()
        {
            captured = false;
            pieceCapturedStateChanged(captured);
        }
    }

	public class Pawn : Piece
	{
        public Pawn(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Pawn;
        }
        public override List<Tile> AvailableMoves(Board board)
		{
			List<Tile> moves = new List<Tile>();
            //Front movement
            Tile nextTile;
			if (IsRelTileEmpty(1, 0, board, out nextTile)) {
                moves.Add(nextTile);

                if (currentTile.row == GetRowColorRelative(1))
                {
                    Tile twoTilesAhead;
                    if (IsRelTileEmpty(2, 0, board, out twoTilesAhead)) {
                        moves.Add(twoTilesAhead);
                    }
                }
            }
            //Left Attack
            Tile leftAttackTile;
            if (IsRelTileEnemy(1, -1, board, out leftAttackTile))
            {
                moves.Add(leftAttackTile);
            }
            //Right Attack
            Tile rightAttackTile;
            if (IsRelTileEnemy(1, 1, board, out rightAttackTile))
            {
                moves.Add(rightAttackTile);
            }
            //TODO: ADD PROMOTION LOGIC
			return moves;
		}
	}
    public class Rook : Piece
    {
        private int[] directions = new int[8] { -1, 0, 1, 0, 0, -1, 0, 1 };

        public Rook(PieceColor color, Tile tile) : base(color, tile) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }

        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while(IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class Bishop : Piece
    {
        private int[] directions = new int[8] { -1, -1, 1, 1, 1, -1, -1, 1 };

        public Bishop(PieceColor color, Tile tile) : base(color, tile) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Bishop;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class Knight : Piece
    {
        private int[] directions = new int[16]
            {
                2, 1, 2, -1,
                -2, 1, -2, -1,
                1, 2, 1, -2,
                -1, 2, -1, -2
            };
        public Knight(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Knight;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();


            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                if (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                }
            }
            return moves;
        }
    }

    public class Queen : Piece
    {
        private int[] directions = new int[16] {
                //Bishop
                -1, -1, 1, 1, 1, -1, -1, 1,
                //Rook
                -1, 0, 1, 0, 0, -1, 0, 1
            };
        public Queen(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Queen;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class King : Piece
    {
        private int[] directions = new int[16] {
                //Bishop
                -1, -1, 1, 1, 1, -1, -1, 1,
                //Rook
                -1, 0, 1, 0, 0, -1, 0, 1
            };
        public King(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.King;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                //TODO: ADD THREATENED LOGIC
                //TODO: ADD CASTLING LOGIC
                if (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                }
            }
            return moves;
        }
    }
}

