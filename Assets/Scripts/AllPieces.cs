using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess
{
    public class Pawn : Piece
    {
        public Pawn(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Pawn;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();
            //Front movement
            Tile nextTile;
            if (IsRelTileEmpty(1, 0, board, out nextTile))
            {
                moves.Add(nextTile);

                if (currentTile.row == GetRowColorRelative(1))
                {
                    Tile twoTilesAhead;
                    if (IsRelTileEmpty(2, 0, board, out twoTilesAhead))
                    {
                        moves.Add(twoTilesAhead);
                    }
                }
            }
            //Left Attack
            Tile leftAttackTile;
            if (IsRelTileEnemy(1, -1, board, out leftAttackTile))
            {
                moves.Add(leftAttackTile);
            }
            //Right Attack
            Tile rightAttackTile;
            if (IsRelTileEnemy(1, 1, board, out rightAttackTile))
            {
                moves.Add(rightAttackTile);
            }
            //TODO: ADD PROMOTION LOGIC
            return moves;
        }
        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();

            //Left Attack
            Tile leftAttackTile;
            if (IsRelTileOnBoard(1, -1, board, out leftAttackTile))
            {
                moves.Add(leftAttackTile);
            }
            //Right Attack
            Tile rightAttackTile;
            if (IsRelTileOnBoard(1, 1, board, out rightAttackTile))
            {
                moves.Add(rightAttackTile);
            }

            return moves;
        }
    }
    public class Rook : Piece
    {
        private int[] directions = new int[8] { -1, 0, 1, 0, 0, -1, 0, 1 };

        public Rook(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }

        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileOnBoard(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class Bishop : Piece
    {
        private int[] directions = new int[8] { -1, -1, 1, 1, 1, -1, -1, 1 };

        public Bishop(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }
        public override PieceType GetPieceType()
        {
            return PieceType.Bishop;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }

        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 4; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileOnBoard(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class Knight : Piece
    {
        private int[] directions = new int[16]
            {
                2, 1, 2, -1,
                -2, 1, -2, -1,
                1, 2, 1, -2,
                -1, 2, -1, -2
            };
        public Knight(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Knight;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();


            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                if (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                }
            }
            return moves;
        }
        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();


            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                if (IsRelTileOnBoard(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                }
            }
            return moves;
        }
    }

    public class Queen : Piece
    {
        private int[] directions = new int[16] {
                //Bishop
                -1, -1, 1, 1, 1, -1, -1, 1,
                //Rook
                -1, 0, 1, 0, 0, -1, 0, 1
            };
        public Queen(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }

        public override PieceType GetPieceType()
        {
            return PieceType.Queen;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }

        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                while (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                    if (newTile.IsTileOccupied())
                    {
                        break;
                    }
                    rowOffset += directions[2 * i];
                    colOffset += directions[2 * i + 1];
                }
            }
            return moves;
        }
    }

    public class King : Piece
    {
        private int[] directions = new int[16] {
            //Bishop
            -1, -1, 1, 1, 1, -1, -1, 1,
            //Rook
            -1, 0, 1, 0, 0, -1, 0, 1
        };
        public King(PieceColor color, Tile tile, MoveManager moveManager) : base(color, tile, moveManager) { }

        public override PieceType GetPieceType()
        {
            return PieceType.King;
        }
        public override List<Tile> AvailableMoves(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                //TODO: ADD CASTLING LOGIC
                List<Tile> threathenedSpaces = moveManager.GetThreatenedSpaces(pieceColor);
                if (IsRelTileEmptyOrEnemy(rowOffset, colOffset, board, out newTile) && !threathenedSpaces.Contains(newTile))
                {
                    moves.Add(newTile);
                }
            }

            return moves;
        }

        public override List<Tile> GetThreathenedTiles(Board board)
        {
            List<Tile> moves = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int rowOffset = directions[2 * i];
                int colOffset = directions[2 * i + 1];
                Tile newTile;
                if (IsRelTileOnBoard(rowOffset, colOffset, board, out newTile))
                {
                    moves.Add(newTile);
                }
            }

            return moves;
        }
    }
}


