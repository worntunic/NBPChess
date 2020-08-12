using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NBPChess
{
    public static class AlgebraicNotation
    {
        private const string queenSideCastle = "0-0-0";
        private const string kingSideCastle = "0-0";
        private const char oppKingInCheck = '+';
        private const char capturedPiece = 'x';
        private const char promotion = '=';
        private const string enPassant = "e.p.";

        private static Dictionary<PieceType, string> pieceNames = new Dictionary<PieceType, string>()
        {
            { PieceType.Pawn, "" },
            { PieceType.Rook, "R" },
            { PieceType.Knight, "N" },
            { PieceType.Bishop, "B" },
            { PieceType.Queen, "Q" },
            { PieceType.King, "K" }
        };

        public static string ToAlgebraic(ChessMove move, MoveManager moveManager)
        {
            string moveString = "";
            //CheckForCastling
            if (move.isCastleMove)
            {
                if (move.isQueenSideCastle)
                {
                    moveString = queenSideCastle;

                } else
                {
                    moveString = kingSideCastle;
                }
                return moveString;
            }
            PieceColor oppColor = (move.activePiece.GetColor() == PieceColor.White) ? PieceColor.Black : PieceColor.White; 
            //Not castle move
            string dstFile = move.toTile.col.ToString().ToLower();
            string dstRank = ((int)move.toTile.row + 1).ToString();
            string pieceName = pieceNames[move.activePiece.GetPieceType()];
            bool ambiguityByFile = false, ambiguityByRank = false;
            bool isOpponentsKingInCheck = move.checksOpponent;
            bool isEnPassantMove = 
                move.activePiece.GetPieceType() == PieceType.Pawn 
                && move.passivePiece != null 
                && move.passivePiece.GetTile() != move.toTile;

            StringBuilder moveStrBuilder = new StringBuilder();

            List<ChessMove> allMovesForSamePieceType = moveManager.AvailableMovesForSamePieceExcluded(move.activePiece);
            for (int i = 0; i < allMovesForSamePieceType.Count; i++)
            {
                //Debug.Log(allMovesForSamePieceType[i].toTile);
                if (allMovesForSamePieceType[i].toTile == move.toTile)
                {

                    if (allMovesForSamePieceType[i].fromTile.col != move.fromTile.col)
                    {
                        ambiguityByRank = true;
                    } else
                    {
                        ambiguityByFile = true;
                    }
                }
            }

            if (isOpponentsKingInCheck)
            {
                moveStrBuilder.Append(oppKingInCheck);
            }
            moveStrBuilder.Append(pieceName);
            if (ambiguityByRank || (move.activePiece.GetPieceType() == PieceType.Pawn && move.passivePiece != null))
            {
                moveStrBuilder.Append(move.fromTile.col.ToString().ToLower());
            }
            if (ambiguityByFile && !isEnPassantMove)
            {
                moveStrBuilder.Append(((int)move.fromTile.row + 1).ToString());
            }
            //Capture
            if (move.passivePiece != null)
            {
                moveStrBuilder.Append(capturedPiece);
            }
            moveStrBuilder.Append(dstFile);
            moveStrBuilder.Append(dstRank);
            //Promotion
            if (move.isPromotionMove)
            {
                moveStrBuilder.Append(promotion);
                moveStrBuilder.Append(pieceNames[move.promotionType]);
            }
            if (isEnPassantMove)
            {
                moveStrBuilder.Append(enPassant);
            }
            moveString = moveStrBuilder.ToString();

            return moveString;
        }

        public static ChessMove ToChessMove(string algebraicMove, MoveManager moveManager, PieceColor color)
        {
            PieceColor oppColor;
            if (color == PieceColor.White)
            {
                oppColor = PieceColor.Black;
            } else
            {
                oppColor = PieceColor.White;
            }
            string modAlgMove = algebraicMove;
            //Strip not checked
            /*int curChar = 0;
            if (modAlgMove[curChar] == oppKingInCheck)
            {
                //curChar++;
                modAlgMove = modAlgMove.Substring(1);
            }*/
            bool promotionMove = false;
            PieceType promotionType = PieceType.Queen;
            if (algebraicMove[algebraicMove.Length - 2] == promotion)
            {
                modAlgMove = algebraicMove.Substring(0, algebraicMove.Length - 1);
                promotionMove = true;
                promotionType = GetPieceName(algebraicMove[algebraicMove.Length - 1].ToString());
            }
            List<ChessMove> allMoves = moveManager.AllAvailableMoves(color);
            foreach (ChessMove move in allMoves)
            {
                ChessMove fullMove = moveManager.DoMove(move, true, false);
                fullMove.checksOpponent = moveManager.IsKingInCheck(oppColor);
                moveManager.UndoMove(fullMove, true, false);
                string genMove = ToAlgebraic(fullMove, moveManager);
                if (genMove == modAlgMove)
                {
                    if (promotionMove)
                    {
                        fullMove.isPromotionMove = true;
                        fullMove.isCompletePromotionMove = true;
                        fullMove.promotionType = promotionType;
                    }
                    return fullMove;
                }
            }
            throw new Exception($"Move {algebraicMove} cannot be performed");
            /*ChessMove chessMove = new ChessMove();
            if (string.Equals(algebraicMove, queenSideCastle))
            {
                return moveManager.GetCastlingMove(color, true);
            } else if (string.Equals(algebraicMove, kingSideCastle))
            {
                return moveManager.GetCastlingMove(color, false);
            }
            //Not a castle move
            PieceColor oppColor = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            //Is opponents king in check
            int curChar = 0;
            if (algebraicMove[curChar] == oppKingInCheck)
            {
                chessMove.checksOpponent = true;
                curChar++;
            } else
            {
                chessMove.checksOpponent = false;
            }

            PieceType type = GetPieceName(algebraicMove[curChar].ToString());
            if (type != PieceType.Pawn)
            {
                curChar++;
            }


            Column srcCol, dstCol;
            Row srcRow, dstRow;
            bool srcColPresent = false, srcRowPresent = false;
            //If there's ambiguity by rank
            if (srcColPresent = Enum.TryParse(algebraicMove[curChar].ToString(), out srcCol))
            {
                curChar++;
            }
            //If there's ambiguity by file
            if (srcRowPresent = Enum.TryParse(algebraicMove[curChar].ToString(), out srcRow))
            {
                curChar++;
            }
            //If there's captured piece
            if (String.Equals(algebraicMove[curChar], capturedPiece))
            {
                curChar++;
            }
            //Destination
            dstCol = (Column)Enum.Parse(typeof(Column), algebraicMove[curChar].ToString());
            curChar++;
            dstRow = (Row)Enum.Parse(typeof(Row), algebraicMove[curChar].ToString());
            curChar++;
            //Get all moves
            List<ChessMove> allMoves = moveManager.AllAvailableMoves(color);
            foreach (ChessMove move in allMoves)
            {
                if (move.toTile.col == dstCol && move.toTile.row == dstRow
                    && (!srcColPresent || move.fromTile.col == srcCol)
                    && (!srcRowPresent || move.fromTile.row == srcRow))
                {
                    chessMove = move;
                    break;
                }
            }
            //Promotion
            if (String.Equals(algebraicMove[curChar], promotion))
            {
                curChar++;

                chessMove.isCompletePromotionMove = true;
                chessMove.isPromotionMove = true;
                chessMove.promotionType = GetPieceName(algebraicMove[curChar].ToString());
                curChar++;
            }
            return chessMove;*/
        }

        private static PieceType GetPieceName(string pieceName)
        {
            foreach (KeyValuePair<PieceType, string> pair in pieceNames)
            {
                if (pair.Value == pieceName)
                {
                    return pair.Key;
                }
            }
            return PieceType.Pawn;
            //throw new System.Exception($"Piece type for string {pieceName} not found");
        }
    }
}

