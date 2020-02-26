using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NBPChess
{
    public static class AlgebraicNotation
    {
        private static Dictionary<PieceType, string> pieceNames = new Dictionary<PieceType, string>()
        {
            { PieceType.Pawn, "" },
            { PieceType.Rook, "R" },
            { PieceType.Knight, "N" },
            { PieceType.Bishop, "B" },
            { PieceType.Queen, "Q" },
            { PieceType.King, "K" }
        };

        public static string ParseMove(ChessMove move, MoveManager moveManager)
        {
            string moveString = "";
            //CheckForCastling
            if (move.isCastleMove)
            {
                if (move.isQueenSideCastle)
                {
                    moveString = "0-0-0";
                } else
                {
                    moveString = "0-0";
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
                moveStrBuilder.Append('+');
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
                moveStrBuilder.Append('x');
            }
            moveStrBuilder.Append(dstFile);
            moveStrBuilder.Append(dstRank);
            //Promotion
            if (move.isPromotionMove)
            {
                moveStrBuilder.Append('=');
                moveStrBuilder.Append(pieceNames[move.promotionType]);
            }
            if (isEnPassantMove)
            {
                moveStrBuilder.Append("e.p.");
            }
            moveString = moveStrBuilder.ToString();

            return moveString;
        }
    }
}

