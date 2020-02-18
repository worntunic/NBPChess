using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
	public enum Column { A, B, C, D, E, F, G, H };
	public enum Row
	{
		_1, _2, _3, _4, _5, _6, _7, _8
	}
	public enum TileColor
	{
		White, Black
	}

	public class Tile
    {
        public Column col { get; }
        public Row row { get; }
        public Piece piece;
        public TileColor tileColor { get; }

		public Tile(int row, int col, Piece piece = null)
		{
            this.row = (Row)row;
            this.col = (Column)col;
            if ((row + col) % 2 == 0)
            {
                tileColor = TileColor.White;
            } else
            {
                tileColor = TileColor.Black;
            }
			this.piece = piece;
		}

        public int RowNum()
        {
            return (int)row;
        }
        public int ColNum()
        {
            return (int)col;
        }

        public bool IsTileOccupied()
		{
			return piece != null;
		}
		public PieceColor GetPieceColor()
		{
			return piece.GetColor();
		}
		public bool IsEnemyPiece(PieceColor currentPlayer)
		{
			return GetPieceColor() != currentPlayer;
		}
        public void SetCurrentPiece(Piece piece)
        {
            this.piece = piece;
        }
        public void RemoveCurrentPiece()
        {
            this.piece = null;
        }
        public override string ToString()
        {
            return col.ToString() + row.ToString();
        }
    }
}
