using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
	public class Board : MonoBehaviour
	{
		public Tile[,] board;
		public Piece[] pieces;
		public TileUI model;

		public void Awake()
		{
			GenerateBoard();
		}

		public void GenerateBoard()
		{
			board = new Tile[8,8];
			for(int i = 0; i < 8; i++)
			{
				for(int j = 0; j < 8; j++)
				{
					board[i, j] = new Tile(i, j);
					TileUI tile = Instantiate(model, transform);
					tile.Initialize(board[i,j].tileID);
				}
			}
		}

		public Tile GetTile(int row, int col)
		{
			return board[row, col];
		}
		public Tile GetTile(Row row, Column col)
		{
			return GetTile((int) row, (int) col);
		}
	}
}

