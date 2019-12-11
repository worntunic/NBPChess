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
	[System.Serializable]
	public struct TileID
	{

		public Column col;
		public Row row;

		public TileID(int row, int col)
		{
			this.row = (Row)row;
			this.col = (Column)col;
		}

		public int RowNum()
		{
			return (int)row;
		}
		public int ColNum()
		{
			return (int)col;
		}

		public override string ToString()
		{
			return col.ToString() + row.ToString();
		}
	}

	public class Tile
	{
		public TileID tileID;
		public Piece piece;

		public Tile(int row, int col, Piece piece = null)
		{
			tileID = new TileID(row, col);
			this.piece = piece;
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
	}
}
