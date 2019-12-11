using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
	public enum PieceColor
	{
		Black, White
	}
	public abstract class Piece
	{
		protected PieceColor pieceColor;

		public abstract List<Tile> AvailableMoves(Board board, TileID occTile);
		public PieceColor GetColor()
		{
			return pieceColor;
		}
	}

	public class Peon : Piece
	{
		public override List<Tile> AvailableMoves(Board board, TileID occTile)
		{
			List<Tile> moves = new List<Tile>();

			if (pieceColor == PieceColor.White)
			{
				Tile nextTile = board.GetTile(occTile.row + 1, occTile.col);
				if (!nextTile.IsTileOccupied())
				{
					moves.Add(nextTile);

					if(occTile.row == Row._2)
					{
						Tile nextNextTile = board.GetTile(Row._4, occTile.col);
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
}

