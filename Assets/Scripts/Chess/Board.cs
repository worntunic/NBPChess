using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NBPChess
{
	public class Board : MonoBehaviour
	{
		public Tile[,] board;
		public TileUI model;
        private Dictionary<Tile, TileUI> tilesWithUI = new Dictionary<Tile, TileUI>();
        private ChessArtSet artSet;
        private RectTransform boardBoundaryRect;
        private TileUI currentlyPointedTile;
        private List<PieceUI> activePieces = new List<PieceUI>();
        private bool initialized = false;

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
            if (initialized)
            {
                foreach (Tile t in board)
                {
                    if (tilesWithUI.ContainsKey(t))
                    {
                        Destroy(tilesWithUI[t].gameObject);
                        tilesWithUI.Remove(t);
                    }
                }
            }
            this.boardBoundaryRect = (RectTransform)transform;
            GenerateBoard();
            initialized = true;
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

                    tile.Initialize(this, board[i,j], artSet.GetTileArt(board[i, j].tileColor));
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

        public TileUI[] GetTiles()
        {
            return tilesWithUI.Values.ToArray();
        }

        public bool IsPositionOnBoard(Vector2 position)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(boardBoundaryRect, position);
        }

        public void SetPointedTile(TileUI pointedTile)
        {
            currentlyPointedTile = pointedTile;
        }

        public Tile GetCurrentlyPointedTile()
        {
            return currentlyPointedTile.tile;
        }

        public void SelectTile(Tile tile)
        {
            tilesWithUI[tile].ChangeTileSelectionState(TileSelectionState.Selected);
        }

        public void DeselectTile(Tile tile)
        {
            tilesWithUI[tile].ChangeTileSelectionState(TileSelectionState.Default);
        }

        public void HighlightTile(Tile tile)
        {
            tilesWithUI[tile].ChangeTileSelectionState(TileSelectionState.Highlighted);
        }

        public void DeselectAllTiles()
        {
            foreach (KeyValuePair<Tile, TileUI> tilePair in tilesWithUI)
            {
                tilePair.Value.ChangeTileSelectionState(TileSelectionState.Default);
            }
        }

        public void SetRayBlockingForAllPieces(bool shouldBlockRays)
        {
            for (int i = 0; i < activePieces.Count; i++)
            {
                activePieces[i].SetRayBlocking(shouldBlockRays);
            }
        }

        public void RegisterPiece(PieceUI piece)
        {
            activePieces.Add(piece);
        }

        public void RemovePiece(PieceUI piece)
        {
            activePieces.Remove(piece);
        }

    }
}

