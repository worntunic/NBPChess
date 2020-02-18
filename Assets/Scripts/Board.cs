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
        private Dictionary<Tile, TileUI> tilesWithUI = new Dictionary<Tile, TileUI>();
        private ChessArtSet artSet;


        public void ChangeArtSet(ChessArtSet artSet)
        {
            this.artSet = artSet;
            foreach (KeyValuePair<Tile, TileUI> tileWithUI in tilesWithUI) {
                tileWithUI.Value.ChangeArt(artSet.GetTileArt(tileWithUI.Key.tileColor));
            }
        }
		public void Initalize(ChessArtSet artSet)
		{
            this.artSet = artSet;
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
                    tilesWithUI.Add(board[i, j], tile);

                    tile.Initialize(board[i,j], artSet.GetTileArt(board[i, j].tileColor));
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
        public TileUI GetTileUI(Tile tile)
        {
            return tilesWithUI[tile];
        }
	}
}

