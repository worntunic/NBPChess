﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    [System.Serializable]
    public struct PieceAndPrefab
    {
        public PieceType type;
        public PieceUI prefab;
    }
    public class PieceManager : MonoBehaviour
    {
        [SerializeField]
        public PieceAndPrefab[] piecePrefabs = new PieceAndPrefab[6];
        private ChessArtSet artSet;
        private Board board;
        public Transform piecesParent;
        private List<Piece> piecesState;
        private PieceType[] initialState = new PieceType[16]
        {
            PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook,
            PieceType.Pawn, PieceType.Pawn, PieceType.Pawn,  PieceType.Pawn, PieceType.Pawn, PieceType.Pawn, PieceType.Pawn, PieceType.Pawn
        };

        public void Initalize(Board board, ChessArtSet artSet)
        {
            this.board = board;
            this.artSet = artSet;
            piecesState = new List<Piece>();
            piecesState.AddRange(CreatePieces(PieceColor.White));
            piecesState.AddRange(CreatePieces(PieceColor.Black));
        }


        private List<Piece> CreatePieces(PieceColor color)
        {
            List<Piece> pieces = new List<Piece>();
            Row startingRow;
            Column startingColumn;
            int indexIncrement;
            if (color == PieceColor.White)
            {
                startingRow = Row._1;
                startingColumn = Column.A;
                indexIncrement = 1;
            } else
            {
                startingRow = Row._8;
                startingColumn = Column.H;
                indexIncrement = -1;
            }
            int currentPieceIndex = 0;
            for (int rowOffset = 0; Mathf.Abs(rowOffset) < 2; rowOffset += indexIncrement)
            {
                Row currentRow = startingRow + rowOffset;
                for (int colOffset = 0; Mathf.Abs(colOffset) < 8; colOffset += indexIncrement)
                {
                    Column currentCol = startingColumn + colOffset;
                    PieceAndPrefab currentPiece = GetPiecePrefabByType(initialState[currentPieceIndex]);
                    Piece piece = CreatePiece(color, currentRow, currentCol, currentPiece);
                    pieces.Add(piece);
                    currentPieceIndex++;
                }
            }
            return pieces;
        }

        private Piece CreatePiece(PieceColor color, Row row, Column col, PieceAndPrefab pieceAndPrefab)
        {

            PieceUI pieceUI = Instantiate(pieceAndPrefab.prefab, piecesParent);

            Tile tile = board.GetTile(row, col);
            pieceUI.Initialize(color, tile, board, artSet.GetSpriteVariants(pieceAndPrefab.type));
            return pieceUI.GetPiece();
        }

        private PieceAndPrefab GetPiecePrefabByType(PieceType type)
        {
            for (int i = 0; i < piecePrefabs.Length; i++)
            {
                if (piecePrefabs[i].type == type)
                {
                    return piecePrefabs[i];
                }
            }
            throw new Exception("There is no prefab for piece type " + type.ToString());
        }
    }
}