using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
	public class Board : MonoBehaviour
	{
		public Tile[,] board;
		public Tile model;

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

					board[i,j] = Instantiate(model, transform);
					board[i,j].Initialize(i, j);
				}
			}
		}
	}
}

