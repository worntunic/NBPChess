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
        public abstract PieceType GetPieceType();
		protected PieceColor pieceColor;
        protected Tile currentTile;

        public Piece(PieceColor color, Tile tile)
        {
            this.pieceColor = color;
            SetTile(tile);
        }

        public void SetTile(Tile newTile)
        {
            Tile oldTile = currentTile;
            if (currentTile != null)
            {
                currentTile.RemoveCurrentPiece();
            }
            currentTile = newTile;
            currentTile.SetCurrentPiece(this);
            //pieceMoved(oldTile, newTile);
        }

		public abstract List<Tile> AvailableMoves(Board board);
		public PieceColor GetColor()
		{
			return pieceColor;
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

			if (pieceColor == PieceColor.White)
			{
				Tile nextTile = board.GetTile(currentTile.row + 1, currentTile.col);
				if (!nextTile.IsTileOccupied())
				{
					moves.Add(nextTile);

					if(currentTile.row == Row._2)
					{
						Tile nextNextTile = board.GetTile(Row._4, currentTile.col);
						if (!nextNextTile.IsTileOccupied())
						{
							moves.Add(nextNextTile);
						}
					}
				}
			}
			return moves;
		}
	}
    public class Rook : Piece
    {
        public Rook(PieceColor color, Tile tile) : base(color, tile) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Bishop : Piece
    {
        public Bishop(PieceColor color, Tile tile) : base(color, tile) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Bishop;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Knight : Piece
    {
        public Knight(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Knight;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Queen : Piece
    {
        public Queen(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Queen;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }

    public class King : Piece
    {
        public King(PieceColor color, Tile tile) : base(color, tile) { }

        public override PieceType GetPieceType()
        {
            return PieceType.King;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}

